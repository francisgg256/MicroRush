using UnityEngine;

/// <summary>
/// Controlador principal del avatar en el minijuego de *Runner* con inversión gravitacional.
/// Gestiona el desplazamiento automático, las físicas aplicadas, la respuesta a los eventos 
/// de colisión y la sobreescritura del renderizado visual.
/// </summary>
public class JugadorGravedad : MonoBehaviour
{
    [Header("Control de Inicio")]
    /// <summary>Candado lógico. Evita que el jugador corra y caiga mientras se lee el cartel.</summary>
    public bool juegoIniciado = false;

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
    /// Inicia al personaje completamente congelado en el aire sin gravedad.
    /// </summary>
    void Start()
    {
        if (rb != null)
        {
            // Congelamos al jugador inicialmente para que no caiga durante el cartel
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        // Dejamos la animación en estado idle o parado si tienes una, 
        // o directamente corriendo si no te importa que mueva las piernas en el sitio.
        if (anim != null) anim.Play("jugadorCorriendo");
    }

    /// <summary>Método llamado por el cartel universal de UI para desbloquear el minijuego.</summary>
    public void IniciarMinijuego()
    {
        juegoIniciado = true;
        if (rb != null)
        {
            // Restauramos la gravedad original en cuanto desaparece el cartel
            rb.gravityScale = fuerzaGravedad;
        }
    }

    /// <summary>
    /// Bucle lógico principal.
    /// Procesa el desplazamiento automático y captura la entrada del usuario para invertir la polaridad física.
    /// </summary>
    void Update()
    {
        // Control de estado: Aborta la ejecución si el nivel no ha empezado, finalizó, o faltan dependencias
        if (!juegoIniciado || juegoTerminado || rb == null) return;

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
    /// </summary>
    void LateUpdate()
    {
        // Solo volteamos el sprite si el juego ya está corriendo
        if (sprite != null && rb != null && juegoIniciado && !juegoTerminado)
        {
            // Sincronización Visual: Voltea el sprite verticalmente si la gravedad empuja hacia el techo
            sprite.flipY = (rb.gravityScale < 0);
        }
    }

    /// <summary>
    /// Sistema de detección de colisiones (Triggers).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D otro)
    {
        Debug.Log("Impacto registrado con entidad: " + otro.name + " | Etiqueta (Tag): " + otro.tag);

        // Si el juego no ha iniciado (aunque no debería chocar) o terminó, salimos
        if (!juegoIniciado || juegoTerminado) return;

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

    void Ganar()
    {
        juegoTerminado = true;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.Play("jugadorParado");

        Debug.Log("Condición de victoria: Meta alcanzada.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
    }

    void Perder()
    {
        juegoTerminado = true;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.Play("jugadorParado");

        Debug.Log("Condición de derrota: Colisión letal.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
