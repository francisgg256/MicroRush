using UnityEngine;

/// <summary>
/// Controla la mecánica de arrastrar la lengua con el ratón manteniendo un tamaño mínimo.
/// Modifica dinámicamente el tamaño visual y procesa colisiones físicas por Trigger.
/// </summary>
public class ControlLengua : MonoBehaviour
{
    [Header("Componentes")]
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D colisionador;

    private float anchoOriginal;
    private float alturaOriginal;
    private bool disparando = false;

    void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (colisionador == null) colisionador = GetComponent<BoxCollider2D>();

        // GUARDAMOS EL TAMANO REAL QUE TIENE EN LA ESCENA
        anchoOriginal = spriteRenderer.size.x;
        alturaOriginal = spriteRenderer.size.y;

        RetraerLengua();
    }

    void Update()
    {
        if (MinijuegoRana.instancia == null || !MinijuegoRana.instancia.juegoIniciado) return;

        if (Input.GetMouseButton(0))
        {
            disparando = true;
            Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicionRaton.z = 0f;

            float distancia = Vector3.Distance(transform.position, posicionRaton);

            // EVITAMOS QUE SE ENCOJA: Si la distancia al ratón es menor que el tamaño original,
            // mantenemos el tamaño original en reposo.
            if (distancia < anchoOriginal)
            {
                distancia = anchoOriginal;
            }

            Vector3 direccion = posicionRaton - transform.position;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angulo);
            spriteRenderer.size = new Vector2(distancia, alturaOriginal);

            colisionador.size = new Vector2(distancia, colisionador.size.y);
            colisionador.offset = new Vector2(distancia / 2f, 0f);
        }
        else if (Input.GetMouseButtonUp(0) && disparando)
        {
            disparando = false;
            RetraerLengua();
        }
    }

    void RetraerLengua()
    {
        // Al soltar, vuelve exactamente al tamaño que le diste en la escena
        spriteRenderer.size = new Vector2(anchoOriginal, alturaOriginal);
        colisionador.size = new Vector2(anchoOriginal, colisionador.size.y);
        colisionador.offset = new Vector2(anchoOriginal / 2f, 0f);
    }

    // --- DETECCIÓN DE CHOQUES ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        ObjetoRana objetoTocado = collision.GetComponent<ObjetoRana>();

        if (objetoTocado != null)
        {
            if (objetoTocado.esPincho)
            {
                MinijuegoRana.instancia.TocarPincho();
            }
            else
            {
                MinijuegoRana.instancia.SumarFruta();
            }

            Destroy(collision.gameObject);
            RetraerLengua();
        }
    }
}
