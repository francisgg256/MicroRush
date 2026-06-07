using System.Collections;
using UnityEngine;

/// <summary>
/// Controlador principal del minijuego de sigilo y reflejos (Semaforo).
/// Implementa una Máquina de Estados Finitos (FSM) que ahora incluye el "Modo Troll" para el Nivel 2.
/// </summary>
public class MinijuegoSemaforo : MonoBehaviour
{
    public static MinijuegoSemaforo instancia;

    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float duracion = 10f;
    public SpriteRenderer luzSemaforo;
    [HideInInspector] public int estadoSemaforo = 0;

    [Header("Modo Troll (Nivel 2)")]
    /// <summary>Activa esta casilla para que el semáforo engañe al jugador.</summary>
    public bool modoTroll = false;

    /// <summary>Probabilidad (0-100) de que pase de Amarillo a Verde en lugar de Rojo.</summary>
    [Range(0f, 100f)] public float probFalsaAlarma = 35f;

    /// <summary>Probabilidad (0-100) de que pase de Verde a Rojo directamente, sin avisar.</summary>
    [Range(0f, 100f)] public float probSaltoRojo = 20f;

    private float tiempoRestante;
    private float temporizadorCambio;
    private bool terminado = false;

    private void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        tiempoRestante = duracion;
        CambiarLuz(0); // Empezamos en verde
        temporizadorCambio = Random.Range(1.5f, 3.5f);
    }

    public void IniciarMinijuego()
    {
        juegoIniciado = true;
    }

    void Update()
    {
        if (terminado || !juegoIniciado) return;

        // 1. Lógica del tiempo general
        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

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
    /// Gestiona las transiciones. En Nivel 2, altera el flujo lógico de los colores para engañar al jugador.
    /// </summary>
    void AvanzarSemaforo()
    {
        if (estadoSemaforo == 0) // ESTADO ACTUAL: VERDE
        {
            // ¿Hacemos la trampa del susto? (Verde -> Rojo directo)
            if (modoTroll && Random.Range(0f, 100f) <= probSaltoRojo)
            {
                CambiarLuz(2);
                temporizadorCambio = Random.Range(1f, 2f); // Se queda en rojo un ratito
            }
            else
            {
                // Flujo normal: Verde -> Amarillo
                CambiarLuz(1);

                // En modo troll, el amarillo dura tiempos súper aleatorios para despistar más
                temporizadorCambio = modoTroll ? Random.Range(0.3f, 1.2f) : 0.8f;
            }
        }
        else if (estadoSemaforo == 1) // ESTADO ACTUAL: AMARILLO
        {
            // ¿Hacemos la trampa de la falsa alarma? (Amarillo -> Verde)
            if (modoTroll && Random.Range(0f, 100f) <= probFalsaAlarma)
            {
                CambiarLuz(0);
                temporizadorCambio = Random.Range(1.5f, 3f); // Vuelve a dejarte correr
            }
            else
            {
                // Flujo normal: Amarillo -> Rojo
                CambiarLuz(2);
                temporizadorCambio = Random.Range(1f, 2f);
            }
        }
        else if (estadoSemaforo == 2) // ESTADO ACTUAL: ROJO
        {
            // El rojo siempre vuelve a verde para que el juego pueda avanzar
            CambiarLuz(0);
            temporizadorCambio = Random.Range(1.5f, 3.5f);
        }
    }

    void CambiarLuz(int nuevoEstado)
    {
        estadoSemaforo = nuevoEstado;

        if (estadoSemaforo == 0) luzSemaforo.color = Color.green;
        else if (estadoSemaforo == 1) luzSemaforo.color = Color.yellow;
        else if (estadoSemaforo == 2) luzSemaforo.color = Color.red;
    }

    public void Ganar()
    {
        if (terminado) return;
        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }

    public void Perder()
    {
        if (terminado) return;
        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }
}
