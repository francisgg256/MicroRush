using UnityEngine;
using Firebase.Database;
using System;

/// <summary>
/// Controlador principal de los servicios en la nube (BaaS - Backend as a Service).
/// Implementa un patrón Singleton para mantener una conexión persistente con Firebase Realtime Database
/// a través de todas las escenas del juego.
/// </summary>
public class ControladorFirebase : MonoBehaviour
{
    /// <summary>
    /// Instancia estática única para facilitar el acceso global y el envío de datos desde cualquier punto del juego.
    /// </summary>
    public static ControladorFirebase instancia;

    /// <summary>
    /// Referencia al nodo raíz de la base de datos de Firebase.
    /// </summary>
    private DatabaseReference referenciaDB;

    /// <summary>
    /// URL específica del proyecto de Firebase (Región Europea). 
    /// Centralizar la URL facilita el mantenimiento si se migra de entorno de pruebas a producción.
    /// </summary>
    private string urlBaseDatos = "https://microrush-33724-default-rtdb.europe-west1.firebasedatabase.app";

    /// <summary>
    /// Método de inicialización temprana.
    /// Establece el Singleton, evita la destrucción del objeto al cambiar de escena 
    /// y configura la conexión inicial con el servidor remoto.
    /// </summary>
    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);

            // Inicializa la conexión utilizando la URL del servidor
            referenciaDB = FirebaseDatabase.GetInstance(urlBaseDatos).RootReference;
            Debug.Log("ControladorFirebase INICIALIZADO con URL: " + urlBaseDatos);
        }
        else
        {
            // Evita duplicados si se recarga la escena inicial
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Construye y envía de forma asíncrona el registro de puntuación del jugador a la nube.
    /// </summary>
    /// <param name="nombre">Nombre de usuario o alias del jugador.</param>
    /// <param name="puntos">Puntuación total obtenida en la partida.</param>
    public void GuardarPuntuacion(string nombre, int puntos)
    {
        // Control de errores: Previene excepciones si la conexión con el servidor falló en el Awake
        if (referenciaDB == null)
        {
            Debug.LogError("Error Crítico de Red: No hay conexión con la base de datos. No se puede guardar la puntuación.");
            return;
        }

        // Genera una clave única (Key) para el nuevo registro en el nodo "scores"
        string clave = referenciaDB.Child("scores").Push().Key;

        // Instancia el objeto de transferencia de datos (DTO)
        DatosPuntuacion nuevaPuntuacion = new DatosPuntuacion(nombre, puntos);

        // Serialización: Convierte el objeto de C# a formato JSON nativo para Firebase
        string json = JsonUtility.ToJson(nuevaPuntuacion);

        // Envía el paquete JSON de forma asíncrona al servidor
        referenciaDB.Child("scores").Child(clave).SetRawJsonValueAsync(json);
        Debug.Log("Transmisión a Firebase Exitosa -> Nombre: " + nombre + " | Puntos: " + puntos);
    }
}

/// <summary>
/// Clase contenedora (Data Transfer Object) diseñada para la serialización.
/// Representa la estructura de datos exacta que se almacenará en los documentos JSON de Firebase.
/// </summary>
[Serializable]
public class DatosPuntuacion
{
    public string player_name;
    public int score;
    public string date;

    /// <summary>
    /// Constructor de la clase de datos.
    /// Asigna automáticamente la marca de tiempo (timestamp) del sistema en el momento de creación.
    /// </summary>
    /// <param name="nombre">Nombre del jugador.</param>
    /// <param name="puntos">Puntuación alcanzada.</param>
    public DatosPuntuacion(string nombre, int puntos)
    {
        player_name = nombre;
        score = puntos;
        // Genera la fecha y hora actual en un formato estándar de base de datos
        date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}