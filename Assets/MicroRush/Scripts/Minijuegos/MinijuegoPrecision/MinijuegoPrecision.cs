using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona el minijuego de precisión y reflejos.
/// Transforma un componente de interfaz de usuario (Slider) en una mecánica interactiva 
/// donde el jugador debe detener un indicador dentro de una "zona segura" específica.
/// </summary>
public class MinijuegoPrecision : MonoBehaviour
{
    [Header("Control de Inicio")]
    /// <summary>Candado lógico. Evita que el nivel y el tiempo funcionen mientras se lee el cartel.</summary>
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    /// <summary>
    /// Componente visual (UI Slider) cuyo valor oscila de izquierda a derecha.
    /// Sirve simultáneamente como indicador visual y como núcleo de la mecánica de juego.
    /// </summary>
    public Slider barraObjetivo;

    /// <summary>
    /// Velocidad a la que el indicador se desplaza por la barra.
    /// Ajustar este valor permite escalar la curva de dificultad del nivel.
    /// </summary>
    public float velocidad = 1.5f;

    /// <summary>
    /// Tiempo límite en segundos para que el jugador tome una decisión y pulse el botón.
    /// </summary>
    public float tiempoRestante = 5f;

    [Header("Zona de Precisión (Valores entre 0 y 1)")]
    /// <summary>Límite inferior de la zona de acierto (ej. 0.45 = 45% de la barra).</summary>
    public float margenMinimo = 0.45f;

    /// <summary>Límite superior de la zona de acierto (ej. 0.55 = 55% de la barra).</summary>
    public float margenMaximo = 0.55f;

    /// <summary>Bandera que controla la dirección actual de la oscilación del indicador.</summary>
    private bool moviendoDerecha = true;

    /// <summary>Bandera para prevenir que el jugador envíe múltiples pulsaciones una vez terminado el juego.</summary>
    private bool terminado = false;

    /// <summary>
    /// Método de inicialización. 
    /// Establece el indicador en el extremo izquierdo (valor 0) para un inicio limpio y predecible.
    /// </summary>
    void Start()
    {
        barraObjetivo.value = 0f;
    }

    /// <summary>Método llamado por el cartel universal de UI para desbloquear el minijuego.</summary>
    public void IniciarMinijuego()
    {
        juegoIniciado = true;
    }

    /// <summary>
    /// Bucle principal del minijuego.
    /// Gestiona la actualización de la UI, la lógica de oscilación matemática (Ping-Pong) 
    /// y la captura de la respuesta del usuario.
    /// </summary>
    void Update()
    {
        // Candado: Corta la ejecución si el nivel ya está resuelto o el cartel sigue en pantalla
        if (terminado || !juegoIniciado) return;

        // Gestión del tiempo y comunicación con el HUD global
        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        // Derrota por tiempo agotado (Falta de respuesta del usuario)
        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
            return; // Sale inmediatamente del Update para no evaluar pulsaciones tardías
        }

        // Lógica de oscilación dinámica de la interfaz gráfica (Efecto Ping-Pong)
        if (moviendoDerecha)
        {
            // Incrementa el valor basándose en el tiempo real para asegurar fluidez visual en la UI
            barraObjetivo.value += velocidad * Time.deltaTime;
            // Invierte la dirección al chocar con el borde derecho
            if (barraObjetivo.value >= 1f) moviendoDerecha = false;
        }
        else
        {
            barraObjetivo.value -= velocidad * Time.deltaTime;
            // Invierte la dirección al chocar con el borde izquierdo
            if (barraObjetivo.value <= 0f) moviendoDerecha = true;
        }

        // Captura de eventos del usuario
        if (Input.GetKeyDown(KeyCode.Space))
        {
            terminado = true;

            // Validación de rango: Comprueba si el valor del Slider se detuvo dentro de la zona segura configurada
            if (barraObjetivo.value >= margenMinimo && barraObjetivo.value <= margenMaximo)
            {
                ControlJuego.instancia.ganarMinijuego();
            }
            else
            {
                ControlJuego.instancia.perderMinijuego();
            }
        }
    }
}