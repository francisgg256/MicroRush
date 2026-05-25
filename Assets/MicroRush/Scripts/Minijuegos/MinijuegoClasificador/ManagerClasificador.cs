using UnityEngine;

/// <summary>
/// Controlador principal (Árbitro) del minijuego de clasificación interactiva (Drag and Drop).
/// Centraliza la lógica de puntuación, evalúa las condiciones de victoria/derrota 
/// y se comunica con el gestor global del sistema.
/// </summary>
public class ManagerClasificador : MonoBehaviour
{
    /// <summary>
    /// Instancia estática única (Singleton) accesible globalmente.
    /// Permite que los múltiples objetos arrastrables notifiquen sus resultados 
    /// sin necesidad de buscar referencias costosas en tiempo de ejecución.
    /// </summary>
    public static ManagerClasificador instancia;

    [Header("Control de Inicio")]
    /// <summary>Candado lógico. Evita que el nivel funcione mientras se lee el cartel de instrucciones.</summary>
    public bool juegoIniciado = false;

    [Header("Configuración")]
    /// <summary>
    /// Cuota de aciertos requerida para superar el nivel.
    /// Parámetro expuesto para facilitar el balanceo empírico de la dificultad.
    /// </summary>
    public int aciertosParaGanar = 5;

    /// <summary>Contador interno que rastrea el progreso actual del usuario.</summary>
    private int aciertosActuales = 0;

    /// <summary>
    /// Método de inicialización temprana.
    /// Asigna la referencia del Singleton en el mismo momento en que el objeto despierta.
    /// </summary>
    private void Awake()
    {
        instancia = this;
    }

    /// <summary>Método llamado por el cartel de UI para desbloquear el minijuego.</summary>
    public void IniciarMinijuego()
    {
        juegoIniciado = true;
        Debug.Log("¡Minijuego de clasificación iniciado!");
    }

    /// <summary>
    /// Registra una clasificación correcta por parte del usuario.
    /// Incrementa el progreso y evalúa dinámicamente si se ha alcanzado la condición de victoria.
    /// </summary>
    public void Acierto()
    {
        // Si el juego no ha empezado, no permitimos contabilizar puntos
        if (!juegoIniciado) return;

        aciertosActuales++;

        // Trazabilidad de progreso para la consola de desarrollo
        Debug.Log("Clasificación exitosa. Progreso actual: " + aciertosActuales + "/" + aciertosParaGanar);

        // Condición de Victoria: El usuario ha clasificado correctamente la cantidad de objetos exigida
        if (aciertosActuales >= aciertosParaGanar)
        {
            // Paso de mensajes seguro al sistema global
            if (ControlJuego.instancia != null)
                ControlJuego.instancia.ganarMinijuego();
        }
    }

    /// <summary>
    /// Registra un error de clasificación. 
    /// En este diseño de nivel (estilo Arcade), un solo fallo desencadena la condición de derrota inmediata.
    /// </summary>
    public void Fallo()
    {
        // Si el juego no ha empezado, ignoramos el fallo
        if (!juegoIniciado) return;

        Debug.Log("Error de clasificación: Objeto depositado en el contenedor incorrecto.");

        // Paso de mensajes seguro al sistema global
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.perderMinijuego();
    }
}