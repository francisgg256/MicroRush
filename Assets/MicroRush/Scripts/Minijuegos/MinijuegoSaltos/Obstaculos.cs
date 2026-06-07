using UnityEngine;

/// <summary>
/// Controla el movimiento del obstáculo. En el Nivel 2 se mueve más rápido 
/// leyendo el multiplicador del Gestor.
/// </summary>
public class Obstaculos : MonoBehaviour
{
    public float velocidad = 5f;

    void Update()
    {
        if (MinijuegoSaltos.instancia != null && !MinijuegoSaltos.instancia.juegoIniciado)
            return;

        // Leemos la dificultad actual
        float multiplicador = MinijuegoSaltos.instancia != null ? MinijuegoSaltos.instancia.multiplicadorDificultad : 1f;

        // Desplaza el obstáculo multiplicando la velocidad base por la dificultad extra
        transform.Translate(Vector2.left * velocidad * multiplicador * Time.deltaTime);

        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MinijuegoSaltos.instancia != null && !MinijuegoSaltos.instancia.juegoIniciado)
            return;

        if (collision.CompareTag("Jugador"))
        {
            Debug.Log("Colisión letal detectada con " + collision.name);
            if (MinijuegoSaltos.instancia != null)
            {
                MinijuegoSaltos.instancia.perder();
            }
        }
    }
}