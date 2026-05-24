using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona la lógica y la interfaz del minijuego de machacar botones (Button Masher).
/// El jugador debe pulsar repetidamente un control para llenar una barra de progreso antes de que se agote el tiempo,
/// contrarrestando un vaciado constante de la misma.
/// </summary>
public class MinijuegoMachaca : MonoBehaviour
{
    /// <summary>
    /// Componente de interfaz de usuario (UI Slider) que proporciona feedback visual inmediato al jugador.
    /// Cumple con la heurística de usabilidad de mostrar el estado del sistema en tiempo real.
    /// </summary>
    public Slider barraProgreso;

    /// <summary>
    /// Variable interna que almacena el progreso actual del llenado (de 0 a 100).
    /// </summary>
    public float progreso = 0f;

    /// <summary>
    /// Tiempo límite en segundos para completar el desafío.
    /// </summary>
    public float tiempoRestante = 10f;

    /// <summary>
    /// Bandera (flag) de control que evita ejecuciones múltiples de las condiciones de victoria o derrota.
    /// </summary>
    private bool terminado = false;

    /// <summary>
    /// Método de inicialización. Asegura que la barra de progreso visual comience vacía,
    /// sincronizando el estado lógico con el estado gráfico.
    /// </summary>
    void Start()
    {
        barraProgreso.value = 0f;
    }

    /// <summary>
    /// Bucle principal del minijuego.
    /// Gestiona la entrada del usuario, la penalización de tiempo, la actualización de la interfaz
    /// y la evaluación de las condiciones de fin de partida.
    /// </summary>
    void Update()
    {
        // Si el juego ha finalizado, detenemos la ejecución para optimizar recursos
        if (terminado) return;

        // Gestión del temporizador independiente de los FPS del sistema
        tiempoRestante -= Time.deltaTime;

        // Comunicación segura con el Singleton global para actualizar el HUD
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        // Captura de entrada (Input): Suma progreso con cada pulsación rápida de la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            progreso += 10f;
        }

        // Mecánica de penalización: La barra se vacía progresivamente con el paso del tiempo
        progreso -= 15f * Time.deltaTime;

        // Control de Errores y Límites: Asegura mediante un Clamp que el valor matemático
        // nunca sea menor que 0 ni mayor que 100, evitando bugs de desbordamiento en la UI.
        progreso = Mathf.Clamp(progreso, 0f, 100f);

        // Actualización de la Interfaz Visual: El componente Slider de Unity requiere un valor normalizado (entre 0 y 1)
        barraProgreso.value = progreso / 100f;

        // Condición de Victoria: El jugador ha logrado llenar la barra
        if (progreso >= 100f)
        {
            terminado = true;
            ControlJuego.instancia.ganarMinijuego();
        }
        // Condición de Derrota: El temporizador ha llegado a 0 antes de llenar la barra
        else if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }
}
