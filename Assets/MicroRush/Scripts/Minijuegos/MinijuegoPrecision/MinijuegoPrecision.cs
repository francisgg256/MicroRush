using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona el minijuego de precisión y reflejos.
/// Soporta modos básicos de un solo impacto y variantes avanzadas (Nivel 2)
/// con múltiples aciertos obligatorios, aceleración controlada y posicionamiento procedimental.
/// </summary>
public class MinijuegoPrecision : MonoBehaviour
{
    [Header("Control de Inicio")]
    /// <summary>Candado lógico. Evita que el nivel y el tiempo funcionen mientras se lee el cartel.</summary>
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    /// <summary>Componente visual (UI Slider) cuyo valor oscila de izquierda a derecha.</summary>
    public Slider barraObjetivo;

    /// <summary>Velocidad inicial a la que el indicador se desplaza por la barra.</summary>
    public float velocidad = 1.2f;

    /// <summary>Tiempo límite en segundos para que el jugador tome todas las decisiones.</summary>
    public float tiempoRestante = 5f;

    [Header("Zona de Precisión Estática (Nivel 1)")]
    /// <summary>Límite inferior de la zona de acierto (ej. 0.45 = 45% de la barra).</summary>
    public float margenMinimo = 0.45f;

    /// <summary>Límite superior de la zona de acierto (ej. 0.55 = 55% de la barra).</summary>
    public float margenMaximo = 0.55f;

    [Header("Dificultad Avanzada (Nivel 2)")]
    /// <summary>Si es true, la zona segura cambiará de lugar aleatoriamente.</summary>
    public bool usarZonaAleatoria = false;

    /// <summary>Cuántas veces consecutivas debe acertar el usuario dentro del tiempo límite.</summary>
    public int aciertosNecesarios = 1;

    /// <summary>Ancho fijo de la barra verde en el modo aleatorio (ej. 0.06f = 6% del total del slider).</summary>
    public float anchoZonaAcierto = 0.06f;

    /// <summary>Cuánto aumenta la velocidad de oscilación tras cada impacto exitoso.</summary>
    public float incrementoVelocidad = 0.2f;

    [Tooltip("Opcional: Arrastra aquí el RectTransform de la imagen de fondo verde para que el script la mueva sola.")]
    public RectTransform zonaVisualVerde;

    private int aciertosActuales = 0;
    private bool moviendoDerecha = true;
    private bool terminado = false;

    /// <summary>Configuración inicial limpia.</summary>
    void Start()
    {
        barraObjetivo.value = 0f;

        // Si arrancamos directamente en modo aleatorio, calculamos la primera posición
        if (usarZonaAleatoria)
        {
            CalcularZonaAleatoria();
        }
    }

    /// <summary>Método llamado por el cartel universal de UI para desbloquear el minijuego.</summary>
    public void IniciarMinijuego()
    {
        juegoIniciado = true;
        if (usarZonaAleatoria)
        {
            CalcularZonaAleatoria();
        }
    }

    void Update()
    {
        // Candado: Corta la ejecución si el nivel terminó o no ha empezado
        if (terminado || !juegoIniciado) return;

        // Gestión del tiempo y comunicación con el HUD global
        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        // Derrota por tiempo agotado
        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
            return;
        }

        // Lógica de oscilación de la barra (Ping-Pong)
        if (moviendoDerecha)
        {
            barraObjetivo.value += velocidad * Time.deltaTime;
            if (barraObjetivo.value >= 1f) moviendoDerecha = false;
        }
        else
        {
            barraObjetivo.value -= velocidad * Time.deltaTime;
            if (barraObjetivo.value <= 0f) moviendoDerecha = true;
        }

        // Captura del Input del usuario
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Validación de rango: ¿Está el indicador dentro de la zona segura?
            if (barraObjetivo.value >= margenMinimo && barraObjetivo.value <= margenMaximo)
            {
                aciertosActuales++;

                // Condición de Victoria: Ha completado el número de impactos exigido
                if (aciertosActuales >= aciertosNecesarios)
                {
                    terminado = true;
                    ControlJuego.instancia.ganarMinijuego();
                }
                else
                {
                    // ACIERTO PARCIAL (Aún faltan impactos para ganar)

                    // LÓGICA DE DIFICULTAD BLINDADA: Aumenta la velocidad, pero NUNCA pasa de 3.5f para que sea jugable
                    velocidad = Mathf.Min(velocidad + incrementoVelocidad, 3.5f);

                    tiempoRestante += 1.5f;           // Bonus de tiempo para poder reaccionar al siguiente golpe

                    if (usarZonaAleatoria)
                    {
                        CalcularZonaAleatoria();      // Teletransporte de la zona segura
                    }
                }
            }
            else
            {
                // Fracaso: Pulsación fuera de rango, derrota inmediata
                terminado = true;
                ControlJuego.instancia.perderMinijuego();
            }
        }
    }

    /// <summary>
    /// Reposiciona algorítmicamente los límites lógicos de acierto y desplaza 
    /// la interfaz gráfica del Canvas para emparejar el comportamiento visual.
    /// </summary>
    void CalcularZonaAleatoria()
    {
        // 1. Freno de seguridad: Evita que el ancho sea un disparate que rompa el juego
        float anchoSeguro = Mathf.Clamp(anchoZonaAcierto, 0.01f, 0.9f);

        // 2. Cálculo lógico blindado entre 0 y 1
        margenMinimo = Random.Range(0f, 1f - anchoSeguro);
        margenMaximo = margenMinimo + anchoSeguro;

        // 3. Automatización UI
        if (zonaVisualVerde != null)
        {
            zonaVisualVerde.anchorMin = new Vector2(margenMinimo, zonaVisualVerde.anchorMin.y);
            zonaVisualVerde.anchorMax = new Vector2(margenMaximo, zonaVisualVerde.anchorMax.y);

            // Forzamos el reseteo de márgenes (Left y Right a 0) en el Inspector
            zonaVisualVerde.offsetMin = new Vector2(0, zonaVisualVerde.offsetMin.y);
            zonaVisualVerde.offsetMax = new Vector2(0, zonaVisualVerde.offsetMax.y);
        }
    }
}