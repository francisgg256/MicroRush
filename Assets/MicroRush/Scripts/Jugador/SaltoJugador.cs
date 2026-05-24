using UnityEngine;

/// <summary>
/// Clase encargada de gestionar la mecánica de salto del jugador.
/// Implementa un sistema seguro que separa la lectura de la entrada del usuario 
/// de la ejecución física, y verifica el contacto con el suelo para evitar saltos infinitos.
/// </summary>
public class SaltoJugador : MonoBehaviour
{
    /// <summary>
    /// Fuerza vertical que se aplicará al jugador al saltar.
    /// Determina la altura máxima que puede alcanzar el personaje.
    /// </summary>
    public int fuerzaSalto;

    /// <summary>
    /// Referencia al componente Rigidbody2D del jugador para aplicar fuerzas físicas.
    /// </summary>
    public Rigidbody2D fisica;

    /// <summary>
    /// Punto de origen (transform) ubicado en los pies del personaje.
    /// Se utiliza como punto de partida para el lanzamiento del rayo detector de suelo.
    /// </summary>
    public Transform puntoSuelo;

    /// <summary>
    /// Variable de control (flag) que indica si el jugador ha solicitado saltar en el frame actual.
    /// Permite trasladar la orden desde el Update al FixedUpdate.
    /// </summary>
    private bool entradaSalto;

    /// <summary>
    /// Método de ciclo de vida para físicas. 
    /// Se encarga de aplicar la fuerza de salto de forma estable e independiente de los FPS.
    /// </summary>
    private void FixedUpdate()
    {
        // Si el jugador pulsó la tecla de salto, aplicamos la fuerza física
        if (entradaSalto)
        {
            // Se utiliza ForceMode2D.Impulse para aplicar toda la fuerza de golpe, ideal para saltos arcade
            fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

            // Reseteamos la variable para no seguir saltando en los siguientes frames
            entradaSalto = false;
        }
    }

    /// <summary>
    /// Método de ciclo de vida que se ejecuta cada frame.
    /// Captura la entrada del usuario de manera inmediata para garantizar la mejor usabilidad y respuesta de control.
    /// </summary>
    void Update()
    {
        // Comprueba si se pulsa la barra espaciadora Y si el personaje está tocando una superficie válida
        if (Input.GetKeyDown(KeyCode.Space) && tocarSuelo())
        {
            entradaSalto = true;
        }
    }

    /// <summary>
    /// Lanza un rayo (Raycast) hacia abajo para determinar si el jugador está apoyado sobre una plataforma.
    /// Esto previene el "doble salto" o salto en el aire no deseado.
    /// </summary>
    /// <returns>Devuelve <c>true</c> si el rayo impacta con un colisionador que no sea el jugador; de lo contrario, <c>false</c>.</returns>
    private bool tocarSuelo()
    {
        // Lanza un rayo desde los pies hacia abajo con una longitud de 0.2 unidades
        RaycastHit2D toca = Physics2D.Raycast(puntoSuelo.position, Vector2.down, 0.2f);

        // Línea roja de depuración visible en el editor de Unity
        Debug.DrawRay(puntoSuelo.position, Vector2.down * 0.2f, Color.red);

        // Verifica que hayamos chocado con algo y que no sea la propia caja de colisión del jugador
        if (toca.collider != null && !toca.collider.CompareTag("Jugador"))
        {
            return true;
        }

        return false;
    }
}