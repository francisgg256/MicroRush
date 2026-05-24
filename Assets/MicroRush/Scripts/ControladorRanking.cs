using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Clase encargada de conectar la aplicación con la base de datos en la nube (Firebase Realtime Database).
/// Recupera de forma asíncrona las 5 puntuaciones más altas, las ordena de mayor a menor y gestiona
/// el traspaso seguro de datos entre hilos para actualizar la interfaz de clasificaciones.
/// </summary>
public class ControladorRanking : MonoBehaviour
{
    [Header("Textos de las Filas (Arrastrar desde el Inspector)")]

    /// <summary>
    /// Lista que contiene las referencias de los componentes de texto (TextMeshPro) 
    /// encargados de mostrar los nombres de los usuarios en el Top 5.
    /// </summary>
    public List<TMP_Text> textosNombres;

    /// <summary>
    /// Lista que contiene las referencias de los componentes de texto (TextMeshPro)
    /// encargados de mostrar las puntuaciones numéricas asociadas a cada jugador.
    /// </summary>
    public List<TMP_Text> textosPuntos;

    /// <summary>
    /// Referencia al nodo raíz de la base de datos distribuida de Firebase.
    /// </summary>
    private DatabaseReference referenciaDB;

    /// <summary>
    /// Dirección URL absoluta que apunta al servidor del proyecto alojado en Firebase Realtime Database (Región: europe-west1).
    /// </summary>
    private string urlBaseDatos = "https://microrush-33724-default-rtdb.europe-west1.firebasedatabase.app";

    /// <summary>
    /// Bandera (flag) de sincronización de hilos. 
    /// Indica al hilo principal (Main Thread) de Unity que la descarga asíncrona ha finalizado con éxito.
    /// </summary>
    private bool datosListos = false;

    /// <summary>
    /// Estructura de datos temporal en memoria estructurada como una lista de tuplas.
    /// Almacena de forma ordenada la información parseada del Snapshot de Firebase antes de transferirse a la UI.
    /// </summary>
    private List<(string nombre, int puntos)> listaJugadoresDescargada;

    /// <summary>
    /// Método de inicialización. Configura el nodo de red, inicializa los componentes visuales
    /// con un estado de carga pasiva e invoca la consulta de datos.
    /// </summary>
    void Start()
    {
        // Inicialización de la conexión con el endpoint de Firebase utilizando la URL específica
        referenciaDB = FirebaseDatabase.GetInstance(urlBaseDatos).RootReference;

        // Feedback de usabilidad: Se inicializan las filas con un texto de espera ("Cargando...")
        // para mitigar la percepción del tiempo de carga de red (Heurística de Nielsen: Visibilidad del estado del sistema)
        for (int i = 0; i < textosNombres.Count; i++)
        {
            textosNombres[i].text = "Cargando...";
            textosPuntos[i].text = "...";
        }

        ObtenerRanking();
    }

    /// <summary>
    /// Realiza una consulta asíncrona no bloqueante a la base de datos NoSQL.
    /// Filtra la colección "scores", ordena por la propiedad "score" y limita los resultados a los últimos 5 registros.
    /// </summary>
    public void ObtenerRanking()
    {
        // Se ejecuta una tarea en segundo plano (Task) para no congelar el videojuego durante la petición de red
        referenciaDB.Child("scores").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWith(task =>
        {
            // Control de Errores y Excepciones: Evalúa si la comunicación por red o autenticación ha fallado
            if (task.IsFaulted)
            {
                Debug.LogError("Error Crítico de Red: No se ha podido establecer conexión con Firebase Realtime Database.");
                return;
            }

            // Tras finalizar la tarea con éxito, se procesa la instantánea de datos (DataSnapshot)
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                listaJugadoresDescargada = new List<(string nombre, int puntos)>();

                // Validación defensiva: Comprueba si el nodo contiene registros válidos
                if (snapshot != null && snapshot.HasChildren)
                {
                    // Iteración sobre los documentos hijos JSON devueltos por el servidor
                    foreach (DataSnapshot hijo in snapshot.Children)
                    {
                        string nombre = hijo.Child("player_name").Value.ToString();
                        int puntos = int.Parse(hijo.Child("score").Value.ToString());

                        // Inyección estructurada en la lista local de tuplas
                        listaJugadoresDescargada.Add((nombre, puntos));
                    }
                }

                // Linq: Ordena la colección descargada de manera descendente para posicionar el récord más alto en el Top 1
                listaJugadoresDescargada = listaJugadoresDescargada.OrderByDescending(j => j.puntos).ToList();

                // Cambio de estado seguro. Se le da paso al Update (Hilo Principal) para renderizar los elementos
                datosListos = true;
            }
        });
    }

    /// <summary>
    /// Método de actualización del ciclo de vida de Unity.
    /// Monitorea de forma segura la bandera de sincronización y realiza la actualización de componentes
    /// gráficos dentro del Thread nativo de la API de Unity.
    /// </summary>
    void Update()
    {
        // Comprobación segura de hilos
        if (datosListos)
        {
            datosListos = false; // Consumo inmediato de la bandera para evitar reprocesados innecesarios

            // Mapeo estructural: Vuelca los datos dinámicos del array en las filas de la interfaz gráfica
            for (int i = 0; i < textosNombres.Count; i++)
            {
                // Si la base de datos contiene un jugador para esta posición del ranking
                if (i < listaJugadoresDescargada.Count)
                {
                    textosNombres[i].text = listaJugadoresDescargada[i].nombre;

                    // Formateo de Interfaces: El parámetro "N0" añade separadores de miles según la cultura del sistema (ej: 1,600)
                    textosPuntos[i].text = listaJugadoresDescargada[i].puntos.ToString("N0");
                }
                else
                {
                    // Control de Interfaz Desierta: Si hay menos de 5 jugadores globales registrados,
                    // rellena las filas restantes con caracteres comodín genéricos en lugar de lanzar una excepción por índice.
                    textosNombres[i].text = "---";
                    textosPuntos[i].text = "0";
                }
            }
        }
    }

    /// <summary>
    /// Evento de interfaz de usuario para regresar al Menú Principal.
    /// Recarga de forma limpia la escena base interrumpiendo cualquier proceso asíncrono restante.
    /// </summary>
    public void OnBotonMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}