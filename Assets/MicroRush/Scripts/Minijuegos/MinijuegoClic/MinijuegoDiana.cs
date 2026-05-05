using UnityEngine;

public class MinijuegoDiana : MonoBehaviour
{
    public static MinijuegoDiana instancia;

    [Header("Configuración del Minijuego")]
    public float duracion = 10f; // Tienes 6 segundos para hacerlo
    public int frutasNecesarias = 5; // Cuántas frutas hay que pinchar para ganar

    private float tiempoRestante;
    private int frutasExplotadas = 0;
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

        // Si el tiempo llega a 0 y NO has explotado las frutas necesarias... pierdes.
        if (tiempoRestante <= 0)
        {
            perder();
        }
    }

    // Esta función la llamará la fruta cuando le hagas clic
    public void SumarAcierto()
    {
        if (terminado) return;

        frutasExplotadas++;
        Debug.Log("¡Fruta explotada! Llevas: " + frutasExplotadas + " de " + frutasNecesarias);

        // Si llegamos al objetivo antes de que acabe el tiempo... ¡Ganamos!
        if (frutasExplotadas >= frutasNecesarias)
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
        if (terminado) return;

        terminado = true;
        ControlJuego.instancia.ganarMinijuego();
    }
}
