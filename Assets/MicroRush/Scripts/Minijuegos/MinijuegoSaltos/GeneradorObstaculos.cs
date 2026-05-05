using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour
{
    public GameObject obstaculoPrefab;
    public float tiempoEntreObstaculos = 2f;

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
            Instantiate(obstaculoPrefab, transform.position, Quaternion.identity);
            temporizador = tiempoEntreObstaculos;
        }
    }
}