using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Clase principal del sistema (Game Manager).
/// Implementa el patrón de diseño Singleton para mantener un estado global y persistente a través de todas las escenas.
/// Centraliza la lógica de puntuación, vidas, transiciones y la selección algorítmica de los minijuegos.
/// </summary>
public class ControlJuego : MonoBehaviour
{
    /// <summary>
    /// Instancia estática única y accesible globalmente por cualquier otra clase del proyecto.
    /// </summary>
    public static ControlJuego instancia;

    [Header("Estado Global del Jugador")]
    /// <summary>Contador de vidas restantes. El juego termina cuando llega a cero.</summary>
    public int vidas = 3;

    /// <summary>Puntuación acumulada durante la sesión actual.</summary>
    public int puntuacion = 0;

    /// <summary>Temporizador del minijuego activo, utilizado para sincronizar el HUD.</summary>
    public float tiempoMinijuego = 0f;

    [Header("Control de Flujo de Escenas")]
    /// <summary>Registra si el último minijuego jugado resultó en victoria ("Ganado") o derrota ("Perdido").</summary>
    public string ultimoResultado = "";

    /// <summary>Almacena el nombre de la última escena cargada para evitar repeticiones consecutivas.</summary>
    public string ultimoMinijuego = "";

    /// <summary>Colección (Lista) con los nombres exactos de las escenas de los minijuegos disponibles.</summary>
    public List<string> minijuegos = new List<string>() { "MinijuegoSaltos", "MinijuegoMachaca" };

    /// <summary>
    /// Método de inicialización temprana.
    /// Configura el patrón Singleton garantizando que solo exista una instancia de esta clase
    /// y que no se destruya al transicionar entre diferentes escenas de Unity (DontDestroyOnLoad).
    /// </summary>
    private void Awake()
    {
        // Si no hay ninguna instancia previa, esta se convierte en la principal
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        // Si ya existe un gestor, se destruye el clon para evitar conflictos de datos
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Reinicia los valores del estado global para comenzar una nueva sesión
    /// y lanza el primer desafío aleatorio.
    /// </summary>
    public void IniciarPartida()
    {
        vidas = 3;
        puntuacion = 0;
        CargarSiguienteMinijuego();
    }

    /// <summary>
    /// Registra el éxito en una prueba, incrementa la puntuación y redirige 
    /// a la pantalla de transición para informar al jugador (Feedback de UI).
    /// </summary>
    public void ganarMinijuego()
    {
        puntuacion += 100;
        ultimoResultado = "Ganado";

        // Apuntamos a la escena única de resultados intermedios
        SceneManager.LoadScene("VictoriaMinijuego");
    }

    /// <summary>
    /// Gestiona el fallo en una prueba restando una vida. 
    /// Evalúa si es un Game Over definitivo para guardar los datos en la nube,
    /// o si el jugador aún tiene intentos restantes.
    /// </summary>
    public void perderMinijuego()
    {
        vidas--;
        ultimoResultado = "Perdido";

        if (vidas <= 0)
        {
            // Persistencia de datos en la nube (BaaS): 
            // Recupera el nombre de usuario de las preferencias locales y guarda el récord en Firebase
            string nombre = PlayerPrefs.GetString("Usuario", "Jugador");
            if (ControladorFirebase.instancia != null)
                ControladorFirebase.instancia.GuardarPuntuacion(nombre, puntuacion);

            // Carga la pantalla final de resultados
            SceneManager.LoadScene("Resultados");
        }
        else
        {
            // Si le quedan vidas, va a la pantalla de transición informativa
            SceneManager.LoadScene("VictoriaMinijuego");
        }
    }

    /// <summary>
    /// Algoritmo de selección de niveles.
    /// Escoge el siguiente minijuego de forma pseudoaleatoria aplicando una restricción de diseño:
    /// evita la repetición consecutiva del mismo nivel para mejorar la experiencia de usuario y dinamismo.
    /// </summary>
    public void CargarSiguienteMinijuego()
    {
        // Control de seguridad: Si solo hay 1 o 0 minijuegos en la lista, lo carga directamente para evitar un bucle infinito
        if (minijuegos.Count <= 1)
        {
            SceneManager.LoadScene(minijuegos[0]);
            return;
        }

        string siguiente = minijuegos[Random.Range(0, minijuegos.Count)];

        // Bucle de validación: Recalcula la selección aleatoria mientras coincida con el último jugado
        while (siguiente == ultimoMinijuego)
        {
            siguiente = minijuegos[Random.Range(0, minijuegos.Count)];
        }

        ultimoMinijuego = siguiente;

        SceneManager.LoadScene(siguiente);
    }
}