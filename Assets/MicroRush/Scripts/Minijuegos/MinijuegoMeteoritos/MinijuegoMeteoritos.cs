using UnityEngine;

public class MinijuegoMeteoritos : MonoBehaviour
{
    // Ahora la instancia se llama MinijuegoMeteoritos
    public static MinijuegoMeteoritos instancia;

    public float duracion = 10f;
    public float tiempoRestante;

    private bool terminado = false;

    private void Awake()
    {
        instancia = this;

        if (ControlJuego.instancia == null)
        {
            Debug.LogError("No hay ControlJuego en la escena");
        }
    }

    void Start()
    {
        tiempoRestante = duracion;
    }

    void Update()
    {
        if (terminado) return;

        tiempoRestante -= Time.deltaTime;

        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;
        }

        if (tiempoRestante <= 0)
        {
            terminarVictoria();
        }
    }

    public void perder()
    {
        if (terminado) return;

        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }

    private void terminarVictoria()
    {
        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }
}
