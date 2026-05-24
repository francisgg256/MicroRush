using UnityEngine;

/// <summary>
/// Clase encargada de gestionar la máquina de estados de las animaciones del jugador.
/// Evalúa constantemente las físicas y la posición para reproducir la animación correcta (correr, saltar, caer o estar parado).
/// </summary>
public class AnimacionJugador : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente Animator que contiene los clips y estados de animación.
    /// </summary>
    public Animator animacion;

    /// <summary>
    /// Referencia al cuerpo físico del jugador para leer su velocidad lineal en los ejes X e Y.
    /// </summary>
    public Rigidbody2D fisica;

    /// <summary>
    /// Punto de origen situado en la base del personaje desde donde se lanzará el Raycast para detectar el suelo.
    /// </summary>
    public Transform puntoSuelo;

    /// <summary>
    /// Método del ciclo de vida de Unity que se ejecuta en cada frame.
    /// Mantiene la animación sincronizada con las acciones físicas del frame actual.
    /// </summary>
    void Update()
    {
        animarJugador();
    }

    /// <summary>
    /// Evalúa la velocidad y el contacto con el entorno para cambiar dinámicamente el estado de la animación.
    /// Prioriza las animaciones aéreas sobre las terrestres.
    /// </summary>
    private void animarJugador()
    {
        // Comprueba si el jugador está en el aire
        if (!tocarSuelo())
        {
            // Si la velocidad vertical es negativa, está cayendo. Si es positiva, está saltando.
            if (fisica.linearVelocity.y < -0.1f) animacion.Play("jugadorCayendo");
            else animacion.Play("jugadorSaltando");
        }
        else
        {
            // Si está en el suelo y tiene velocidad horizontal, está corriendo
            if (Mathf.Abs(fisica.linearVelocity.x) > 0.1f)
            {
                animacion.Play("jugadorCorriendo");
            }
            else
            {
                animacion.Play("jugadorParado");
            }
        }
    }

    /// <summary>
    /// Lanza un rayo invisible (Raycast) hacia abajo desde el punto base del jugador para detectar colisiones con la geometría del nivel.
    /// </summary>
    /// <returns>Devuelve <c>true</c> si el rayo impacta contra una superficie sólida que no sea el propio jugador; de lo contrario, <c>false</c>.</returns>
    private bool tocarSuelo()
    {
        RaycastHit2D toca = Physics2D.Raycast(puntoSuelo.position, Vector2.down, 0.2f);

        // Línea de depuración visible en la ventana Scene para ajustar la longitud del rayo
        Debug.DrawRay(puntoSuelo.position, Vector2.down * 0.2f, Color.red);

        // Si colisiona con algo y ese algo no tiene la etiqueta "Jugador", estamos tocando el suelo
        if (toca.collider != null && !toca.collider.CompareTag("Jugador"))
        {
            return true;
        }

        return false;
    }
}