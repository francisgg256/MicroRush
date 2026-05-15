using UnityEngine;

public class JugadorSemaforo : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 5f;
    public Transform meta;

    [Header("Componentes")]
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator anim;

    void Update()
    {
        if (rb == null) return;

        float movH = Input.GetAxisRaw("Horizontal");
        float movV = Input.GetAxisRaw("Vertical");
        bool moviendoEspacio = Input.GetKey(KeyCode.Space);

        bool intentandoMoverse = Mathf.Abs(movH) > 0.1f || Mathf.Abs(movV) > 0.1f || moviendoEspacio;

        Vector2 velocidadFinal = Vector2.zero;

        if (intentandoMoverse)
        {
            // REGLA DE ORO: Si el semáforo está en estado 2 (ROJO)... pierdes
            if (MinijuegoSemaforo.instancia.estadoSemaforo == 2)
            {
                Debug.Log("¡TE MOVIZTE EN ROJO!");
                rb.linearVelocity = Vector2.zero;

                if (anim != null) anim.Play("jugadorParado");

                MinijuegoSemaforo.instancia.Perder();
                return;
            }

            // Si está en Verde (0) o Amarillo (1), te deja moverte
            if (moviendoEspacio)
            {
                velocidadFinal = new Vector2(velocidad, 0);
            }
            else
            {
                velocidadFinal = new Vector2(movH, movV).normalized * velocidad;
            }
        }

        rb.linearVelocity = velocidadFinal;

        ActualizarVisuales(movH, velocidadFinal.magnitude);

        if (meta != null && rb.position.x >= meta.position.x)
        {
            MinijuegoSemaforo.instancia.Ganar();
        }
    }

    void ActualizarVisuales(float movH, float rapidez)
    {
        if (movH > 0.1f) sprite.flipX = false;
        else if (movH < -0.1f) sprite.flipX = true;

        if (anim != null)
        {
            if (rapidez > 0.1f)
            {
                anim.Play("jugadorCorriendo");
            }
            else
            {
                anim.Play("jugadorParado");
            }
        }
    }
}
