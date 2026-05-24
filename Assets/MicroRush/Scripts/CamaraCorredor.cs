using UnityEngine;

/// <summary>
/// Controlador de la cámara principal para minijuegos de desplazamiento lateral (Side-scroller / Runner).
/// Mantiene el encuadre centrado en la acción y aplica un desplazamiento (offset) espacial
/// para maximizar el área visible y mejorar el tiempo de reacción del jugador.
/// </summary>
public class CamaraCorredor : MonoBehaviour
{
    [Header("A quién seguimos")]

    /// <summary>
    /// Referencia espacial (Transform) del avatar del jugador. 
    /// Sirve como punto de anclaje dinámico para el seguimiento de la cámara.
    /// </summary>
    public Transform jugador;

    [Header("Configuración")]

    /// <summary>
    /// Margen de anticipación visual en el eje X. 
    /// Desplaza el centro de la cámara por delante del jugador para darle mayor campo de visión sobre los obstáculos.
    /// </summary>
    public float distanciaAdelante = 3f;

    /// <summary>
    /// Método del ciclo de vida de Unity que se ejecuta después de todos los métodos Update estándar.
    /// Garantiza que la cámara se actualice solo cuando el jugador ya ha resuelto sus físicas y movimientos,
    /// previniendo vibraciones visuales o tirones (stuttering) en la pantalla.
    /// </summary>
    void LateUpdate()
    {
        // Programación defensiva: Comprueba que la referencia del jugador sigue existiendo en la escena
        if (jugador != null)
        {
            // Tracking Unidimensional (Eje X):
            // 1. Sigue al jugador horizontalmente sumando el margen de anticipación (UX).
            // 2. Mantiene la altura (Y) fija original para evitar mareos o desorientación cuando el jugador salta.
            // 3. Fija el plano de profundidad (Z) en -10f, el estándar para cámaras ortográficas 2D, evitando clipeo visual.
            transform.position = new Vector3(jugador.position.x + distanciaAdelante, transform.position.y, -10f);
        }
    }
}
