using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Collections.Generic;
using System.Linq;

public class ControladorRanking : MonoBehaviour
{
    [Header("Textos de las Filas (Arrastrar desde el Inspector)")]
    public List<TMP_Text> textosNombres; // Lista para los 5 textos de nombres
    public List<TMP_Text> textosPuntos;  // Lista para los 5 textos de puntos

    private DatabaseReference referenciaDB;
    private string urlBaseDatos = "https://microrush-33724-default-rtdb.europe-west1.firebasedatabase.app";

    // Variables para pasar datos del hilo secundario (Firebase) al principal (Unity)
    private bool datosListos = false;
    private List<(string nombre, int puntos)> listaJugadoresDescargada;

    void Start()
    {
        referenciaDB = FirebaseDatabase.GetInstance(urlBaseDatos).RootReference;

        // Ponemos textos de carga temporales en las filas
        for (int i = 0; i < textosNombres.Count; i++)
        {
            textosNombres[i].text = "Cargando...";
            textosPuntos[i].text = "...";
        }

        ObtenerRanking();
    }

    public void ObtenerRanking()
    {
        referenciaDB.Child("scores").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al cargar la base de datos.");
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                listaJugadoresDescargada = new List<(string nombre, int puntos)>();

                foreach (DataSnapshot hijo in snapshot.Children)
                {
                    string nombre = hijo.Child("player_name").Value.ToString();
                    int puntos = int.Parse(hijo.Child("score").Value.ToString());
                    listaJugadoresDescargada.Add((nombre, puntos));
                }

                // Ordenamos de mayor a menor
                listaJugadoresDescargada = listaJugadoresDescargada.OrderByDescending(j => j.puntos).ToList();

                // Le avisamos al Update que ya puede pintar la UI
                datosListos = true;
            }
        });
    }

    void Update()
    {
        // Unity pinta la UI de forma segura aquí
        if (datosListos)
        {
            datosListos = false; // Lo apagamos para que solo lo haga una vez

            for (int i = 0; i < listaJugadoresDescargada.Count; i++)
            {
                // Evitamos errores si hay más datos que filas creadas
                if (i < textosNombres.Count)
                {
                    textosNombres[i].text = listaJugadoresDescargada[i].nombre;
                    // El "N0" hace que los números tengan separador de miles (ej: 56,192)
                    textosPuntos[i].text = listaJugadoresDescargada[i].puntos.ToString("N0");
                }
            }
        }
    }

    // Acuérdate de conectar esto al botón "<" de abajo a la izquierda
    public void OnBotonMenu()
    {
        SceneManager.LoadScene("MenuNuevo");
    }
}