using System.Collections;
using UnityEngine;

/// <summary>
/// Gestor principal del minijuego de ritmo.
/// Controla el generador de notas, el tiempo límite y las condiciones de victoria/derrota.
/// </summary>
public class MinijuegoGuitarra : MonoBehaviour
{
    public static MinijuegoGuitarra instancia;

    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float tiempoRestante = 15f; // Lo que dura el minijuego

    [Header("Generador de Notas")]
    public GameObject[] prefabsNotas;   // Mete aquí tus 4 prefabs (A, S, W, D)
    public Transform[] puntosAparicion; // Mete aquí tus 4 Spawns (los objetos vacíos de arriba)
    public float tiempoEntreNotas = 1f;

    private bool terminado = false;

    void Awake()
    {
        instancia = this; // Singleton para que las notas lo encuentren fácilmente
    }

    /// <summary>Llamado por tu cartel de "¡ACCION!"</summary>
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

        // Si aguantas todo el tiempo sin fallar, ¡ganas!
        if (tiempoRestante <= 0)
        {
            terminado = true;
            if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
        }
    }

    IEnumerator GenerarNotasAleatorias()
    {
        while (juegoIniciado && !terminado)
        {
            // Elegimos una pista al azar (del 0 al 3)
            int indiceAleatorio = Random.Range(0, prefabsNotas.Length);

            // Creamos la nota en su punto de Spawn correspondiente
            Instantiate(prefabsNotas[indiceAleatorio], puntosAparicion[indiceAleatorio].position, Quaternion.identity);

            // Ritmo dinámico: espera un poco antes de lanzar la siguiente nota
            float tiempoEspera = Random.Range(tiempoEntreNotas * 0.8f, tiempoEntreNotas * 1.2f);
            yield return new WaitForSeconds(tiempoEspera);
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
