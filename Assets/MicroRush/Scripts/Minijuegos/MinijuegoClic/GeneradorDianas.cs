using UnityEngine;

/// <summary>
/// Clase responsable de la instanciación dinámica y procedimental de objetivos en pantalla.
/// Gestiona la aparición aleatoria de elementos interactivos (positivos y negativos) 
/// dentro de los límites visuales de la interfaz del usuario.
/// </summary>
public class GeneradorDianas : MonoBehaviour
{
    [Header("Prefabs")]

    /// <summary>
    /// Objeto base (Prefab) que representa un objetivo válido (ej. fruta).
    /// </summary>
    public GameObject prefabFruta;

    /// <summary>
    /// Objeto base (Prefab) que representa una penalización o peligro letal (ej. bomba o pinchos).
    /// </summary>
    public GameObject prefabPeligro;

    [Header("Configuración")]

    /// <summary>Límite máximo de coordenadas en el eje horizontal para asegurar que el objeto sea visible en pantalla.</summary>
    public float limiteX = 7f;

    /// <summary>Límite máximo de coordenadas en el eje vertical para asegurar que el objeto sea visible en pantalla.</summary>
    public float limiteY = 4f;

    /// <summary>Intervalo de tiempo en segundos entre la aparición de un nuevo elemento y el siguiente.</summary>
    public float tiempoEntreApariciones = 0.8f;

    /// <summary>
    /// Tiempo de vida útil del objeto en segundos. 
    /// Si el usuario no interactúa con él en este periodo, el objeto se destruye automáticamente.
    /// </summary>
    public float tiempoVidaObjeto = 2f;

    /// <summary>Temporizador interno para controlar cuándo generar el siguiente objeto.</summary>
    private float cronometro = 0f;

    /// <summary>
    /// Bucle de actualización principal.
    /// Gestiona la instanciación respetando el bloqueo del cartel de instrucciones.
    /// </summary>
    void Update()
    {
        // 1. Candado: Si el juego no ha iniciado, abortamos la ejecución de este fotograma
        if (MinijuegoDiana.instancia != null && !MinijuegoDiana.instancia.juegoIniciado)
            return;

        // Acumulamos el tiempo
        cronometro += Time.deltaTime;

        // Si superamos el umbral, generamos un objeto y reiniciamos el reloj
        if (cronometro >= tiempoEntreApariciones)
        {
            AparecerObjeto();
            cronometro = 0f;
        }
    }

    /// <summary>
    /// Lógica central de instanciación procedimental.
    /// Calcula probabilidades paramétricas, selecciona el tipo de objeto, determina 
    /// una posición espacial válida y gestiona su ciclo de vida automático.
    /// </summary>
    void AparecerObjeto()
    {
        // 1. Balanceo de Dificultad (RNG): 
        // Genera un valor aleatorio entre 0.0 y 1.0. Si es mayor a 0.75, selecciona el peligro (25% de probabilidad).
        bool salePeligro = Random.value > 0.75f;

        // Operador ternario para asignar la referencia correcta basándose en el cálculo anterior
        GameObject prefabElegido = salePeligro ? prefabPeligro : prefabFruta;

        // 2. Distribución Espacial: Calcula coordenadas aleatorias dentro de los límites de la interfaz (Viewport)
        float posX = Random.Range(-limiteX, limiteX);
        float posY = Random.Range(-limiteY, limiteY);
        Vector3 posicionAleatoria = new Vector3(posX, posY, 0f);

        // 3. Instanciación: Crea el objeto en el mundo 2D sin alterar su rotación nativa
        GameObject nuevoObjeto = Instantiate(prefabElegido, posicionAleatoria, Quaternion.identity);

        // 4. Optimización (Garbage Collection): 
        // Programa la destrucción del objeto instanciado tras expirar su tiempo de vida,
        // evitando fugas de memoria si el jugador ignora los objetivos.
        Destroy(nuevoObjeto, tiempoVidaObjeto);
    }
}
