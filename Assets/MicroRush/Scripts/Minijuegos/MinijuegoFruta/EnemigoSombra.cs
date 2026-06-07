using UnityEngine;

public class EnemigoSombra : MonoBehaviour
{
    [Header("Configuración de Persecución")]
    public float velocidadPersecucion = 3.5f;

    [Header("Comportamiento Inicial")]
    public float tiempoVentaja = 1.5f;

    private Transform jugador;
    private MinijuegoFrutas manager;

    private bool estaPersiguiendo = false;
    private float temporizador = 0f;

    void Start()
    {
        manager = FindFirstObjectByType<MinijuegoFrutas>();

        GameObject objetoJugador = GameObject.FindGameObjectWithTag("Jugador");
        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;
            transform.position = jugador.position;
            Debug.Log("1. Sombra teletransportada con éxito a los pies del jugador.");
        }
        else
        {
            Debug.LogError("ERROR: La sombra no encuentra al jugador. ¿Seguro que tu rana tiene el Tag 'Jugador' arriba del todo?");
        }
    }

    void Update()
    {
        if (manager == null) return;

        if (!manager.juegoIniciado)
        {
            // El juego aún no ha empezado (cartel en pantalla)
            return;
        }

        if (jugador == null) return;

        if (!estaPersiguiendo)
        {
            temporizador += Time.deltaTime;

            if (temporizador >= tiempoVentaja)
            {
                estaPersiguiendo = true;
                Debug.Log("2. ¡Se acabó la ventaja! La sombra arranca.");
            }
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidadPersecucion * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (estaPersiguiendo && collision.CompareTag("Jugador"))
        {
            if (manager != null)
            {
                Debug.Log("3. ¡Te atrapó!");
                manager.PerderPorSombra();
            }
        }
    }
}
