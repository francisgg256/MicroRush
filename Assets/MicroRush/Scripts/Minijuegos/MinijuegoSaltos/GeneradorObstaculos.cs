using UnityEngine;

/// <summary>
/// Clase que instancia obstáculos. En el Nivel 2, reduce el tiempo de espera 
/// basándose en el multiplicador de dificultad global.
/// </summary>
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
        if (MinijuegoSaltos.instancia != null && !MinijuegoSaltos.instancia.juegoIniciado)
            return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            Instantiate(obstaculoPrefab, transform.position, Quaternion.identity);

            // Obtenemos el multiplicador (Si estamos en Nivel 1, siempre será 1. Si es Nivel 2, será mayor)
            float multiplicador = MinijuegoSaltos.instancia != null ? MinijuegoSaltos.instancia.multiplicadorDificultad : 1f;

            // Dividimos el tiempo de espera entre el multiplicador para que aparezcan más rápido
            temporizador = tiempoEntreObstaculos / multiplicador;
        }
    }
}