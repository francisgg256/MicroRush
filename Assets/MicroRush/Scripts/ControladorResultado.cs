using UnityEngine;
using TMPro;

/// <summary>
/// Controlador de la interfaz de la pantalla de transición (Intermedio).
/// Proporciona feedback visual y psicológico inmediato al jugador sobre el resultado de su última prueba,
/// gestionando la navegación automática hacia el siguiente nivel para mantener el ritmo arcade.
/// </summary>
public class ControladorResultado : MonoBehaviour
{
    [Header("Configuración")]
    /// <summary>Tiempo de retardo en segundos antes de auto-transicionar a la siguiente escena.</summary>
    public float tiempoEspera = 3f;

    [Header("Elementos Visuales")]
    /// <summary>Texto dinámico (TextMeshPro) para el encabezado principal ("¡VICTORIA!" o "¡DERROTA!").</summary>
    public TMP_Text textoTitulo;

    /// <summary>Texto dinámico para mostrar la puntuación total acumulada actualizada.</summary>
    public TMP_Text textoPuntos;

    /// <summary>Texto dinámico opcional para mostrar las vidas restantes (refuerza la visibilidad del estado).</summary>
    public TMP_Text textoVidas;

    [Header("Colores Arcade (Psicología del Color)")]
    /// <summary>Color asociado al éxito (feedback positivo). Por defecto, un tono azul cian/verde.</summary>
    public Color colorVictoria = new Color(0.16f, 0.71f, 0.96f);

    /// <summary>Color asociado al fracaso o alerta (feedback negativo). Por defecto, un tono rojo intenso.</summary>
    public Color colorDerrota = new Color(0.96f, 0.26f, 0.21f);

    /// <summary>
    /// Método de inicialización. 
    /// Evalúa el estado global persistente y adapta el diseño visual de toda la interfaz
    /// (textos y colores) en base al éxito o fracaso en el último minijuego.
    /// </summary>
    void Start()
    {
        // Programación defensiva: Comprobación de la existencia del Game Manager (Singleton)
        if (ControlJuego.instancia != null)
        {
            // Actualización del estado (Puntos) con formato numérico para facilitar la lectura
            textoPuntos.text = "SCORE: " + ControlJuego.instancia.puntuacion.ToString("N0") + " PTS";

            // Actualización del estado (Vidas) comprobando primero que el componente exista en la UI
            if (textoVidas != null)
            {
                textoVidas.text = "VIDAS: " + ControlJuego.instancia.vidas.ToString();
            }

            // Lógica de Adaptación de Interfaz: Cambia los mensajes y la paleta de colores dinámicamente
            if (ControlJuego.instancia.ultimoResultado == "Ganado")
            {
                textoTitulo.text = "¡VICTORIA!";
                textoTitulo.color = colorVictoria;
            }
            else if (ControlJuego.instancia.ultimoResultado == "Perdido")
            {
                textoTitulo.text = "¡DERROTA!";
                textoTitulo.color = colorDerrota;
            }
        }

        // Automatización del flujo de usuario: Programa el salto a la siguiente escena sin requerir input manual
        Invoke("PasarAlSiguiente", tiempoEspera);
    }

    /// <summary>
    /// Invoca la lógica de selección pseudoaleatoria del Singleton global 
    /// para cargar el siguiente minijuego de la cola.
    /// </summary>
    void PasarAlSiguiente()
    {
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.CargarSiguienteMinijuego();
        }
    }
}
