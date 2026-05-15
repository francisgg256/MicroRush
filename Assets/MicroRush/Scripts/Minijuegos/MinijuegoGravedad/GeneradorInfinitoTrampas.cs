using UnityEngine;

public class GeneradorInfinitoTrampas : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject trampaPrefab;
    public Transform jugador; // Para saber por dónde va
    public Transform meta;    // Para saber cuándo parar

    [Header("Configuración de Generación")]
    public float distanciaDeVision = 20f; // A qué distancia por delante del jugador empezamos a crear
    public float distanciaSeguraMeta = 10f; // Margen antes del trofeo

    [Header("Alturas (Eje Y)")]
    public float alturaSuelo = -3.5f;
    public float alturaTecho = 3.5f;

    [Header("Dificultad")]
    public float distanciaMinima = 7f;
    public float distanciaMaxima = 12f;

    private float proximaXParaGenerar;
    private int rachaMismoLado = 0;
    private bool ladoAnteriorEnTecho = false;

    void Start()
    {
        // Empezamos a generar un poco por delante del jugador
        if (jugador != null)
        {
            proximaXParaGenerar = jugador.position.x + 10f;
        }
    }

    void Update()
    {
        if (jugador == null || meta == null) return;

        // ¿El jugador se está acercando al punto donde toca poner la siguiente sierra?
        // Y... ¿Todavía estamos lejos de la meta?
        if (jugador.position.x + distanciaDeVision > proximaXParaGenerar &&
            proximaXParaGenerar < meta.position.x - distanciaSeguraMeta)
        {
            CrearNuevaTrampa();
        }
    }

    void CrearNuevaTrampa()
    {
        // 1. Decidimos el lado (Techo o Suelo) con control de racha
        bool ponerEnTecho = Random.value > 0.5f;

        if (ponerEnTecho == ladoAnteriorEnTecho)
        {
            rachaMismoLado++;
            if (rachaMismoLado > 2)
            {
                ponerEnTecho = !ponerEnTecho;
                rachaMismoLado = 1;
            }
        }
        else
        {
            rachaMismoLado = 1;
        }
        ladoAnteriorEnTecho = ponerEnTecho;

        // 2. Calculamos posición
        float posicionY = ponerEnTecho ? alturaTecho : alturaSuelo;
        Vector2 posicionFinal = new Vector2(proximaXParaGenerar, posicionY);

        // 3. Instanciar
        GameObject nueva = Instantiate(trampaPrefab, posicionFinal, Quaternion.identity);

        // 4. Si es techo, le damos la vuelta
        if (ponerEnTecho)
        {
            nueva.transform.localScale = new Vector3(1, -1, 1);
        }

        // 5. Calculamos cuándo será la siguiente (el salto)
        float salto = Random.Range(distanciaMinima, distanciaMaxima);
        proximaXParaGenerar += salto;
    }
}
