using UnityEngine;

/// <summary>
/// Clase que implementa el patrón de diseño "Spawner" (Generador).
/// Se encarga de instanciar de forma dinámica y periódica nuevos obstáculos en la escena.
/// Este enfoque ahorra memoria al no tener todos los objetos cargados desde el principio.
/// </summary>
public class GeneradorObstaculos : MonoBehaviour
{
    /// <summary>
    /// Objeto base (Prefab) que se va a clonar en la escena.
    /// Permite asignar desde el editor de Unity el tipo de obstáculo (ej. una sierra, una caja).
    /// </summary>
    public GameObject obstaculoPrefab;

    /// <summary>
    /// Intervalo de tiempo en segundos que transcurre entre la creación de un obstáculo y el siguiente.
    /// Controla la dificultad del minijuego (menor tiempo = mayor dificultad).
    /// </summary>
    public float tiempoEntreObstaculos = 2f;

    /// <summary>
    /// Variable interna utilizada para llevar la cuenta regresiva hasta la próxima instanciación.
    /// </summary>
    private float temporizador;

    /// <summary>
    /// Método de inicialización de Unity.
    /// Configura el temporizador interno para que el primer obstáculo tarde el tiempo exacto establecido en aparecer.
    /// </summary>
    void Start()
    {
        temporizador = tiempoEntreObstaculos;
    }

    /// <summary>
    /// Método del ciclo de vida que se ejecuta en cada frame.
    /// Gestiona la cuenta regresiva basándose en el tiempo real transcurrido para garantizar consistencia.
    /// </summary>
    void Update()
    {
        // Candado: Comprobamos si el manager existe y si ya ha dado la orden de inicio
        if (MinijuegoSaltos.instancia != null && !MinijuegoSaltos.instancia.juegoIniciado)
            return;

        // Resta el tiempo que ha tardado en renderizarse el último frame (Time.deltaTime).
        temporizador -= Time.deltaTime;

        // Cuando el temporizador llega a cero o menos, es hora de generar un nuevo obstáculo
        if (temporizador <= 0)
        {
            // Crea una copia del prefab en la posición exacta del generador, sin alterar su rotación
            Instantiate(obstaculoPrefab, transform.position, Quaternion.identity);

            // Reinicia el contador para el siguiente ciclo
            temporizador = tiempoEntreObstaculos;
        }
    }
}