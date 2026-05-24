using UnityEngine;

/// <summary>
/// Clase encargada de gestionar la orientación visual del jugador.
/// Invierte el sprite horizontalmente (Flip) basándose en la dirección física del movimiento,
/// mejorando la respuesta visual y la coherencia de la interfaz gráfica del personaje.
/// </summary>
public class ControlFlipJugador : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente Rigidbody2D del jugador.
    /// Se utiliza para leer la velocidad lineal en el eje X y determinar la dirección del movimiento.
    /// </summary>
    public Rigidbody2D fisica;

    /// <summary>
    /// Referencia al componente SpriteRenderer que dibuja al jugador en pantalla.
    /// Permite acceder a la propiedad 'flipX' para voltear la imagen sin rotar el objeto 3D real.
    /// </summary>
    public SpriteRenderer sprite;

    /// <summary>
    /// Método del ciclo de vida de Unity que se ejecuta en cada frame.
    /// Evalúa la velocidad horizontal y ajusta la orientación del sprite hacia la derecha o la izquierda.
    /// </summary>
    void Update()
    {
        // Si la velocidad es positiva (se mueve a la derecha), el sprite mantiene su orientación original.
        if (fisica.linearVelocity.x > 0.1f)
        {
            sprite.flipX = false;
        }
        // Si la velocidad es negativa (se mueve a la izquierda), el sprite se invierte horizontalmente.
        else if (fisica.linearVelocity.x < -0.1f)
        {
            sprite.flipX = true;
        }
    }
}