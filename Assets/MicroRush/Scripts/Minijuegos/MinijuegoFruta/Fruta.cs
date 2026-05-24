using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de los objetos coleccionables (frutas) en la escena.
/// Gestiona la detección de colisiones con el jugador, la comunicación con el gestor del minijuego 
/// y proporciona feedback visual (animaciones) antes de liberar la memoria.
/// </summary>
public class Fruta : MonoBehaviour
{
    /// <summary>
    /// Bandera de control de estado. 
    /// Previene la ejecución múltiple del evento de recolección (evitando bugs de puntuación doble).
    /// </summary>
    private bool yaRecogida = false;

    /// <summary>Referencia al componente Animator para ejecutar el feedback visual.</summary>
    private Animator animator;

    /// <summary>Referencia al componente que renderiza la imagen en pantalla.</summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>Referencia a la caja de colisión (Trigger) del objeto.</summary>
    private Collider2D frutaCollider;

    /// <summary>
    /// Tiempo de espera en segundos antes de eliminar el objeto de la memoria.
    /// Permite que la animación de recolección se reproduzca por completo.
    /// </summary>
    public float tiempoDestruccion = 0.5f;

    /// <summary>
    /// Método de inicialización. 
    /// Obtiene y cachea las referencias a los componentes del propio objeto para optimizar el rendimiento.
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        frutaCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Evento físico que se dispara cuando un objeto entra en el área interactiva (Trigger) de la fruta.
    /// </summary>
    /// <param name="collision">Datos del colisionador que ha entrado en contacto.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Validación múltiple: Comprueba que sea el jugador, que no se haya recogido ya, 
        // y que el componente Animator exista para evitar NullReferenceException.
        if (collision.CompareTag("Jugador") && !yaRecogida && animator != null)
        {
            // Bloqueo de estado: Evita que en el siguiente frame se vuelva a evaluar como verdadero
            yaRecogida = true;

            // Paso de mensajes: Comunica al gestor del minijuego que este objeto específico ha sido recolectado
            FindAnyObjectByType<MinijuegoFrutas>().FrutaRecogida(gameObject);

            // Desactiva la colisión física inmediatamente para evitar interacciones "fantasma" durante la animación
            if (frutaCollider != null) frutaCollider.enabled = false;

            // Feedback Visual (UI/UX): Lanza la animación de recolección para informar al jugador del éxito de la acción
            animator.SetTrigger("recogerFruta");

            // Optimización (Garbage Collection): Programa la destrucción del objeto tras el tiempo estipulado
            Destroy(gameObject, tiempoDestruccion);
        }
    }
}
