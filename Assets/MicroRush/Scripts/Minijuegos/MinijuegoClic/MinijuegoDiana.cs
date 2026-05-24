using UnityEngine;

/// <summary>
/// Clase que gestiona la lógica central, el temporizador y las condiciones de victoria 
/// del minijuego de reflejos y precisión (Point & Click).
/// Implementa el patrón Singleton para recibir mensajes directamente de los objetos interactivos.
/// </summary>
public class MinijuegoDiana : MonoBehaviour
{
    /// <summary>
    /// Instancia estática (Singleton) de la clase.
    /// Centraliza la recepción de eventos de acierto o fallo provenientes de los elementos de la interfaz.
    /// </summary>
    public static MinijuegoDiana instancia;

    [Header("Configuración del Minijuego")]

    /// <summary>
    /// Tiempo límite en segundos que tiene el usuario para completar el objetivo.
    /// </summary>
    public float duracion = 10f;

    /// <summary>
    /// Cantidad de interacciones exitosas (clics) requeridas para superar el nivel.
    /// </summary>
    public int frutasNecesarias = 5;

    /// <summary>Variable interna para el seguimiento de la cuenta regresiva temporal.</summary>
    private float tiempoRestante;

    /// <summary>Contador del progreso actual del jugador.</summary>
    private int frutasExplotadas = 0;

    /// <summary>Bandera de seguridad para bloquear la ejecución una vez resuelto el minijuego.</summary>
    private bool terminado = false;

    /// <summary>
    /// Método de inicialización temprana.
    /// Establece el patrón Singleton y verifica la integridad de las dependencias globales.
    /// </summary>
    private void Awake()
    {
        instancia = this;

        // Control de Excepciones: Valida la existencia del gestor global en la jerarquía de la escena
        if (ControlJuego.instancia == null)
        {
            Debug.LogError("Error Crítico: El gestor principal 'ControlJuego' no se encuentra en la escena.");
        }
    }

    /// <summary>
    /// Método de configuración inicial.
    /// </summary>
    void Start()
    {
        tiempoRestante = duracion;
    }

    /// <summary>
    /// Bucle de actualización principal.
    /// Gestiona la sincronización del temporizador con el HUD global y evalúa la condición de derrota por tiempo.
    /// </summary>
    void Update()
    {
        if (terminado) return;

        // Decremento del tiempo de forma independiente a la tasa de fotogramas (FPS)
        tiempoRestante -= Time.deltaTime;

        // Visibilidad del Estado (UX): Envía el tiempo restante al HUD global para mantener informado al jugador
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;
        }

        // Condición de Derrota: El jugador no alcanzó la cuota de interacciones antes de agotar el tiempo
        if (tiempoRestante <= 0)
        {
            perder();
        }
    }

    /// <summary>
    /// Evento invocado externamente por los objetos interactivos (ObjetivoClick) al ser pulsados con éxito.
    /// Incrementa el progreso y evalúa dinámicamente la condición de victoria.
    /// </summary>
    public void SumarAcierto()
    {
        if (terminado) return;

        frutasExplotadas++;

        // Trazabilidad y depuración del sistema de progresión
        Debug.Log("Interacción exitosa registrada. Progreso: " + frutasExplotadas + " / " + frutasNecesarias);

        // Condición de Victoria: Se ha alcanzado o superado el objetivo de interacciones a tiempo
        if (frutasExplotadas >= frutasNecesarias)
        {
            terminarVictoria();
        }
    }

    /// <summary>
    /// Bloquea el estado del minijuego y notifica la derrota al sistema general.
    /// Puede ser llamado por el agotamiento del tiempo o por interactuar con un objeto penalizador (peligro).
    /// </summary>
    public void perder()
    {
        if (terminado) return;

        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }

    /// <summary>
    /// Bloquea el estado del minijuego y notifica la victoria al sistema general.
    /// </summary>
    private void terminarVictoria()
    {
        if (terminado) return;

        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }
}
