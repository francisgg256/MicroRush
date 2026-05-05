using UnityEngine;

public class ObjetivoClick : MonoBehaviour
{
    [Header("Configuración")]
    public bool esPeligro = false; // Marcar esto SOLO en la bola de pinchos
    public float tiempoDestruccion = 0.5f;

    private bool yaPulsada = false;
    private Animator animator;
    private Collider2D miCollider;

    void Start()
    {
        // Cogemos los componentes automáticamente al empezar
        animator = GetComponent<Animator>();
        miCollider = GetComponent<Collider2D>();
    }

    // Usamos OnMouseDown en vez de OnTriggerEnter2D para detectar el clic
    void OnMouseDown()
    {
        // Si ya le hemos hecho clic, ignoramos para que no pase dos veces
        if (yaPulsada) return;

        yaPulsada = true;

        // Desactivamos el collider para que no se le pueda hacer clic otra vez mientras hace la animación
        if (miCollider != null) miCollider.enabled = false;

        // ... (código anterior del OnMouseDown)

        if (esPeligro)
        {
            // Si es la bola de pinchos, perdemos instantáneamente
            if (MinijuegoDiana.instancia != null)
            {
                MinijuegoDiana.instancia.perder();
            }
        }
        else
        {
            // Si en tu Animator el parámetro se llama recogerManzana, ponlo así:
            if (animator != null)
            {
                animator.SetTrigger("recogerManzana");
            }

            // NUEVA LÍNEA: Le decimos al controlador que hemos sumado un punto
            if (MinijuegoDiana.instancia != null)
            {
                MinijuegoDiana.instancia.SumarAcierto();
            }

            // Y la destruimos
            Destroy(gameObject, tiempoDestruccion);
        }
    }
}
