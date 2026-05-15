using UnityEngine;

public class MinijuegoSemaforo : MonoBehaviour
{
    public static MinijuegoSemaforo instancia;

    [Header("Configuración")]
    public float duracion = 10f;
    public SpriteRenderer luzSemaforo;

    // ESTADOS: 0 = Verde, 1 = Amarillo, 2 = Rojo
    [HideInInspector] public int estadoSemaforo = 0;

    private float tiempoRestante;
    private float temporizadorCambio;
    private bool terminado = false;

    private void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        tiempoRestante = duracion;
        CambiarLuz(0); // Empezamos siempre en verde
        temporizadorCambio = Random.Range(1.5f, 3.5f); // Tiempo inicial del verde
    }

    void Update()
    {
        if (terminado) return;

        // 1. Lógica del tiempo general
        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null) ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        if (tiempoRestante <= 0)
        {
            Perder();
        }

        // 2. Lógica de cambiar el color de la luz
        temporizadorCambio -= Time.deltaTime;
        if (temporizadorCambio <= 0)
        {
            AvanzarSemaforo();
        }
    }

    void AvanzarSemaforo()
    {
        if (estadoSemaforo == 0) // Si está VERDE -> Pasa a AMARILLO
        {
            CambiarLuz(1);
            temporizadorCambio = 0.8f; // El amarillo dura menos de 1 segundo (es solo un aviso)
        }
        else if (estadoSemaforo == 1) // Si está AMARILLO -> Pasa a ROJO
        {
            CambiarLuz(2);
            temporizadorCambio = Random.Range(1f, 2f); // El rojo dura entre 1 y 2 segundos
        }
        else if (estadoSemaforo == 2) // Si está ROJO -> Vuelve a VERDE
        {
            CambiarLuz(0);
            temporizadorCambio = Random.Range(1.5f, 3.5f); // El verde dura entre 1.5 y 3.5 segundos
        }
    }

    void CambiarLuz(int nuevoEstado)
    {
        estadoSemaforo = nuevoEstado;

        // Pintamos el círculo del color que toque
        if (estadoSemaforo == 0) luzSemaforo.color = Color.green;
        else if (estadoSemaforo == 1) luzSemaforo.color = Color.yellow;
        else if (estadoSemaforo == 2) luzSemaforo.color = Color.red;
    }

    public void Ganar()
    {
        if (terminado) return;
        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }

    public void Perder()
    {
        if (terminado) return;
        terminado = true;
        ControlJuego.instancia.perderMinijuego();
    }
}
