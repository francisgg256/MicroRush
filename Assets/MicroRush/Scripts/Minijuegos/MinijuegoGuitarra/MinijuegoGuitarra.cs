using System.Collections;
using UnityEngine;

/// <summary>
/// Gestor principal del minijuego de ritmo.
/// Controla el generador de notas, el tiempo límite y el Modo Frenético del Nivel 2.
/// </summary>
public class MinijuegoGuitarra : MonoBehaviour
{
    public static MinijuegoGuitarra instancia;

    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float tiempoRestante = 10f;

    [Header("Generador de Notas (Nivel 1)")]
    public GameObject[] prefabsNotas;
    public Transform[] puntosAparicion;
    public float tiempoEntreNotas = 0.7f;

    [Header("Modo Frenético (Nivel 2)")]
    /// <summary>Activa esta casilla para habilitar las notas dobles y la súper velocidad.</summary>
    public bool modoFrenetico = false;

    /// <summary>Tiempo de espera entre oleadas en el Nivel 2.</summary>
    public float tiempoEntreNotasFrenetico = 0.4f;

    /// <summary>Probabilidad (0-100) de que caigan dos notas al mismo tiempo.</summary>
    [Range(0f, 100f)] public float probabilidadNotaDoble = 35f;

    /// <summary>Multiplicador de la velocidad de caída de las notas.</summary>
    public float multiplicadorVelocidadNotas = 1.6f;

    private bool terminado = false;

    void Awake()
    {
        instancia = this;
    }

    public void IniciarMinijuego()
    {
        juegoIniciado = true;
        StartCoroutine(GenerarNotasAleatorias());
    }

    void Update()
    {
        if (terminado || !juegoIniciado) return;

        tiempoRestante -= Time.deltaTime;

        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        if (tiempoRestante <= 0)
        {
            terminado = true;
            if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
        }
    }

    IEnumerator GenerarNotasAleatorias()
    {
        // Elegimos el ritmo base según el nivel
        float ritmoActual = modoFrenetico ? tiempoEntreNotasFrenetico : tiempoEntreNotas;

        while (juegoIniciado && !terminado)
        {
            // 1. Generamos la primera nota obligatoria
            int indice1 = Random.Range(0, prefabsNotas.Length);
            LanzarNota(indice1);

            // 2. Si estamos en Nivel 2, tiramos los dados para ver si sale una nota doble
            if (modoFrenetico && Random.Range(0f, 100f) <= probabilidadNotaDoble)
            {
                int indice2 = Random.Range(0, prefabsNotas.Length);

                // Bucle de seguridad: Evita que la segunda nota caiga exactamente en el mismo carril que la primera
                while (indice2 == indice1)
                {
                    indice2 = Random.Range(0, prefabsNotas.Length);
                }

                LanzarNota(indice2);
            }

            // Ritmo dinámico para darle "swing"
            float tiempoEspera = Random.Range(ritmoActual * 0.8f, ritmoActual * 1.2f);
            yield return new WaitForSeconds(tiempoEspera);
        }
    }

    /// <summary>Instancia una nota y le aplica la velocidad extra si estamos en Nivel 2.</summary>
    void LanzarNota(int indice)
    {
        GameObject nuevaNotaObj = Instantiate(prefabsNotas[indice], puntosAparicion[indice].position, Quaternion.identity);

        if (modoFrenetico)
        {
            NotaGuitarra scriptNota = nuevaNotaObj.GetComponent<NotaGuitarra>();
            if (scriptNota != null)
            {
                scriptNota.AcelerarNota(multiplicadorVelocidadNotas);
            }
        }
    }

    // --- MÉTODOS DE DERROTA ---
    public void PerderPorNotaPerdida()
    {
        if (terminado) return;
        terminado = true;
        Debug.Log("¡Se te escapó una nota! Has perdido.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }

    public void PerderPorFalloInput()
    {
        if (terminado) return;
        terminado = true;
        Debug.Log("¡Pulsaste cuando no debías! Has perdido.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
