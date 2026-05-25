using UnityEngine;

/// <summary>
/// Clase que gestiona la lógica específica del minijuego de saltos.
/// Implementa el patrón de diseño Singleton para permitir un acceso global rápido
/// y controla la condición de victoria basada en la supervivencia durante un tiempo determinado.
/// </summary>
public class MinijuegoSaltos : MonoBehaviour
{
    /// <summary>
    /// Instancia estática (Singleton) de la clase.
    /// Permite que otros scripts (como los obstáculos o el jugador) informen de colisiones 
    /// sin necesidad de buscar la referencia en la escena, optimizando el rendimiento.
    /// </summary>
    public static MinijuegoSaltos instancia;

    [Header("Control de Inicio")]
    /// <summary>Candado lógico. Evita que el nivel y el tiempo funcionen mientras se lee el cartel.</summary>
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    /// <summary>
    /// Duración total del minijuego en segundos. 
    /// Representa el tiempo que el jugador debe sobrevivir esquivando obstáculos para ganar.
    /// </summary>
    public float duracion = 10f;

    /// <summary>
    /// Temporizador interno que rastrea el tiempo de supervivencia restante en la partida actual.
    /// </summary>
    public float tiempoRestante;

    /// <summary>
    /// Bandera (flag) de control de estado.
    /// Evita que las funciones de victoria o derrota se ejecuten múltiples veces por accidente.
    /// </summary>
    private bool terminado = false;

    /// <summary>
    /// Método de inicialización temprana de Unity.
    /// Configura el patrón Singleton y realiza un control de errores y dependencias.
    /// </summary>
    private void Awake()
    {
        // Asignación de la instancia global
        instancia = this;

        // Control de excepciones: verifica que el gestor principal del juego exista en la escena
        if (ControlJuego.instancia == null)
        {
            Debug.LogError("Error Crítico: No hay instancia de ControlJuego en la escena.");
        }
    }

    /// <summary>
    /// Método de configuración inicial. Establece el temporizador al valor máximo al iniciar el nivel.
    /// </summary>
    void Start()
    {
        tiempoRestante = duracion;
    }

    /// <summary>Método llamado por el cartel universal de UI para desbloquear el minijuego.</summary>
    public void IniciarMinijuego()
    {
        juegoIniciado = true;
    }

    /// <summary>
    /// Método del ciclo de vida que se ejecuta fotograma a fotograma.
    /// Se encarga de actualizar el HUD global y evaluar constantemente la condición de victoria.
    /// </summary>
    void Update()
    {
        // Candado: Si el minijuego ya terminó o no ha empezado, cortamos la ejecución
        if (terminado || !juegoIniciado) return;

        // Cuenta regresiva independiente de los fotogramas por segundo (FPS) del dispositivo
        tiempoRestante -= Time.deltaTime;

        // Comunicación de mensajes: Actualizamos la UI en el gestor principal de forma segura
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;
        }

        // Condición de victoria: El tiempo ha llegado a cero y el jugador ha sobrevivido
        if (tiempoRestante <= 0)
        {
            terminarVictoria();
        }
    }

    /// <summary>
    /// Detiene el minijuego y notifica al gestor principal que el jugador ha fracasado.
    /// Se invoca externamente (ej. cuando el jugador colisiona con un obstáculo letal).
    /// </summary>
    public void perder()
    {
        if (terminado || !juegoIniciado) return;

        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }

    /// <summary>
    /// Detiene el minijuego y notifica al gestor principal que el jugador ha superado la prueba.
    /// Se ejecuta internamente al expirar el tiempo de supervivencia.
    /// </summary>
    private void terminarVictoria()
    {
        if (terminado) return;

        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }
}