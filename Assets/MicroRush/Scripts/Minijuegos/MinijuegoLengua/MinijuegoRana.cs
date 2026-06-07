using System.Collections;
using UnityEngine;

/// <summary>
/// Gestor principal del minijuego de la rana.
/// Controla la lluvia clásica (Nivel 1) y el Modo Reflejos Extremos Anti-Camperos (Nivel 2).
/// </summary>
public class MinijuegoRana : MonoBehaviour
{
    public static MinijuegoRana instancia;

    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    public float tiempoRestante = 5f;
    public int frutasParaGanar = 3;

    [Header("Generador (Nivel 1)")]
    public GameObject prefabFruta;
    public GameObject prefabPincho;
    public float tiempoEntreApariciones = 0.7f;

    /// <summary>Porcentaje (0-100) de que caiga un pincho en lugar de una fruta.</summary>
    public float probabilidadPincho = 30f;

    [Header("Modo Reflejos (Nivel 2)")]
    /// <summary>Activa esta casilla para moscas veloces y pinchos anti-camperos.</summary>
    public bool modoReflejos = false;

    /// <summary>Las frutas y pinchos caen mucho más seguido.</summary>
    public float tiempoAparicionNivel2 = 0.35f;

    /// <summary>Multiplicador que hace que los objetos caigan el doble de rápido.</summary>
    public float multiplicadorVelocidadNivel2 = 2f;

    /// <summary>Multiplicador de tamaño. 0.5 las hace a la mitad de su tamaño (moscas).</summary>
    public float escalaObjetosNivel2 = 0.5f;

    [Tooltip("Límites X (Izquierda y Derecha) donde pueden aparecer los objetos")]
    public float limiteXIzquierda = -7f;
    public float limiteXDerecha = 7f;
    public float alturaAparicion = 6f;

    private int frutasComidas = 0;
    private bool terminado = false;

    void Awake()
    {
        instancia = this;
    }

    public void IniciarMinijuego()
    {
        juegoIniciado = true;
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
            // Decidimos qué prefab usar basándonos en la probabilidad
            GameObject prefabElegido = Random.Range(0f, 100f) < probabilidadPincho ? prefabPincho : prefabFruta;

            // Calculamos posición
            float posicionX = Random.Range(limiteXIzquierda, limiteXDerecha);

            // Instanciamos el objeto
            GameObject nuevoObjeto = Instantiate(prefabElegido, new Vector3(posicionX, alturaAparicion, 0f), Quaternion.identity);

            // --- MAGIA DEL NIVEL 2 ---
            if (modoReflejos)
            {
                // 1. Encogemos el objeto para que requiera más puntería
                nuevoObjeto.transform.localScale *= escalaObjetosNivel2;

                // 2. Le inyectamos la súper velocidad al script de caída
                ObjetoRana scriptCaida = nuevoObjeto.GetComponent<ObjetoRana>();
                if (scriptCaida != null)
                {
                    scriptCaida.velocidadCaida *= multiplicadorVelocidadNivel2;
                }
            }

            // Calculamos el tiempo de espera para la siguiente aparición
            float tiempoEspera = modoReflejos ? tiempoAparicionNivel2 : tiempoEntreApariciones;
            yield return new WaitForSeconds(tiempoEspera);
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
        ControlJuego.instancia.perderMinijuego();
    }
}
