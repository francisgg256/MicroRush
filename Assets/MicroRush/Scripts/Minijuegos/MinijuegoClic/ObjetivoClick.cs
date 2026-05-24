using UnityEngine;

/// <summary>
/// Clase que transforma un objeto 2D en un elemento interactivo clickable (Point & Click).
/// Gestiona la interacción directa del usuario mediante el ratón o pantalla táctil, 
/// procesando la lógica de recompensas o penalizaciones y emitiendo feedback visual.
/// </summary>
public class ObjetivoClick : MonoBehaviour
{
    [Header("Configuración")]

    /// <summary>
    /// Define el comportamiento del objeto. Si es 'true', actuará como un obstáculo letal (ej. bomba).
    /// Si es 'false', actuará como un objetivo válido que suma puntos (ej. manzana).
    /// </summary>
    public bool esPeligro = false;

    /// <summary>
    /// Tiempo de retardo en segundos antes de destruir el objeto.
    /// Permite que la animación de interacción se reproduzca completamente.
    /// </summary>
    public float tiempoDestruccion = 0.5f;

    /// <summary>Bandera de seguridad para evitar que el usuario registre múltiples clics sobre el mismo objeto.</summary>
    private bool yaPulsada = false;

    /// <summary>Referencia al componente encargado de reproducir las animaciones de feedback visual.</summary>
    private Animator animator;

    /// <summary>Referencia a la caja de colisión del objeto, utilizada para detectar el puntero del ratón.</summary>
    private Collider2D miCollider;

    /// <summary>
    /// Método de inicialización. 
    /// Cachea dinámicamente las referencias de los componentes necesarios para optimizar el rendimiento.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        miCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Evento nativo de Unity que se dispara cuando el usuario presiona el botón principal del ratón 
    /// (o toca la pantalla táctil) exactamente sobre el Collider2D de este objeto.
    /// </summary>
    void OnMouseDown()
    {
        // Control de Errores: Previene el "Spam" de clics evitando que un jugador rápido puntúe doble
        if (yaPulsada) return;

        yaPulsada = true;

        // Desactivación interactiva: Se anula el collider para que el objeto deje de interceptar el ratón
        // mientras termina su animación de destrucción.
        if (miCollider != null) miCollider.enabled = false;

        // Bifurcación de lógica según la configuración del objeto
        if (esPeligro)
        {
            // Feedback negativo: Interacción con un objeto letal
            if (MinijuegoDiana.instancia != null)
            {
                MinijuegoDiana.instancia.perder();
            }
        }
        else
        {
            // Feedback positivo (Visual): Reproduce la animación de recolección
            if (animator != null)
            {
                animator.SetTrigger("recogerManzana");
            }

            // Paso de mensajes: Notifica al gestor del minijuego que se ha logrado un acierto
            if (MinijuegoDiana.instancia != null)
            {
                MinijuegoDiana.instancia.SumarAcierto();
            }

            // Optimización de memoria: Limpia el objeto de la escena tras dar tiempo al feedback visual
            Destroy(gameObject, tiempoDestruccion);
        }
    }
}
