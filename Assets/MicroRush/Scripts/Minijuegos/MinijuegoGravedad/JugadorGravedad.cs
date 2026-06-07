using UnityEngine;

/// <summary>
/// Controlador principal del avatar en el minijuego de Runner de Supervivencia.
/// Gestiona la inversión gravitacional, el Autoscroll y el aumento de dificultad del Nivel 2.
/// </summary>
public class JugadorGravedad : MonoBehaviour
{
    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración de Supervivencia")]
    public float tiempoParaGanar = 7f;

    [Header("Configuración de Movimiento (Nivel 1)")]
    public float velocidadBase = 6f;
    private float velocidadActual;
    public float fuerzaGravedad = 3f;

    [Header("Modo Extremo (Nivel 2)")]
    /// <summary>Activa esta casilla para que la velocidad aumente progresivamente a medida que pasa el tiempo.</summary>
    public bool aceleracionProgresiva = false;

    /// <summary>El límite de velocidad para que el juego no se vuelva literalmente imposible.</summary>
    public float velocidadMaxima = 12f;

    /// <summary>Cuánto aumenta la velocidad por cada segundo que pasa el jugador vivo.</summary>
    public float ritmoAceleracion = 1f;

    [Header("Componentes")]
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator anim;

    [Header("Audio")]
    public AudioSource audioSource;

    private bool juegoTerminado = false;

    void Start()
    {
        // Al empezar, arrancamos con la velocidad normal
        velocidadActual = velocidadBase;

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        if (anim != null) anim.Play("jugadorCorriendo");
    }

    public void IniciarMinijuego()
    {
        juegoIniciado = true;
        if (rb != null)
        {
            rb.gravityScale = fuerzaGravedad;
        }
    }

    void Update()
    {
        if (!juegoIniciado || juegoTerminado || rb == null) return;

        // 1. Cronómetro de Supervivencia
        tiempoParaGanar -= Time.deltaTime;

        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoParaGanar;

        if (tiempoParaGanar <= 0)
        {
            Ganar();
            return;
        }

        // --- NUEVO: Acelerador del Nivel 2 ---
        // Si el modo extremo está activado y aún no hemos llegado al límite, pisamos el acelerador
        if (aceleracionProgresiva && velocidadActual < velocidadMaxima)
        {
            velocidadActual += ritmoAceleracion * Time.deltaTime;
        }

        // 2. Movimiento Infinito (Autoscroll) usando la velocidad calculada
        rb.linearVelocity = new Vector2(velocidadActual, rb.linearVelocity.y);

        // 3. Mecánica de Inversión Gravitacional
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.gravityScale *= -1;

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }

    void LateUpdate()
    {
        if (sprite != null && rb != null && juegoIniciado && !juegoTerminado)
        {
            sprite.flipY = (rb.gravityScale < 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!juegoIniciado || juegoTerminado) return;

        if (otro.CompareTag("Trampa"))
        {
            Perder();
        }
    }

    void Ganar()
    {
        juegoTerminado = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        if (anim != null) anim.Play("jugadorParado");

        Debug.Log("¡Supervivencia completada! Victoria.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
    }

    void Perder()
    {
        juegoTerminado = true;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.Play("jugadorParado");

        Debug.Log("Colisión letal. Derrota inmediata.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
