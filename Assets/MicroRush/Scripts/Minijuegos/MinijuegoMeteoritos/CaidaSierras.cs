using UnityEngine;

/// <summary>
/// Clase que controla el comportamiento físico y lógico de los obstáculos descendentes (sierras).
/// Gestiona su trayectoria vertical, la optimización de memoria (autodestrucción) al salir de cámara, 
/// y la detección de colisiones letales con el jugador.
/// </summary>
public class CaidaSierras : MonoBehaviour
{
    /// <summary>
    /// Velocidad de desplazamiento vertical del obstáculo hacia abajo.
    /// Define el nivel de desafío y el tiempo de reacción disponible para el jugador.
    /// </summary>
    public float velocidad = 5f;

    /// <summary>
    /// Método del ciclo de vida de Unity que se ejecuta fotograma a fotograma.
    /// Aplica una traslación constante e independiente del procesador (Time.deltaTime) 
    /// y evalúa la limpieza de memoria.
    /// </summary>
    void Update()
    {
        // Candado: Si el juego no ha empezado, las sierras se quedan congeladas en el aire
        if (MinijuegoMeteoritos.instancia != null && !MinijuegoMeteoritos.instancia.juegoIniciado)
            return;

        // Movimiento constante hacia abajo (Vector2.down) sincronizado con el tiempo real
        transform.Translate(Vector2.down * velocidad * Time.deltaTime);

        // Optimización de Memoria (Garbage Collection):
        // Cuando el obstáculo cruza el límite inferior de la cámara (Y < -10f), 
        // se destruye el GameObject para no consumir recursos computacionales innecesariamente.
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Evento del motor de físicas 2D que detecta la superposición de áreas (Trigger).
    /// Se utiliza para registrar el impacto letal sin aplicar fuerzas de rebote mecánicas.
    /// </summary>
    /// <param name="collision">Datos del componente Collider2D que ha entrado en contacto con la sierra.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Candado: Evitamos impactos accidentales antes de que empiece el minijuego
        if (MinijuegoMeteoritos.instancia != null && !MinijuegoMeteoritos.instancia.juegoIniciado)
            return;

        // Filtrado por etiqueta (Tag): Verifica que el impacto haya sido exclusivamente con el jugador
        if (collision.CompareTag("Jugador"))
        {
            // Trazabilidad para el desarrollador en la consola de depuración
            Debug.Log("Colisión letal detectada con " + collision.name);

            // Usamos el gestor local para perder, que ya tiene las comprobaciones necesarias
            if (MinijuegoMeteoritos.instancia != null)
            {
                MinijuegoMeteoritos.instancia.perder();
            }
        }
    }
}
