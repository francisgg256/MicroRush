using UnityEngine;

/// <summary>
/// Controlador principal del minijuego de sigilo y reflejos (Semaforo).
/// Implementa una Máquina de Estados Finitos (FSM) para gestionar las fases de tiempo
/// y proporciona un fuerte feedback visual utilizando la psicología del color.
/// </summary>
public class MinijuegoSemaforo : MonoBehaviour
{
    /// <summary>
    /// Instancia Singleton que permite a otros scripts (como el del jugador) consultar 
    /// el estado actual del semáforo para determinar si el movimiento es una infracción.
    /// </summary>
    public static MinijuegoSemaforo instancia;

    [Header("Configuración")]

    /// <summary>Tiempo límite global en segundos para que el jugador cruce la meta.</summary>
    public float duracion = 10f;

    /// <summary>Componente visual del semáforo que cambiará de color para advertir al jugador.</summary>
    public SpriteRenderer luzSemaforo;

    /// <summary>
    /// Variable de estado actual (0 = Verde, 1 = Amarillo, 2 = Rojo).
    /// Se oculta en el Inspector para evitar manipulaciones accidentales durante el diseño del nivel,
    /// pero es pública para que el script del jugador pueda consultarla.
    /// </summary>
    [HideInInspector] public int estadoSemaforo = 0;

    /// <summary>Temporizador global de la partida.</summary>
    private float tiempoRestante;

    /// <summary>Temporizador local que dicta cuánto tiempo queda para la siguiente transición de estado.</summary>
    private float temporizadorCambio;

    /// <summary>Bandera de seguridad que congela la lógica una vez determinada la victoria o derrota.</summary>
    private bool terminado = false;

    /// <summary>Inicialización temprana del patrón Singleton.</summary>
    private void Awake()
    {
        instancia = this;
    }

    /// <summary>
    /// Método de configuración inicial.
    /// Establece el estado seguro por defecto (Verde) y calcula un tiempo aleatorio para el primer cambio.
    /// </summary>
    void Start()
    {
        tiempoRestante = duracion;
        CambiarLuz(0);
        temporizadorCambio = Random.Range(1.5f, 3.5f);
    }

    /// <summary>
    /// Bucle de lógica principal.
    /// Evalúa de forma concurrente el tiempo límite global y las transiciones de la máquina de estados.
    /// </summary>
    void Update()
    {
        if (terminado) return;

        // 1. Lógica del tiempo general y actualización del HUD
        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        // Condición de Derrota: El jugador no alcanza la meta antes del fin del tiempo global
        if (tiempoRestante <= 0)
        {
            Perder();
        }

        // 2. Lógica de la Máquina de Estados del Semáforo
        temporizadorCambio -= Time.deltaTime;
        if (temporizadorCambio <= 0)
        {
            AvanzarSemaforo();
        }
    }

    /// <summary>
    /// Gestiona las transiciones de la Máquina de Estados Finitos (FSM).
    /// Calcula dinámicamente la duración del siguiente estado para hacer el patrón impredecible.
    /// </summary>
    void AvanzarSemaforo()
    {
        if (estadoSemaforo == 0) // Transición: VERDE -> AMARILLO
        {
            CambiarLuz(1);
            // Diseño de Usabilidad: El amarillo es breve, actuando únicamente como un aviso (Feedback anticipado)
            temporizadorCambio = 0.8f;
        }
        else if (estadoSemaforo == 1) // Transición: AMARILLO -> ROJO
        {
            CambiarLuz(2);
            temporizadorCambio = Random.Range(1f, 2f);
        }
        else if (estadoSemaforo == 2) // Transición: ROJO -> VERDE
        {
            CambiarLuz(0);
            temporizadorCambio = Random.Range(1.5f, 3.5f);
        }
    }

    /// <summary>
    /// Aplica el cambio de estado lógico y actualiza inmediatamente la interfaz visual.
    /// Utiliza colores universales de la semiótica para que el usuario entienda las reglas sin tutoriales.
    /// </summary>
    /// <param name="nuevoEstado">Código numérico del nuevo estado a aplicar.</param>
    void CambiarLuz(int nuevoEstado)
    {
        estadoSemaforo = nuevoEstado;

        // Modificación dinámica del material/color basándose en el estado actual
        if (estadoSemaforo == 0) luzSemaforo.color = Color.green;
        else if (estadoSemaforo == 1) luzSemaforo.color = Color.yellow;
        else if (estadoSemaforo == 2) luzSemaforo.color = Color.red;
    }

    /// <summary>Notifica la victoria al gestor global y bloquea el estado.</summary>
    public void Ganar()
    {
        if (terminado) return;
        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }

    /// <summary>Notifica la derrota al gestor global y bloquea el estado.</summary>
    public void Perder()
    {
        if (terminado) return;
        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }
}
