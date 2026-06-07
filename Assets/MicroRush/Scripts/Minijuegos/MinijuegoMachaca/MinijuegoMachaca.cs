using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona la lógica y la interfaz del minijuego de machacar botones (Button Masher).
/// Permite configurar la dificultad desde el Inspector exponiendo la fuerza de pulsación y la gravedad de vaciado.
/// </summary>
public class MinijuegoMachaca : MonoBehaviour
{
    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public Slider barraProgreso;
    public float progreso = 0f;
    public float tiempoRestante = 5f;

    [Header("Dificultad")]
    /// <summary>Cuánto progreso suma cada pulsación de la barra espaciadora.</summary>
    public float puntosPorPulsacion = 10f;

    /// <summary>Cuánto progreso se resta automáticamente cada segundo.</summary>
    public float penalizacionPorSegundo = 15f;

    private bool terminado = false;

    void Start()
    {
        barraProgreso.value = 0f;
    }

    public void IniciarMinijuego()
    {
        juegoIniciado = true;
    }

    void Update()
    {
        if (terminado || !juegoIniciado) return;

        tiempoRestante -= Time.deltaTime;

        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        // Captura de entrada (Input): Suma el valor configurable
        if (Input.GetKeyDown(KeyCode.Space))
        {
            progreso += puntosPorPulsacion;
        }

        // Mecánica de penalización: Vaciado configurable
        progreso -= penalizacionPorSegundo * Time.deltaTime;

        progreso = Mathf.Clamp(progreso, 0f, 100f);
        barraProgreso.value = progreso / 100f;

        // Condiciones de fin
        if (progreso >= 100f)
        {
            terminado = true;
            ControlJuego.instancia.ganarMinijuego();
        }
        else if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }
}
