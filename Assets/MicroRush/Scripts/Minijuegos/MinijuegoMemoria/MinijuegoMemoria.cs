using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase controladora del minijuego de memoria visual (estilo "Simón Dice").
/// Gestiona secuencias procedimentales, animaciones asíncronas de la interfaz de usuario (UI) 
/// y el control estricto de turnos para prevenir interacciones no deseadas.
/// </summary>
public class MinijuegoMemoria : MonoBehaviour
{
    [Header("Control de Inicio")]
    /// <summary>Candado lógico. Evita que el juego arranque mientras se lee el cartel.</summary>
    public bool juegoIniciado = false;

    [Header("Configuración de Interfaz")]

    /// <summary>
    /// Array de componentes de imagen (UI) que actúan como los botones luminosos.
    /// Su transparencia (Canal Alfa) será manipulada para simular el estado de encendido/apagado.
    /// </summary>
    public Image[] luces;

    /// <summary>Cantidad de rondas que el jugador debe memorizar y superar para ganar el minijuego.</summary>
    public int rondasParaGanar = 4;

    /// <summary>Duración en segundos de la animación de destello. Controla el ritmo (Pacing) del juego.</summary>
    public float velocidadBrillo = 0.5f;

    /// <summary>Estructura de datos dinámica que almacena la secuencia de índices generada por el sistema.</summary>
    private List<int> secuencia = new List<int>();

    /// <summary>Índice actual que el jugador está intentando adivinar en su turno.</summary>
    private int pasoJugador = 0;

    /// <summary>Bandera de control de estado. Define si la interfaz debe aceptar o ignorar los clics del usuario.</summary>
    private bool turnoJugador = false;

    /// <summary>Bandera de seguridad que bloquea la lógica una vez resuelto el minijuego.</summary>
    private bool terminado = false;

    /// <summary>
    /// Método de inicialización.
    /// Prepara el estado visual por defecto (luces apagadas) y espera la orden de inicio.
    /// </summary>
    void Start()
    {
        // Diseño Visual: Reducimos la opacidad al 30% (0.3f) para indicar que los botones están "apagados"
        foreach (Image luz in luces)
        {
            luz.color = new Color(luz.color.r, luz.color.g, luz.color.b, 0.3f);
        }

        // IMPORTANTE: Ya no lanzamos la corrutina aquí. Esperamos al cartel.
    }

    /// <summary>Método llamado por el cartel universal de UI para desbloquear el minijuego.</summary>
    public void IniciarMinijuego()
    {
        juegoIniciado = true;
        // Ahora sí, lanzamos el primer hilo asíncrono para comenzar la ronda
        StartCoroutine(SiguienteRonda());
    }

    /// <summary>
    /// Corrutina que gestiona la lógica del turno de la CPU.
    /// Añade un nuevo paso a la secuencia, bloquea la interfaz y muestra el patrón visual al jugador.
    /// </summary>
    IEnumerator SiguienteRonda()
    {
        // Bloqueo de Interfaz: Se ignoran los inputs del usuario mientras la CPU "habla"
        turnoJugador = false;
        pasoJugador = 0;

        // Pausa dramática/cognitiva para que el usuario se prepare
        yield return new WaitForSeconds(1f);

        // Generación procedimental: Añade un nuevo índice aleatorio basado en la cantidad de luces disponibles
        secuencia.Add(Random.Range(0, luces.Length));

        // Bucle de Feedback Visual: Reproduce la secuencia acumulada
        foreach (int indiceColor in secuencia)
        {
            // Espera a que termine un destello antes de lanzar el siguiente
            yield return StartCoroutine(DestelloLuz(indiceColor));
        }

        // Desbloqueo de Interfaz: Cede el control al usuario
        turnoJugador = true;
    }

    /// <summary>
    /// Corrutina encargada del feedback visual de un botón específico.
    /// Modifica el canal Alfa del color para crear un efecto de "parpadeo" o "encendido".
    /// </summary>
    /// <param name="indice">Índice del array de luces que debe brillar.</param>
    IEnumerator DestelloLuz(int indice)
    {
        Image luz = luces[indice];

        // ENCENDIDO: Opacidad al 100% (1f)
        luz.color = new Color(luz.color.r, luz.color.g, luz.color.b, 1f);

        // El tiempo de espera bloquea esta corrutina específica sin congelar el juego entero
        yield return new WaitForSeconds(velocidadBrillo);

        // APAGADO: Opacidad al 30% (0.3f)
        luz.color = new Color(luz.color.r, luz.color.g, luz.color.b, 0.3f);

        // Pausa breve entre destellos para facilitar la lectura visual cuando se repite el mismo color
        yield return new WaitForSeconds(velocidadBrillo / 2f);
    }

    /// <summary>
    /// Evento de interfaz (UI Event) vinculado al componente 'Button' de Unity.
    /// Procesa la interacción del usuario y evalúa el progreso de la ronda.
    /// </summary>
    /// <param name="indicePulsado">El número identificativo del botón que el usuario acaba de tocar.</param>
    public void BotonPulsado(int indicePulsado)
    {
        // Prevención de Errores: Ignora clics si el juego no ha empezado, es fuera de turno, o ya terminó
        if (!juegoIniciado || !turnoJugador || terminado) return;

        // Feedback inmediato: Reacciona visualmente al toque del usuario
        StartCoroutine(DestelloLuz(indicePulsado));

        // Validación Lógica: Comprueba si el botón coincide con el patrón guardado
        if (indicePulsado == secuencia[pasoJugador])
        {
            // Acierto: Avanza el validador
            pasoJugador++;

            // Condición de Ronda: Comprueba si ha completado toda la secuencia actual
            if (pasoJugador >= secuencia.Count)
            {
                // Condición de Victoria Global
                if (secuencia.Count >= rondasParaGanar)
                {
                    Ganar();
                }
                else
                {
                    // Transición a la siguiente fase aumentando la dificultad
                    StartCoroutine(SiguienteRonda());
                }
            }
        }
        else
        {
            // Fracaso: Se rompió el patrón de memoria
            Perder();
        }
    }

    /// <summary>Notifica la victoria lógica al gestor global y bloquea el estado local.</summary>
    void Ganar()
    {
        terminado = true;
        Debug.Log("Validación de secuencia completada. Victoria alcanzada.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
    }

    /// <summary>Notifica la derrota lógica al gestor global y bloquea el estado local.</summary>
    void Perder()
    {
        terminado = true;
        Debug.Log("Error en el patrón de memoria. Derrota inmediata.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
