using UnityEngine;

public class JugadorGravedad : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 6f;
    public float fuerzaGravedad = 3f;

    [Header("Componentes")]
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator anim;

    private bool juegoTerminado = false;

    void Start()
    {
        if (rb != null)
        {
            rb.gravityScale = fuerzaGravedad;
        }

        if (anim != null) anim.Play("jugadorCorriendo");
    }

    void Update()
    {
        if (juegoTerminado || rb == null) return;

        // 1. CORRER HACIA LA DERECHA AUTOMÁTICAMENTE
        rb.linearVelocity = new Vector2(velocidad, rb.linearVelocity.y);

        // 2. INVERTIR LA GRAVEDAD (Solo físicas)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.gravityScale *= -1;
        }
    }

    // 3. ¡EL TRUCO MÁGICO!
    // LateUpdate se ejecuta DESPUÉS del Animator.
    // Obligamos al dibujo a darse la vuelta, ignorando lo que diga la animación.
    void LateUpdate()
    {
        if (sprite != null && rb != null && !juegoTerminado)
        {
            // Si la gravedad es negativa (hacia arriba), le damos la vuelta al sprite
            sprite.flipY = (rb.gravityScale < 0);
        }
    }

    // 4. DETECTAR CHOQUES
    private void OnTriggerEnter2D(Collider2D otro)
    {
        // EL CHIVATO: Nos dirá en la consola con qué estamos chocando
        Debug.Log("Acabo de chocar con: " + otro.name + " | Y su etiqueta es: " + otro.tag);

        if (juegoTerminado) return;

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

        Debug.Log("¡Llegaste a la meta!");
        if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
    }

    void Perder()
    {
        juegoTerminado = true;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.Play("jugadorParado");

        Debug.Log("¡Te chocaste con una sierra!");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
