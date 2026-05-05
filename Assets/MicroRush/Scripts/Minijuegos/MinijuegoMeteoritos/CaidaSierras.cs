using UnityEngine;

public class CaidaSierras : MonoBehaviour
{
    public float velocidad = 5f;

    void Update()
    {
        // Vector2.down para que caiga
        transform.Translate(Vector2.down * velocidad * Time.deltaTime);

        // Si la Y baja de -10 (ajusta este número según tu cámara) se destruye
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Asegúrate de que tu personaje tiene el Tag "Jugador" en el Inspector
        if (collision.CompareTag("Jugador"))
        {
            Debug.Log("Colisión con " + collision.name);

            // Llamamos al script general de tu juego para perder
            if (ControlJuego.instancia != null)
            {
                ControlJuego.instancia.perderMinijuego();
            }
        }
    }
}
