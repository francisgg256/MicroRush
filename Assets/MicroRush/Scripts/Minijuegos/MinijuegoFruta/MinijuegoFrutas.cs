using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controlador principal de la lógica del minijuego de recolección de frutas.
/// Calcula dinámicamente las condiciones de victoria basándose en el diseño del nivel
/// y gestiona el progreso del jugador implementando estructuras de datos seguras.
/// </summary>
public class MinijuegoFrutas : MonoBehaviour
{
    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float tiempoRestante = 7f;

    private int frutasTotales = 0;
    private int frutasRecogidas = 0;
    private bool terminado = false;

    private HashSet<GameObject> frutasContadas = new HashSet<GameObject>();

    void Start()
    {
        GameObject[] todasLasFrutas = GameObject.FindGameObjectsWithTag("Frutas");
        frutasTotales = todasLasFrutas.Length;
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

        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }

    public void FrutaRecogida(GameObject frutaObjeto)
    {
        if (terminado || !juegoIniciado) return;

        if (!frutasContadas.Contains(frutaObjeto))
        {
            frutasContadas.Add(frutaObjeto);
            frutasRecogidas++;

            if (frutasRecogidas >= frutasTotales)
            {
                terminado = true;
                ControlJuego.instancia.ganarMinijuego();
            }
        }
    }

    /// <summary>
    /// NUEVO: Evento invocado externamente por la Sombra cuando atrapa al jugador en el Nivel 2.
    /// </summary>
    public void PerderPorSombra()
    {
        if (terminado) return;
        terminado = true;

        Debug.Log("¡La sombra te ha atrapado!");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
