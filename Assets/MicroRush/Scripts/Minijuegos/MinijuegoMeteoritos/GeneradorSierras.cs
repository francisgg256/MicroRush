using UnityEngine;

/// <summary>
/// Clase que implementa una variante avanzada del patrón de diseño "Spawner".
/// Se encarga de instanciar obstáculos (sierras) de forma periódica, aplicando 
/// aleatoriedad espacial en el eje horizontal para generar patrones de esquiva impredecibles.
/// </summary>
public class GeneradorSierras : MonoBehaviour
{
    [Header("Configuración del Generador")]

    /// <summary>
    /// Objeto base (Prefab) del obstáculo que será clonado en la escena.
    /// </summary>
    public GameObject obstaculoPrefab;

    /// <summary>
    /// Intervalo de tiempo en segundos entre la aparición de cada obstáculo.
    /// </summary>
    public float tiempoEntreObstaculos = 2f;

    /// <summary>
    /// Margen máximo de desviación en el eje X respecto a la posición central del generador.
    /// Define la anchura del área de peligro (ej. un valor de 8f permite instanciar entre -8f y +8f).
    /// </summary>
    public float limiteX = 8f;

    /// <summary>
    /// Variable interna utilizada para la cuenta regresiva entre instanciaciones.
    /// </summary>
    private float temporizador;

    /// <summary>
    /// Método de inicialización. 
    /// Prepara el temporizador para que la primera sierra tarde el tiempo exacto estipulado en aparecer.
    /// </summary>
    void Start()
    {
        temporizador = tiempoEntreObstaculos;
    }

    /// <summary>
    /// Método del ciclo de vida (bucle principal).
    /// Gestiona la cuenta regresiva de forma independiente a los FPS y calcula 
    /// dinámicamente las coordenadas de aparición con un factor de aleatoriedad.
    /// </summary>
    void Update()
    {
        // Candado: Comprobamos si el manager existe y si ya dio la orden de empezar
        if (MinijuegoMeteoritos.instancia != null && !MinijuegoMeteoritos.instancia.juegoIniciado)
            return;

        // Decremento del tiempo basado en el tiempo real entre fotogramas (prevención de desincronización)
        temporizador -= Time.deltaTime;

        // Condición de instanciación: El temporizador ha expirado
        if (temporizador <= 0)
        {
            // Aleatoriedad Espacial Relativa: 
            // Calcula una posición 'X' tomando como origen la posición actual del generador (transform.position.x)
            // y sumándole un valor aleatorio dentro de los límites establecidos.
            float posX = transform.position.x + Random.Range(-limiteX, limiteX);
            Vector3 posicionAleatoria = new Vector3(posX, transform.position.y, 0f);

            // Instancia el objeto en la nueva coordenada calculada, manteniendo su rotación original (Quaternion.identity)
            Instantiate(obstaculoPrefab, posicionAleatoria, Quaternion.identity);

            // Reinicio del ciclo
            temporizador = tiempoEntreObstaculos;
        }
    }
}
