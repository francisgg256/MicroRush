using UnityEngine;

/// <summary>
/// Controlador principal del avatar en el minijuego de Runner de Supervivencia.
/// Gestiona la inversión gravitacional, el Autoscroll y el cronómetro de supervivencia.
/// </summary>
public class JugadorGravedad : MonoBehaviour
{
    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración de Supervivencia")]
    public float tiempoParaGanar = 20f;

    [Header("Configuración de Movimiento")]
    public float velocidad = 6f;
    public float fuerzaGravedad = 3f;

    [Header("Componentes")]
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator anim;

    [Header("Audio")]
    public AudioSource audioSource; // Referencia al componente de sonido

    private bool juegoTerminado = false;

    void Start()
    {
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

        // 2. Movimiento Infinito (Autoscroll)
        rb.linearVelocity = new Vector2(velocidad, rb.linearVelocity.y);

        // 3. Mecánica de Inversión Gravitacional
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.gravityScale *= -1;

            // --- NUEVO: Reproducir sonido ---
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
