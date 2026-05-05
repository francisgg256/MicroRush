using UnityEngine;

public class GeneradorDianas : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabFruta;
    public GameObject prefabPeligro;

    [Header("Configuración")]
    public float limiteX = 7f; // Límite horizontal de la pantalla
    public float limiteY = 4f; // Límite vertical de la pantalla
    public float tiempoEntreApariciones = 0.8f; // Cada cuánto sale un objeto
    public float tiempoVidaObjeto = 2f; // Cuánto tarda en desaparecer si no le das clic

    void Start()
    {
        // Empieza a escupir objetos en bucle
        InvokeRepeating("AparecerObjeto", 0.5f, tiempoEntreApariciones);
    }

    void AparecerObjeto()
    {
        // 1. Decidimos al azar si sale una fruta o una bola de pinchos (ej: 25% de que sea pinchos)
        bool salePeligro = Random.value > 0.75f;
        GameObject prefabElegido = salePeligro ? prefabPeligro : prefabFruta;

        // 2. Buscamos una posición aleatoria en toda la pantalla
        float posX = Random.Range(-limiteX, limiteX);
        float posY = Random.Range(-limiteY, limiteY);
        Vector3 posicionAleatoria = new Vector3(posX, posY, 0f);

        // 3. Hacemos aparecer el objeto
        GameObject nuevoObjeto = Instantiate(prefabElegido, posicionAleatoria, Quaternion.identity);

        // 4. Si el jugador no le hace clic a tiempo, el objeto se destruye solo
        Destroy(nuevoObjeto, tiempoVidaObjeto);
    }
}
