using UnityEngine;
using System.Collections;

/// <summary>
/// Gestor principal del minijuego de la cesta.
/// Controla la cuenta atrás, las condiciones de victoria por recolección y el spawn aleatorio
/// procedimental de frutas y pinchos letales para el Nivel 2.
/// </summary>
public class CestaGameManager : MonoBehaviour
{
    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float tiempoRestante = 7f;

    [Header("Generador Base")]
    public GameObject frutaPrefab;
    public Transform[] puntosGeneracion;
    public float tiempoEntreSpawns = 0.8f;

    [Header("Configuración Dificultad (Nivel 2)")]
    /// <summary>Activa esta casilla únicamente en la escena del Nivel 2.</summary>
    public bool generarPinchos = false;

    /// <summary>Prefab del objeto de los pinchos (debe tener el script ObjetoCayendo con 'esLetal' activado).</summary>
    public GameObject pinchoPrefab;

    /// <summary>Probabilidad de que el objeto generado sea un pincho en lugar de una fruta (0 a 100).</summary>
    [Range(0f, 100f)] public float probabilidadPincho = 30f;

    private int frutasRecogidas = 0;
    private float tiempoSpawn = 0f;
    private bool terminado = false;

    public void IniciarMinijuego()
    {
        juegoIniciado = true;
    }

    void Update()
    {
        if (terminado || !juegoIniciado) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            tiempoSpawn -= Time.deltaTime;

            if (ControlJuego.instancia != null)
                ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

            // Gestión del bucle de generación de objetos
            if (tiempoSpawn <= 0)
            {
                SpawnObjeto();
                tiempoSpawn = tiempoEntreSpawns;
            }
        }
        else
        {
            EvaluarFinPartida();
        }
    }

    /// <summary>
    /// Instancia de forma aleatoria una fruta o un pincho en base a las probabilidades del nivel.
    /// </summary>
    void SpawnObjeto()
    {
        if (puntosGeneracion.Length == 0) return;

        // Seleccionamos un punto de spawn aleatorio del array
        Vector3 posicionSpawn = puntosGeneracion[Random.Range(0, puntosGeneracion.Length)].position;
        GameObject objetoAEstructurar = frutaPrefab;

        // Si el Nivel 2 está activo, calculamos la probabilidad de lanzar un pincho
        if (generarPinchos && pinchoPrefab != null)
        {
            float decisionAleatoria = Random.Range(0f, 100f);
            if (decisionAleatoria <= probabilidadPincho)
            {
                objetoAEstructurar = pinchoPrefab;
            }
        }

        // Instanciamos el objeto seleccionado
        if (objetoAEstructurar != null)
        {
            Instantiate(objetoAEstructurar, posicionSpawn, Quaternion.identity);
        }
    }

    public void SumarFruta()
    {
        if (terminado) return;
        frutasRecogidas++;
    }

    /// <summary>
    /// Detiene el minijuego de forma fulminante si el jugador toca un obstáculo prohibido.
    /// Invocado desde ObjetoCayendo.cs
    /// </summary>
    public void RegistrarDerrotaInmediata()
    {
        if (terminado) return;
        terminado = true;

        Debug.Log("¡Derrota Crítica! Recogiste un pincho.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }

    /// <summary>
    /// Evalúa de forma estándar si el jugador cumplió la cuota mínima de recolección al acabar el tiempo.
    /// </summary>
    void EvaluarFinPartida()
    {
        terminado = true;

        if (frutasRecogidas >= 3)
        {
            if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
        }
        else
        {
            if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
        }
    }
}
