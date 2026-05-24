using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador de la interfaz de la pantalla final de resultados (Game Over).
/// Se encarga de recopilar la información de la sesión finalizada, formatearla visualmente 
/// y proporcionar feedback dinámico al usuario, así como atajos rápidos de navegación.
/// </summary>
public class ControladorResultados : MonoBehaviour
{
    [Header("Estadísticas de la Partida")]

    /// <summary>Componente de texto para mostrar el nombre del piloto/jugador.</summary>
    public TMP_Text textoNombre;

    /// <summary>Componente de texto para mostrar la puntuación total acumulada.</summary>
    public TMP_Text textoPuntosFinales;

    /// <summary>Componente de texto que calcula y muestra la cantidad de pruebas superadas.</summary>
    public TMP_Text textoPruebasSuperadas;

    /// <summary>Componente de texto para el feedback psicológico dinámico ("¡Increíble!", "¡Sigue practicando!").</summary>
    public TMP_Text textoMensaje;

    /// <summary>
    /// Método de inicialización. 
    /// Aplica la heurística de usabilidad "Reconocer en lugar de recordar" recopilando los datos
    /// persistentes del juego (PlayerPrefs y Singleton) y mostrándoselos al jugador de forma clara.
    /// </summary>
    void Start()
    {
        // 1. Recuperación de la identidad del usuario desde el almacenamiento local
        string nombreJugador = PlayerPrefs.GetString("Usuario", "Jugador Desconocido");
        if (textoNombre != null)
        {
            textoNombre.text = "JUGADOR: " + nombreJugador;
        }

        // 2. Extracción de estadísticas desde el gestor global (Game Manager)
        if (ControlJuego.instancia != null)
        {
            int puntosTotales = ControlJuego.instancia.puntuacion;

            // Formateo visual ("N0") para añadir separadores de millares a la puntuación
            if (textoPuntosFinales != null)
            {
                textoPuntosFinales.text = "SCORE: " + puntosTotales.ToString("N0") + " PTS";
            }

            // Transformación de datos: Deduce los niveles superados basándose en la regla de puntuación (100 pts por nivel)
            if (textoPruebasSuperadas != null)
            {
                int minijuegos = puntosTotales / 100;
                textoPruebasSuperadas.text = "MINIJUEGOS COMPLETADOS: " + minijuegos;
            }

            // Diseño de Experiencia de Usuario (UX): Feedback dinámico basado en el rendimiento
            // Esto fomenta la retención del jugador recompensando las buenas partidas y animando en las malas.
            if (textoMensaje != null)
            {
                if (puntosTotales >= 500) textoMensaje.text = "¡RÉCORD BRUTAL!";
                else if (puntosTotales >= 200) textoMensaje.text = "¡BUEN INTENTO!";
                else textoMensaje.text = "¡HAY QUE ENTRENAR MÁS!";
            }
        }
    }

    /// <summary>
    /// Evento de navegación hacia el menú principal.
    /// </summary>
    public void OnBotonMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Evento de navegación directa hacia el panel de clasificaciones globales (Firebase).
    /// </summary>
    public void OnBotonRanking()
    {
        SceneManager.LoadScene("Ranking");
    }

    /// <summary>
    /// Evento de atajo (Shortcut) para reiniciar el ciclo de juego inmediatamente.
    /// Cumple con el principio de usabilidad de "Flexibilidad y eficiencia de uso", 
    /// reduciendo la fricción para los jugadores más activos.
    /// </summary>
    public void OnBotonReintentar()
    {
        // Control de excepciones para evitar crasheos si el gestor principal fue destruido accidentalmente
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.IniciarPartida();
        }
    }
}