using UnityEngine;

/// <summary>
/// Generador de obstáculos que ajusta su cadencia de disparo suavemente
/// basándose en la duración total del minijuego usando Mathf.Lerp.
/// </summary>
public class GeneradorSierras : MonoBehaviour
{
    [Header("Configuración Básica")]
    public GameObject obstaculoPrefab;
    public float limiteX = 8f;

    [Header("Dificultad Avanzada (Nivel 2)")]
    public bool apuntarAlJugador = false;
    public Transform jugador;

    [Header("Aceleración Automática (Lerp)")]
    /// <summary>Cadencia al primer segundo del minijuego.</summary>
    public float tiempoInicial = 1.5f;

    /// <summary>Cadencia en el último segundo del minijuego.</summary>
    public float tiempoMinimo = 0.4f;

    /// <summary>Cuánto dura tu minijuego en segundos (para calcular la curva).</summary>
    public float duracionMinijuego = 7f;

    private float temporizador;
    private float tiempoTranscurrido = 0f;

    void Start()
    {
        temporizador = tiempoInicial;

        // Auto-búsqueda del jugador
        if (apuntarAlJugador && jugador == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Jugador");
            if (obj != null) jugador = obj.transform;
        }
    }

    void Update()
    {
        // Candado lógico de las instrucciones
        if (MinijuegoMeteoritos.instancia != null && !MinijuegoMeteoritos.instancia.juegoIniciado)
            return;

        // Llevamos la cuenta del tiempo global y del temporizador de disparo
        tiempoTranscurrido += Time.deltaTime;
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            float posX = 0f;

            // 1. LÓGICA DE POSICIÓN
            if (apuntarAlJugador && jugador != null)
            {
                posX = jugador.position.x;
            }
            else
            {
                posX = transform.position.x + Random.Range(-limiteX, limiteX);
            }

            Vector3 posicionAparicion = new Vector3(posX, transform.position.y, 0f);
            Instantiate(obstaculoPrefab, posicionAparicion, Quaternion.identity);

            // 2. LÓGICA DE ACELERACIÓN SUAVE AL TIEMPO DEL JUEGO
            // Interpolamos entre el tiempo inicial y el mínimo según el porcentaje completado del minijuego
            float nuevoTiempoEspera = Mathf.Lerp(tiempoInicial, tiempoMinimo, tiempoTranscurrido / duracionMinijuego);

            temporizador = nuevoTiempoEspera;
        }
    }
}
