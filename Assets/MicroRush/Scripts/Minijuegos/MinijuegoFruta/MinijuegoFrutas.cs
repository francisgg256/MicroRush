using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controlador principal de la lógica del minijuego de recolección de frutas.
/// Calcula dinámicamente las condiciones de victoria basándose en el diseño del nivel
/// y gestiona el progreso del jugador implementando estructuras de datos seguras.
/// </summary>
public class MinijuegoFrutas : MonoBehaviour
{
    /// <summary>Tiempo límite en segundos para recolectar todas las frutas del escenario.</summary>
    public float tiempoRestante = 10f;

    /// <summary>Cantidad total de frutas presentes en el nivel al inicio de la partida.</summary>
    private int frutasTotales = 0;

    /// <summary>Contador del progreso actual del jugador.</summary>
    private int frutasRecogidas = 0;

    /// <summary>Bandera de control para detener la lógica una vez finalizado el minijuego.</summary>
    private bool terminado = false;

    /// <summary>
    /// Estructura de datos optimizada (Conjunto Hash) que almacena referencias únicas.
    /// Prevención de errores: Garantiza matemáticamente que una misma fruta no pueda ser 
    /// contada dos veces, incluso si ocurren múltiples eventos de colisión simultáneos.
    /// </summary>
    private HashSet<GameObject> frutasContadas = new HashSet<GameObject>();

    /// <summary>
    /// Método de inicialización. 
    /// Escanea la escena en busca de todos los objetos interactuables mediante su etiqueta (Tag).
    /// Esto hace que el código sea escalable, adaptándose automáticamente a cualquier cambio en el diseño del nivel.
    /// </summary>
    void Start()
    {
        GameObject[] todasLasFrutas = GameObject.FindGameObjectsWithTag("Frutas");
        frutasTotales = todasLasFrutas.Length;
    }

    /// <summary>
    /// Bucle principal del minijuego.
    /// Gestiona la cuenta regresiva, la actualización del HUD global y evalúa la condición de derrota por tiempo.
    /// </summary>
    void Update()
    {
        if (terminado) return;

        tiempoRestante -= Time.deltaTime;

        // Paso de mensajes: Sincronización del temporizador visual en la interfaz del usuario
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        // Condición de Derrota: El tiempo se agota antes de recolectar todos los objetivos
        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }

    /// <summary>
    /// Evento invocado externamente por los objetos "Fruta" al detectar una colisión con el jugador.
    /// </summary>
    /// <param name="frutaObjeto">Referencia al GameObject específico que acaba de ser recolectado.</param>
    public void FrutaRecogida(GameObject frutaObjeto)
    {
        if (terminado) return;

        // Control de Integridad de Datos: Verifica que este objeto exacto no haya sido procesado previamente
        if (!frutasContadas.Contains(frutaObjeto))
        {
            frutasContadas.Add(frutaObjeto);
            frutasRecogidas++;

            // Condición de Victoria: El progreso del jugador iguala al total de objetos detectados inicialmente
            if (frutasRecogidas >= frutasTotales)
            {
                terminado = true;
                ControlJuego.instancia.ganarMinijuego();
            }
        }
    }
}
