using UnityEngine;

/// <summary>
/// Motor de Generación Procedimental de Contenido (PCG).
/// Instancia obstáculos dinámicamente basándose en la posición relativa del jugador.
/// Implementa algoritmos de control de rachas para garantizar que la generación sea 
/// matemáticamente superable y justa para el usuario.
/// </summary>
public class GeneradorInfinitoTrampas : MonoBehaviour
{
    [Header("Referencias")]

    /// <summary>Prefabricado (Prefab) de la trampa a instanciar.</summary>
    public GameObject trampaPrefab;

    /// <summary>Referencia al Transform del jugador para calcular el punto focal de generación.</summary>
    public Transform jugador;

    /// <summary>Referencia a la meta para delimitar el final de la generación procedimental.</summary>
    public Transform meta;

    [Header("Configuración de Generación")]

    /// <summary>
    /// Distancia de anticipación (Offset). 
    /// Genera los obstáculos fuera de la cámara (Just-in-Time) para evitar que el jugador vea el "pop-in" de los objetos.
    /// </summary>
    public float distanciaDeVision = 20f;

    /// <summary>Zona de seguridad libre de obstáculos antes de alcanzar el objetivo final.</summary>
    public float distanciaSeguraMeta = 10f;

    [Header("Alturas (Eje Y)")]
    /// <summary>Coordenada vertical para posicionar las trampas en el suelo.</summary>
    public float alturaSuelo = -3.5f;

    /// <summary>Coordenada vertical para posicionar las trampas invertidas en el techo.</summary>
    public float alturaTecho = 3.5f;

    [Header("Dificultad")]
    /// <summary>Separación mínima entre trampas para garantizar el espacio de reacción.</summary>
    public float distanciaMinima = 7f;

    /// <summary>Separación máxima para evitar tiempos muertos aburridos en la jugabilidad.</summary>
    public float distanciaMaxima = 12f;

    /// <summary>Coordenada horizontal calculada para la próxima instanciación.</summary>
    private float proximaXParaGenerar;

    /// <summary>Contador del algoritmo de equidad (Fairness) para rastrear elementos consecutivos en el mismo eje.</summary>
    private int rachaMismoLado = 0;

    /// <summary>Memoria de estado para comparar la trampa actual con la anterior.</summary>
    private bool ladoAnteriorEnTecho = false;

    /// <summary>
    /// Inicialización del sistema. 
    /// Establece el primer punto de generación por delante de la posición inicial del jugador.
    /// </summary>
    void Start()
    {
        if (jugador != null)
        {
            proximaXParaGenerar = jugador.position.x + 10f;
        }
    }

    /// <summary>
    /// Bucle principal de evaluación espacial.
    /// Comprueba continuamente si el jugador se ha acercado lo suficiente al umbral de generación 
    /// y si aún no se ha alcanzado la zona de seguridad de la meta.
    /// </summary>
    void Update()
    {
        // Control de Excepciones: Aborta si se pierden las referencias críticas
        if (jugador == null || meta == null) return;

        // Evaluación de ventana de instanciación (Culling espacial invertido)
        if (jugador.position.x + distanciaDeVision > proximaXParaGenerar &&
            proximaXParaGenerar < meta.position.x - distanciaSeguraMeta)
        {
            CrearNuevaTrampa();
        }
    }

    /// <summary>
    /// Algoritmo central de diseño procedimental.
    /// Calcula la posición, evalúa la equidad del diseño, instancia el objeto y lo orienta correctamente.
    /// </summary>
    void CrearNuevaTrampa()
    {
        // 1. Balanceo RNG (Random Number Generation)
        bool ponerEnTecho = Random.value > 0.5f;

        // Prevención de picos de dificultad absurdos: Control de Rachas
        // Si el algoritmo intenta colocar más de 2 trampas en el mismo sitio consecutivo, se fuerza la alternancia.
        if (ponerEnTecho == ladoAnteriorEnTecho)
        {
            rachaMismoLado++;
            if (rachaMismoLado > 2)
            {
                ponerEnTecho = !ponerEnTecho; // Inversión forzada para mantener la jugabilidad
                rachaMismoLado = 1;
            }
        }
        else
        {
            rachaMismoLado = 1;
        }
        ladoAnteriorEnTecho = ponerEnTecho;

        // 2. Traducción de la decisión lógica a coordenadas espaciales
        float posicionY = ponerEnTecho ? alturaTecho : alturaSuelo;
        Vector2 posicionFinal = new Vector2(proximaXParaGenerar, posicionY);

        // 3. Instanciación en la jerarquía del motor
        GameObject nueva = Instantiate(trampaPrefab, posicionFinal, Quaternion.identity);

        // 4. Adaptación visual interactiva: Volteo escalar en el eje Y para que los pinchos/sierras apunten al jugador
        if (ponerEnTecho)
        {
            nueva.transform.localScale = new Vector3(1, -1, 1);
        }

        // 5. Cálculo del siguiente intervalo espacial (Pacing del nivel)
        float salto = Random.Range(distanciaMinima, distanciaMaxima);
        proximaXParaGenerar += salto;
    }
}
