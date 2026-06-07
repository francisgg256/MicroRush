using UnityEngine;

/// <summary>
/// Controlador de movimiento exclusivo para el minijuego de la Cesta.
/// Utiliza un método de giro manual para evitar conflictos con el Animator
/// y respeta el tamaño original de los objetos.
/// </summary>
public class MovimientoRanaCesta : MonoBehaviour
{
    [Header("Control de Estado")]
    public bool puedeMoverse = false;

    [Header("Físicas")]
    public int velocidad = 6;
    public Rigidbody2D fisica;

    [Header("Gráficos y Cesta")]
    public SpriteRenderer spriteRana; // Arrastrar el SpriteRenderer de la rana
    public Transform cesta;           // Arrastrar el objeto de la Cesta

    private float entradaX;

    public void HabilitarMovimiento()
    {
        puedeMoverse = true;
    }

    void Update()
    {
        if (!puedeMoverse)
        {
            entradaX = 0f;
            return;
        }

        entradaX = Input.GetAxis("Horizontal");

        // --- MÉTODO ANTIBLOQUEO CORREGIDO ---
        if (entradaX < 0)
        {
            // 1. Giramos el dibujo de la rana
            if (spriteRana != null) spriteRana.flipX = true;

            // 2. Pasamos la cesta a la izquierda y la volteamos RESPETANDO SU TAMAÑO ORIGINAL
            if (cesta != null)
            {
                // Movemos posición
                Vector3 pos = cesta.localPosition;
                pos.x = -Mathf.Abs(pos.x);
                cesta.localPosition = pos;

                // Volteamos haciendo negativo su tamaño actual (sin volverlo gigante)
                Vector3 escala = cesta.localScale;
                escala.x = -Mathf.Abs(escala.x);
                cesta.localScale = escala;
            }
        }
        else if (entradaX > 0)
        {
            // 1. La rana vuelve a mirar a la derecha
            if (spriteRana != null) spriteRana.flipX = false;

            // 2. Pasamos la cesta a la derecha y la restauramos RESPETANDO SU TAMAÑO
            if (cesta != null)
            {
                // Movemos posición
                Vector3 pos = cesta.localPosition;
                pos.x = Mathf.Abs(pos.x);
                cesta.localPosition = pos;

                // Volvemos a hacer positivo su tamaño actual
                Vector3 escala = cesta.localScale;
                escala.x = Mathf.Abs(escala.x);
                cesta.localScale = escala;
            }
        }
    }

    private void FixedUpdate()
    {
        if (fisica != null)
        {
            fisica.linearVelocity = new Vector2(entradaX * velocidad, fisica.linearVelocity.y);
        }
    }
}
