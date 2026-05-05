using UnityEngine;
using Firebase.Database;
using System;

public class ControladorFirebase : MonoBehaviour
{
    public static ControladorFirebase instancia;
    private DatabaseReference referenciaDB;
    private string urlBaseDatos = "https://microrush-33724-default-rtdb.europe-west1.firebasedatabase.app";

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);

            referenciaDB = FirebaseDatabase.GetInstance(urlBaseDatos).RootReference;
            Debug.Log("ControladorFirebase INICIALIZADO con URL: " + urlBaseDatos);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarPuntuacion(string nombre, int puntos)
    {
        if (referenciaDB == null)
        {
            Debug.LogError("Error: No hay conexión con la base de datos.");
            return;
        }

        string clave = referenciaDB.Child("scores").Push().Key;
        DatosPuntuacion nuevaPuntuacion = new DatosPuntuacion(nombre, puntos);
        string json = JsonUtility.ToJson(nuevaPuntuacion);

        referenciaDB.Child("scores").Child(clave).SetRawJsonValueAsync(json);
        Debug.Log("Enviando a Firebase -> Nombre: " + nombre + " | Puntos: " + puntos);
    }
}

[Serializable]
public class DatosPuntuacion
{
    public string player_name;
    public int score;
    public string date;

    public DatosPuntuacion(string nombre, int puntos)
    {
        player_name = nombre;
        score = puntos;
        date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}