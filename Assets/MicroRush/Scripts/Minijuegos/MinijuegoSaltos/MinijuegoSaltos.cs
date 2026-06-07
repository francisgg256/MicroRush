using UnityEngine;

/// <summary>
/// Gestor principal del minijuego de saltos.
/// Ahora incluye la lógica del Nivel 2 para aumentar la dificultad progresivamente.
/// </summary>
public class MinijuegoSaltos : MonoBehaviour
{
    public static MinijuegoSaltos instancia;

    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float duracion = 7f;
    public float tiempoRestante;

    [Header("Modo Acelerado (Nivel 2)")]
    /// <summary>Activa esta casilla para que la velocidad aumente con el tiempo.</summary>
    public bool modoAcelerado = false;

    /// <summary>Cuánto se multiplica la velocidad al llegar al final del minijuego.</summary>
    public float multiplicadorMaximo = 2.5f;

    /// <summary>Variable pública leída por los obstáculos para saber a qué velocidad ir.</summary>
    [HideInInspector] public float multiplicadorDificultad = 1f;

    private bool terminado = false;

    private void Awake()
    {
        instancia = this;
        if (ControlJuego.instancia == null)
        {
            Debug.LogError("Error Crítico: No hay instancia de ControlJuego en la escena.");
        }
    }

    void Start()
    {
        tiempoRestante = duracion;
        multiplicadorDificultad = 1f; // Empezamos a velocidad normal (x1)
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
        {
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;
        }

        // --- MAGIA DEL NIVEL 2 ---
        if (modoAcelerado)
        {
            // Calculamos qué porcentaje del tiempo total ha pasado (de 0.0 a 1.0)
            float progreso = 1f - (tiempoRestante / duracion);

            // Subimos el multiplicador suavemente desde 1 hasta el máximo establecido
            multiplicadorDificultad = Mathf.Lerp(1f, multiplicadorMaximo, progreso);
        }

        if (tiempoRestante <= 0)
        {
            terminarVictoria();
        }
    }

    public void perder()
    {
        if (terminado || !juegoIniciado) return;

        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }

    private void terminarVictoria()
    {
        if (terminado) return;

        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }
}