using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento de los objetos coleccionables (frutas) en la escena.
/// Gestiona la detección de colisiones con el jugador, la comunicación con el gestor del minijuego 
/// y proporciona feedback visual (animaciones) antes de liberar la memoria.
/// </summary>
public class Fruta : MonoBehaviour
{
    private bool yaRecogida = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D frutaCollider;

    public float tiempoDestruccion = 0.5f;

    /// <summary>Referencia cacheada al manager para comprobar si el juego ha iniciado.</summary>
    private MinijuegoFrutas manager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        frutaCollider = GetComponent<Collider2D>();
        manager = FindAnyObjectByType<MinijuegoFrutas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Candado: Comprobamos si el manager existe y si el juego ya ha iniciado
        if (manager != null && !manager.juegoIniciado) return;

        // Validación múltiple
        if (collision.CompareTag("Jugador") && !yaRecogida && animator != null)
        {
            yaRecogida = true;

            // Paso de mensajes
            manager.FrutaRecogida(gameObject);

            if (frutaCollider != null) frutaCollider.enabled = false;

            // Feedback Visual
            animator.SetTrigger("recogerFruta");

            // Optimización
            Destroy(gameObject, tiempoDestruccion);
        }
    }
}
