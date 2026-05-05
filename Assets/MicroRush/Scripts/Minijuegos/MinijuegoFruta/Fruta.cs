using UnityEngine;

public class Fruta : MonoBehaviour
{
    private bool yaRecogida = false;
    private Animator animator; 
    private SpriteRenderer spriteRenderer; 
    private Collider2D frutaCollider; 
    public float tiempoDestruccion = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        frutaCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador") && !yaRecogida && animator != null)
        {
            yaRecogida = true; 

            FindAnyObjectByType<MinijuegoFrutas>().FrutaRecogida(gameObject);

            if (frutaCollider != null) frutaCollider.enabled = false;

            animator.SetTrigger("recogerFruta");

            Destroy(gameObject, tiempoDestruccion);
        }
    }
}
