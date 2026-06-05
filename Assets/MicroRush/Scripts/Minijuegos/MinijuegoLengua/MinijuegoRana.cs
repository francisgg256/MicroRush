using System.Collections;
using UnityEngine;

/// <summary>
/// Gestor principal del minijuego de la rana.
/// Genera objetos procedimentales desde el techo y gestiona la condición de victoria.
/// </summary>
public class MinijuegoRana : MonoBehaviour
{
    public static MinijuegoRana instancia;

    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float tiempoRestante = 10f;
    public int frutasParaGanar = 5;

    [Header("Generador de Objetos")]
    public GameObject prefabFruta;
    public GameObject prefabPincho;
    public float tiempoEntreApariciones = 1.2f;

    [Tooltip("Límites X (Izquierda y Derecha) donde pueden aparecer los objetos")]
    public float limiteXIzquierda = -7f;
    public float limiteXDerecha = 7f;
    public float alturaAparicion = 6f;

    private int frutasComidas = 0;
    private bool terminado = false;

    void Awake()
    {
        instancia = this; // Singleton para que la lengua lo encuentre fácil
    }

    public void IniciarMinijuego()
    {
        juegoIniciado = true;
        // Arranca la máquina de hacer llover frutas
        StartCoroutine(GenerarObjetos());
    }

    void Update()
    {
        if (terminado || !juegoIniciado) return;

        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }

    IEnumerator GenerarObjetos()
    {
        while (juegoIniciado && !terminado)
        {
            // Decidimos aleatoriamente si cae fruta (70% prob) o pincho (30% prob)
            GameObject objetoASpawnear = Random.Range(0f, 100f) < 70f ? prefabFruta : prefabPincho;

            // Calculamos una posición aleatoria en el techo
            float posicionX = Random.Range(limiteXIzquierda, limiteXDerecha);
            Vector3 posicionGeneracion = new Vector3(posicionX, alturaAparicion, 0f);

            // Creamos el objeto
            Instantiate(objetoASpawnear, posicionGeneracion, Quaternion.identity);

            yield return new WaitForSeconds(tiempoEntreApariciones);
        }
    }

    // --- FUNCIONES LLAMADAS POR LA LENGUA ---

    public void SumarFruta()
    {
        if (terminado) return;

        frutasComidas++;
        if (frutasComidas >= frutasParaGanar)
        {
            terminado = true;
            ControlJuego.instancia.ganarMinijuego();
        }
    }

    public void TocarPincho()
    {
        if (terminado) return;

        terminado = true;
        // El pincho mata al instante
        ControlJuego.instancia.perderMinijuego();
    }
}
