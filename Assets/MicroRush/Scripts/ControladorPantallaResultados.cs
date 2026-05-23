using UnityEngine;
using TMPro; // Necesario para los textos
using UnityEngine.SceneManagement;

public class ControladorResultados : MonoBehaviour
{
    [Header("Estadísticas de la Partida")]
    public TMP_Text textoNombre;
    public TMP_Text textoPuntosFinales;
    public TMP_Text textoPruebasSuperadas;
    public TMP_Text textoMensaje; // Opcional: Para poner "¡Sigue practicando!" o "¡Increíble!"

    void Start()
    {
        // 1. Cargamos el nombre que guardaste en el Login
        string nombreJugador = PlayerPrefs.GetString("Usuario", "Jugador Desconocido");
        if (textoNombre != null)
        {
            textoNombre.text = "JUGADOR: " + nombreJugador;
        }

        // 2. Sacamos los datos de la partida desde el ControlJuego
        if (ControlJuego.instancia != null)
        {
            int puntosTotales = ControlJuego.instancia.puntuacion;

            if (textoPuntosFinales != null)
            {
                textoPuntosFinales.text = "SCORE: " + puntosTotales.ToString("N0") + " PTS";
            }

            if (textoPruebasSuperadas != null)
            {
                // Si cada minijuego superado suma 100 puntos, calculamos cuántos pasó
                int minijuegos = puntosTotales / 100;
                textoPruebasSuperadas.text = "MINIJUEGOS COMPLETADOS: " + minijuegos;
            }

            // Un detallito extra: Mensaje dinámico según lo bien que lo haya hecho
            if (textoMensaje != null)
            {
                if (puntosTotales >= 500) textoMensaje.text = "¡RÉCORD BRUTAL!";
                else if (puntosTotales >= 200) textoMensaje.text = "¡BUEN INTENTO!";
                else textoMensaje.text = "¡HAY QUE ENTRENAR MÁS!";
            }
        }
    }

    public void OnBotonMenu()
    {
        // Asegúrate de poner el nombre exacto de tu escena de menú
        SceneManager.LoadScene("Menu");
    }

    public void OnBotonRanking()
    {
        SceneManager.LoadScene("Ranking"); // (Asumiendo que así se llama tu escena)
    }

    // IDEA EXTRA: Un botón para reiniciar rápido sin pasar por el menú
    public void OnBotonReintentar()
    {
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.IniciarPartida();
        }
    }
}