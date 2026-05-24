using UnityEngine;

/// <summary>
/// Clase que gestiona la lógica central y la condición de victoria del minijuego de supervivencia de meteoritos.
/// Implementa el patrón Singleton para facilitar el paso de mensajes desde los obstáculos generados dinámicamente.
/// </summary>
public class MinijuegoMeteoritos : MonoBehaviour
{
    /// <summary>
    /// Instancia estática única (Singleton) accesible de forma global.
    /// Permite a los meteoritos notificar colisiones letales directamente a este gestor.
    /// </summary>
    public static MinijuegoMeteoritos instancia;

    /// <summary>
    /// Tiempo total en segundos que el jugador debe sobrevivir esquivando meteoritos para superar la prueba.
    /// </summary>
    public float duracion = 10f;

    /// <summary>
    /// Temporizador interno que lleva la cuenta regresiva del tiempo de supervivencia actual.
    /// </summary>
    public float tiempoRestante;

    /// <summary>
    /// Bandera de estado (flag) que bloquea la ejecución de lógica adicional una vez 
    /// que el jugador ha ganado o perdido, previniendo bucles o doble puntuación.
    /// </summary>
    private bool terminado = false;

    /// <summary>
    /// Método de inicialización temprana.
    /// Asigna la instancia global y realiza una comprobación de dependencias críticas.
    /// </summary>
    private void Awake()
    {
        instancia = this;

        // Control de Excepciones: Advierte en consola si el juego se ha iniciado desde esta escena 
        // sin pasar por el menú principal, lo cual dejaría al nivel sin su gestor global.
        if (ControlJuego.instancia == null)
        {
            Debug.LogError("Error de Dependencia: No se ha detectado 'ControlJuego' en la escena activa.");
        }
    }

    /// <summary>
    /// Método de configuración inicial que reinicia el temporizador al valor máximo estipulado.
    /// </summary>
    void Start()
    {
        tiempoRestante = duracion;
    }

    /// <summary>
    /// Bucle principal del minijuego.
    /// Se encarga de restar el tiempo, actualizar la interfaz gráfica a través del gestor principal 
    /// y comprobar constantemente si se ha cumplido la condición de victoria.
    /// </summary>
    void Update()
    {
        if (terminado) return;

        // Sincronización del tiempo basada en los fotogramas reales procesados
        tiempoRestante -= Time.deltaTime;

        // Paso de mensajes: Actualiza el HUD en tiempo real de forma segura
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;
        }

        // Condición de Victoria: El tiempo de supervivencia se ha agotado por completo
        if (tiempoRestante <= 0)
        {
            terminarVictoria();
        }
    }

    /// <summary>
    /// Detiene el progreso del minijuego y notifica la derrota al sistema global.
    /// Este método está diseñado para ser invocado externamente por los propios meteoritos al impactar.
    /// </summary>
    public void perder()
    {
        if (terminado) return;

        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }

    /// <summary>
    /// Gestiona la transición lógica hacia el éxito del nivel.
    /// Se ejecuta internamente cuando el jugador logra evadir todos los obstáculos durante el tiempo establecido.
    /// </summary>
    private void terminarVictoria()
    {
        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }
}
