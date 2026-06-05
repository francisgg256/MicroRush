using UnityEngine;

/// <summary>
/// Motor de Generación Procedimental de Contenido (PCG) Infinito.
/// Instancia obstáculos dinámicamente delante del jugador sin límite de distancia.
/// </summary>
public class GeneradorInfinitoTrampas : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject trampaPrefab;
    public Transform jugador;

    [Header("Configuración de Generación")]
    public float distanciaDeVision = 20f;

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
        if (jugador != null)
        {
            proximaXParaGenerar = jugador.position.x + 10f;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Evaluación infinita: Mientras el jugador avance, creamos trampas.
        // Hemos quitado la comprobación de la meta, ahora es realmente infinito.
        if (jugador.position.x + distanciaDeVision > proximaXParaGenerar)
        {
            CrearNuevaTrampa();
        }
    }

    void CrearNuevaTrampa()
    {
        bool ponerEnTecho = Random.value > 0.5f;

        // Control de rachas para que no sea injusto
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

        float posicionY = ponerEnTecho ? alturaTecho : alturaSuelo;
        Vector2 posicionFinal = new Vector2(proximaXParaGenerar, posicionY);

        GameObject nueva = Instantiate(trampaPrefab, posicionFinal, Quaternion.identity);

        if (ponerEnTecho)
        {
            nueva.transform.localScale = new Vector3(1, -1, 1);
        }

        float salto = Random.Range(distanciaMinima, distanciaMaxima);
        proximaXParaGenerar += salto;
    }
}
