using UnityEngine;
using TMPro;

public class ControladorResultado : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoEspera = 3f;

    [Header("Elementos Visuales")]
    public TMP_Text textoTitulo; // Texto superior ("¡VICTORIA!" / "¡DERROTA!")
    public TMP_Text textoPuntos; // Texto central ("SCORE: 300 PTS")
    public TMP_Text textoVidas;  // OPCIONAL: Para mostrar los corazones/vidas que le quedan

    [Header("Colores Arcade")]
    public Color colorVictoria = new Color(0.16f, 0.71f, 0.96f); // Azul Cian / Verde
    public Color colorDerrota = new Color(0.96f, 0.26f, 0.21f);  // Rojo

    void Start()
    {
        // Comprobamos de forma segura si el jugador ha ganado mirando tu variable string
        if (ControlJuego.instancia != null)
        {
            // Mostramos la puntuación total acumulada hasta el momento
            textoPuntos.text = "SCORE: " + ControlJuego.instancia.puntuacion.ToString("N0") + " PTS";

            // Mostramos de forma opcional las vidas restantes en la transición
            if (textoVidas != null)
            {
                textoVidas.text = "VIDAS: " + ControlJuego.instancia.vidas.ToString();
            }

            // Adaptamos el diseño según tu variable de texto
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

        // Cuenta atrás de 3 segundos para el siguiente minijuego
        Invoke("PasarAlSiguiente", tiempoEspera);
    }

    void PasarAlSiguiente()
    {
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.CargarSiguienteMinijuego();
        }
    }
}
