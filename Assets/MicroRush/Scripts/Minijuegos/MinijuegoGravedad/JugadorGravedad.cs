using UnityEngine;

/// <summary>
/// Controlador principal del avatar en el minijuego de *Runner* con inversión gravitacional.
/// Gestiona el desplazamiento automático, las físicas aplicadas, la respuesta a los eventos 
/// de colisión y la sobreescritura del renderizado visual.
/// </summary>
public class JugadorGravedad : MonoBehaviour
{
    [Header("Configuración de Movimiento")]

    /// <summary>Velocidad constante de avance en el eje horizontal (Autoscroll).</summary>
    public float velocidad = 6f;

    /// <summary>Magnitud base de la fuerza gravitatoria. Se invertirá para generar la mecánica de juego.</summary>
    public float fuerzaGravedad = 3f;

    [Header("Componentes")]

    /// <summary>Referencia al motor de físicas 2D para la manipulación de velocidades y gravedad.</summary>
    public Rigidbody2D rb;

    /// <summary>Referencia al renderizador para aplicar transformaciones visuales dinámicas (Flip).</summary>
    public SpriteRenderer sprite;

    /// <summary>Referencia al gestor de animaciones del jugador.</summary>
    public Animator anim;

    /// <summary>Bandera de estado que bloquea los controles y físicas una vez concluido el minijuego.</summary>
    private bool juegoTerminado = false;

    /// <summary>
    /// Método de inicialización.
    /// Configura la escala de gravedad inicial y arranca la animación de carrera por defecto.
    /// </summary>
    void Start()
    {
        if (rb != null)
        {
            rb.gravityScale = fuerzaGravedad;
        }

        if (anim != null) anim.Play("jugadorCorriendo");
    }

    /// <summary>
    /// Bucle lógico principal.
    /// Procesa el desplazamiento automático y captura la entrada del usuario para invertir la polaridad física.
    /// </summary>
    void Update()
    {
        // Control de estado: Aborta la ejecución si el nivel finalizó o faltan dependencias críticas
        if (juegoTerminado || rb == null) return;

        // 1. Desplazamiento Constante (Auto-Runner):
        // Fija la velocidad en X, pero respeta la inercia actual en Y para no romper la caída libre.
        rb.linearVelocity = new Vector2(velocidad, rb.linearVelocity.y);

        // 2. Mecánica de Inversión Gravitacional (Accesibilidad: One-Button Play):
        // Detecta tanto la barra espaciadora como el clic izquierdo/toque en pantalla.
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // Invierte la polaridad matemática de la gravedad
            rb.gravityScale *= -1;
        }
    }

    /// <summary>
    /// Método del ciclo de vida ejecutado tras el procesamiento de animaciones.
    /// Se utiliza para sobreescribir las transformaciones visuales que el componente Animator 
    /// pudiera haber forzado durante su propio ciclo de actualización.
    /// </summary>
    void LateUpdate()
    {
        if (sprite != null && rb != null && !juegoTerminado)
        {
            // Sincronización Visual: Voltea el sprite verticalmente si la gravedad empuja hacia el techo (valor negativo)
            sprite.flipY = (rb.gravityScale < 0);
        }
    }

    /// <summary>
    /// Sistema de detección de colisiones (Triggers).
    /// Evalúa las interacciones espaciales con los obstáculos y la meta.
    /// </summary>
    /// <param name="otro">Datos del colisionador interceptado.</param>
    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Trazabilidad de desarrollo: Registra los impactos en consola para facilitar la depuración (Debugging)
        Debug.Log("Impacto registrado con entidad: " + otro.name + " | Etiqueta (Tag): " + otro.tag);

        if (juegoTerminado) return;

        // Evaluación de Condiciones de Nivel
        if (otro.CompareTag("Trampa"))
        {
            Perder();
        }
        else if (otro.CompareTag("Meta") || otro.name == "Meta")
        {
            Ganar();
        }
    }

    /// <summary>
    /// Ejecuta la secuencia de victoria, frenando las físicas y notificando al gestor global.
    /// </summary>
    void Ganar()
    {
        juegoTerminado = true;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.Play("jugadorParado");

        Debug.Log("Condición de victoria: Meta alcanzada.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
    }

    /// <summary>
    /// Ejecuta la secuencia de derrota, deteniendo al avatar y notificando el fallo al sistema.
    /// </summary>
    void Perder()
    {
        juegoTerminado = true;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.Play("jugadorParado");

        Debug.Log("Condición de derrota: Colisión letal.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
