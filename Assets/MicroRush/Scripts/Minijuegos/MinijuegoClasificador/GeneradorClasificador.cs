using UnityEngine;

/// <summary>
/// Controlador encargado de la instanciación procedimental de objetos en el minijuego de clasificación.
/// Funciona como un emisor continuo (Spawner) que nutre la mecánica interactiva de "Drag and Drop".
/// </summary>
public class GeneradorClasificador : MonoBehaviour
{
    [Header("Ajustes")]

    /// <summary>
    /// Colección (Array) de elementos prefabricados disponibles para instanciar.
    /// Aporta escalabilidad: permite añadir nuevos tipos de frutas o peligros desde el Inspector sin alterar el código.
    /// </summary>
    public GameObject[] prefabsObjetos;

    /// <summary>
    /// Intervalo de tiempo en segundos entre la aparición de cada objeto.
    /// Define el ritmo de juego (Pacing) y la carga cognitiva a la que se somete al usuario.
    /// </summary>
    public float tiempoEntreSpawns = 1.5f;

    /// <summary>
    /// Rango máximo de desviación horizontal respecto al centro del generador.
    /// Define la amplitud de la zona de aparición (ej. 6f permite instanciar entre -6 y +6 unidades).
    /// </summary>
    public float rangoX = 6f;

    /// <summary>Variable interna utilizada como acumulador de tiempo real para el ciclo de instanciación.</summary>
    private float cronometro;

    /// <summary>
    /// Bucle lógico principal.
    /// Gestiona la sincronización temporal del emisor de forma independiente a la tasa de fotogramas (FPS).
    /// </summary>
    void Update()
    {
        // 1. Candado: Si el juego no ha iniciado, abortamos la ejecución de este fotograma
        if (ManagerClasificador.instancia != null && !ManagerClasificador.instancia.juegoIniciado)
            return;

        // Acumulación del tiempo transcurrido desde el último fotograma
        cronometro += Time.deltaTime;

        // Validación del umbral temporal
        if (cronometro >= tiempoEntreSpawns)
        {
            SoltarObjeto();
            // Reinicio del ciclo
            cronometro = 0;
        }
    }

    /// <summary>
    /// Lógica central de instanciación procedimental.
    /// Selecciona un objeto aleatorio de la colección y calcula una posición espacial dinámica.
    /// </summary>
    void SoltarObjeto()
    {
        // 1. Selección Aleatoria Uniforme (RNG): Escoge un índice válido dentro de la longitud actual del array
        int indice = Random.Range(0, prefabsObjetos.Length);

        // 2. Aleatoriedad Espacial Relativa: 
        // Calcula la coordenada X tomando como anclaje la posición actual de este generador,
        // lo que permite mover el emisor libremente en el editor sin romper la lógica del script.
        float posX = transform.position.x + Random.Range(-rangoX, rangoX);

        // 3. Formación del vector espacial bidimensional (dejando Z en 0 por defecto)
        Vector3 posicion = new Vector3(posX, transform.position.y, 0);

        // 4. Creación del objeto en la jerarquía del motor, manteniendo su rotación original
        Instantiate(prefabsObjetos[indice], posicion, Quaternion.identity);
    }
}
