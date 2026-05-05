using UnityEngine;

public class GeneradorSierras : MonoBehaviour
{
    public GameObject obstaculoPrefab;
    public float tiempoEntreObstaculos = 2f;
    public float limiteX = 8f; // He subido un poco esto por defecto

    private float temporizador;

    void Start()
    {
        temporizador = tiempoEntreObstaculos;
    }

    void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            // EL CAMBIO MÁGICO: Ahora toma la posición 'X' de tu Generador como punto central
            float posX = transform.position.x + Random.Range(-limiteX, limiteX);
            Vector3 posicionAleatoria = new Vector3(posX, transform.position.y, 0f);

            // Instanciamos
            Instantiate(obstaculoPrefab, posicionAleatoria, Quaternion.identity);

            temporizador = tiempoEntreObstaculos;
        }
    }
}
