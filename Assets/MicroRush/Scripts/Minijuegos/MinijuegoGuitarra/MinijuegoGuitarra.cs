using System.Collections;
using UnityEngine;

/// <summary>
/// Gestor principal del minijuego de ritmo.
/// Controla el generador de notas, el tiempo límite y el Modo Frenético balanceado.
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
    public bool modoFrenetico = false;

    // Aumentado un poco para dar margen de lectura (antes 0.4)
    public float tiempoEntreNotasFrenetico = 0.5f;

    [Range(0f, 100f)] public float probabilidadNotaDoble = 35f;

    // Bajado de 1.6 a 1.35 para que el jugador tenga tiempo físico de reaccionar
    public float multiplicadorVelocidadNotas = 1.35f;

    private bool terminado = false;

    // Variable para el Sistema Anti-Spam
    private bool dobleNotaAnterior = false;

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
        float ritmoActual = modoFrenetico ? tiempoEntreNotasFrenetico : tiempoEntreNotas;

        while (juegoIniciado && !terminado)
        {
            // 1. Generamos la primera nota obligatoria
            int indice1 = Random.Range(0, prefabsNotas.Length);
            LanzarNota(indice1);

            float tiempoEspera = Random.Range(ritmoActual * 0.8f, ritmoActual * 1.2f);

            // --- 2. SISTEMA ANTI-SPAM DE NOTAS DOBLES ---
            // Solo tira los dados si NO salió una nota doble justo antes
            if (modoFrenetico && !dobleNotaAnterior && Random.Range(0f, 100f) <= probabilidadNotaDoble)
            {
                int indice2 = Random.Range(0, prefabsNotas.Length);

                // Evita que la segunda nota caiga en el mismo carril
                while (indice2 == indice1)
                {
                    indice2 = Random.Range(0, prefabsNotas.Length);
                }

                LanzarNota(indice2);
                dobleNotaAnterior = true; // Marcamos que acaba de salir una doble

                // Le damos una "micro-pausa" de respiro al jugador tras exigirle doble reflejo
                tiempoEspera *= 1.4f;
            }
            else
            {
                // Si esta vez salió simple, reseteamos el seguro para que en la próxima pueda salir doble
                dobleNotaAnterior = false;
            }

            yield return new WaitForSeconds(tiempoEspera);
        }
    }

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
