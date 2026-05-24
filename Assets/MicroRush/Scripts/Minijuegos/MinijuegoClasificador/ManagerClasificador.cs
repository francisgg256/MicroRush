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

    /// <summary>
    /// Registra una clasificación correcta por parte del usuario.
    /// Incrementa el progreso y evalúa dinámicamente si se ha alcanzado la condición de victoria.
    /// </summary>
    public void Acierto()
    {
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
        Debug.Log("Error de clasificación: Objeto depositado en el contenedor incorrecto.");

        // Paso de mensajes seguro al sistema global
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.perderMinijuego();
    }
}
