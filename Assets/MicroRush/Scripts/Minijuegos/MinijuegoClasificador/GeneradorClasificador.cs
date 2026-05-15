using UnityEngine;

public class GeneradorClasificador : MonoBehaviour
{
    [Header("Ajustes")]
    public GameObject[] prefabsObjetos; // Aquí arrastras la Fresa y la Bola de Pinchos
    public float tiempoEntreSpawns = 1.5f;
    public float rangoX = 6f; // Qué tan ancho es el pasillo de caída

    private float cronometro;

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= tiempoEntreSpawns)
        {
            SoltarObjeto();
            cronometro = 0;
        }
    }

    void SoltarObjeto()
    {
        int indice = Random.Range(0, prefabsObjetos.Length);

        // EL CAMBIO ESTÁ AQUÍ: Ahora toma como centro la posición de tu Generador
        float posX = transform.position.x + Random.Range(-rangoX, rangoX);

        Vector3 posicion = new Vector3(posX, transform.position.y, 0);

        Instantiate(prefabsObjetos[indice], posicion, Quaternion.identity);
    }
}
