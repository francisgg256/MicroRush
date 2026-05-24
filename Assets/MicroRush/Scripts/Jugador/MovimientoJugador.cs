using UnityEngine;

/// <summary>
/// Clase responsable de gestionar el movimiento horizontal del jugador.
/// Utiliza el sistema de físicas de Unity (Rigidbody2D) para asegurar un movimiento fluido 
/// y colisiones precisas, separando la lectura de *inputs* de la ejecución física.
/// </summary>
public class MovimientoJugador : MonoBehaviour
{
    /// <summary>
    /// Velocidad de desplazamiento horizontal del jugador.
    /// Define la magnitud de la fuerza aplicada al eje X.
    /// </summary>
    public int velocidad;

    /// <summary>
    /// Referencia al componente Rigidbody2D del jugador.
    /// Permite modificar la velocidad lineal para mover al personaje respetando la gravedad y las colisiones.
    /// </summary>
    public Rigidbody2D fisica;

    /// <summary>
    /// Variable interna que almacena el valor de la entrada del usuario en el eje horizontal.
    /// Toma valores entre -1 (izquierda) y 1 (derecha).
    /// </summary>
    private float entradaX;

    /// <summary>
    /// Método del ciclo de vida de Unity que se ejecuta en cada frame.
    /// Se utiliza exclusivamente para capturar la entrada del usuario ("Input") 
    /// y garantizar una respuesta inmediata a los controles.
    /// </summary>
    void Update()
    {
        // Captura el valor del eje horizontal configurado en el Input Manager de Unity (A/D o Flechas)
        entradaX = Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// Método del ciclo de vida de Unity que se ejecuta en intervalos de tiempo fijos.
    /// Se utiliza para aplicar los cálculos de físicas, garantizando un comportamiento 
    /// estable independientemente de los fotogramas por segundo (FPS) del juego.
    /// </summary>
    private void FixedUpdate()
    {
        // Aplica la nueva velocidad horizontal calculada (entrada * velocidad) 
        // y mantiene la velocidad vertical actual para no interferir con saltos o caídas.
        fisica.linearVelocity = new Vector2(entradaX * velocidad, fisica.linearVelocity.y);
    }
}