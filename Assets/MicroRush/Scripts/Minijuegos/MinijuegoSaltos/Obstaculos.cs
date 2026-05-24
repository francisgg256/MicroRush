using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento individual de cada obstáculo generado en el nivel.
/// Gestiona su desplazamiento autónomo, la optimización de memoria (autodestrucción) 
/// y la detección de colisiones letales con el jugador.
/// </summary>
public class Obstaculos : MonoBehaviour
{
    /// <summary>
    /// Velocidad de desplazamiento horizontal del obstáculo hacia la izquierda.
    /// Puede ser ajustada para aumentar la dificultad progresiva del minijuego.
    /// </summary>
    public float velocidad = 5f;

    /// <summary>
    /// Método del ciclo de vida que se ejecuta en cada frame.
    /// Aplica un movimiento constante e independiente de los FPS, y gestiona la limpieza de memoria.
    /// </summary>
    void Update()
    {
        // Desplaza el obstáculo hacia la izquierda calculando el tiempo real entre frames
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);

        // Optimización de recursos: Si el obstáculo sale de la pantalla (eje X < -15),
        // se destruye su GameObject para liberar memoria y evitar sobrecargar el motor (Garbage Collection).
        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Evento de las físicas 2D que se dispara cuando otro colisionador entra en el área de este objeto.
    /// Utiliza un colisionador en modo "Trigger" para detectar el impacto sin generar un rebote físico.
    /// </summary>
    /// <param name="collision">Datos del objeto (Collider2D) que ha entrado en contacto con el obstáculo.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que ha impactado tiene la etiqueta "Jugador"
        if (collision.CompareTag("Jugador"))
        {
            // Registro en consola para depuración y control de errores
            Debug.Log("Colisión letal detectada con " + collision.name);

            // Paso de mensajes: Comunica al gestor del minijuego (Singleton) que el jugador ha perdido
            MinijuegoSaltos.instancia.perder();
        }
    }
}