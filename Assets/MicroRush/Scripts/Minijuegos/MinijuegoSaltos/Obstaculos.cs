using UnityEngine;

public class Obstaculos : MonoBehaviour
{
    public float velocidad = 5f;

    void Update()
    {
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);

        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            Debug.Log("Colisión con " + collision.name);
            MinijuegoSaltos.instancia.perder();
        }
    }
}