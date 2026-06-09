using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Gestor del minijuego de cerrar pop-ups.
/// Calcula dinámicamente el tamaño de cada anuncio para evitar que se salgan de la pantalla.
/// </summary>
public class MinijuegoAnuncios : MonoBehaviour
{
    [Header("Control de Inicio")]
    public bool juegoIniciado = false;

    [Header("Configuración General")]
    public float tiempoRestante = 5f;
    private bool terminado = false;

    [Header("Modo Clásico (Nivel 1)")]
    public int anunciosRestantes = 5;

    [Header("Modo Dinámico (Nivel 2)")]
    public bool usarGeneradorDinamico = false;
    public GameObject[] prefabsAnuncios;
    public Transform pantallaOrdenador;
    public float ritmoAparicion = 0.6f;

    /// <summary>Límite máximo de anuncios en pantalla antes de que el sistema colapse y pierdas.</summary>
    public int maxAnunciosSimultaneos = 5;

    private int anunciosEnPantalla = 0;

    void Start()
    {
        if (!usarGeneradorDinamico)
        {
            anunciosEnPantalla = anunciosRestantes;
        }
    }

    public void IniciarMinijuego()
    {
        juegoIniciado = true;

        if (usarGeneradorDinamico)
        {
            anunciosEnPantalla = 0;
            StartCoroutine(GeneradorAnuncios());
        }
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

            if (usarGeneradorDinamico)
            {
                // En nivel 2, si sobrevives al tiempo sin acumular 5 anuncios, ganas.
                ControlJuego.instancia.ganarMinijuego();
            }
            else
            {
                // En nivel 1, si se acaba el tiempo y no has cerrado los fijos, pierdes.
                ControlJuego.instancia.perderMinijuego();
            }
        }
    }

    IEnumerator GeneradorAnuncios()
    {
        if (pantallaOrdenador == null)
        {
            Debug.LogError("Error: No has asignado el objeto 'Ordenador' al script.");
            yield break;
        }

        RectTransform rectPantalla = pantallaOrdenador.GetComponent<RectTransform>();
        float anchoPantalla = rectPantalla.rect.width;
        float altoPantalla = rectPantalla.rect.height;

        while (juegoIniciado && !terminado)
        {
            yield return new WaitForSeconds(ritmoAparicion);

            if (terminado || !juegoIniciado) break;

            int indiceAleatorio = Random.Range(0, prefabsAnuncios.Length);
            GameObject anuncioElegido = prefabsAnuncios[indiceAleatorio];

            GameObject nuevoAnuncio = Instantiate(anuncioElegido, pantallaOrdenador);

            // --- LÓGICA DE BORDES PERFECTOS ---
            RectTransform rectAnuncio = nuevoAnuncio.GetComponent<RectTransform>();
            float anchoAnuncio = rectAnuncio.rect.width;
            float altoAnuncio = rectAnuncio.rect.height;

            float margenX = (anchoAnuncio / 2f) + 15f;
            float margenY = (altoAnuncio / 2f) + 15f;

            float rangeX = Mathf.Max(0, (anchoPantalla / 2f) - margenX);
            float rangeY = Mathf.Max(0, (altoPantalla / 2f) - margenY);

            Vector3 offsetAleatorio = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
            nuevoAnuncio.transform.localPosition = offsetAleatorio;

            // Conexión del botón para destruir el anuncio
            Button botonAnuncio = nuevoAnuncio.GetComponentInChildren<Button>();
            if (botonAnuncio != null)
            {
                botonAnuncio.onClick.RemoveAllListeners();
                botonAnuncio.onClick.AddListener(() => CerrarAnuncio(nuevoAnuncio));
            }

            anunciosEnPantalla++;

            // --- CONDICIÓN DE DERROTA POR COLAPSO ---
            if (anunciosEnPantalla >= maxAnunciosSimultaneos)
            {
                terminado = true;
                ControlJuego.instancia.perderMinijuego();
            }
        }
    }

    public void CerrarAnuncio(GameObject anuncio)
    {
        if (terminado || !juegoIniciado) return;

        Destroy(anuncio);

        if (usarGeneradorDinamico)
        {
            anunciosEnPantalla--;
        }
        else
        {
            anunciosRestantes--;
            if (anunciosRestantes <= 0)
            {
                terminado = true;
                ControlJuego.instancia.ganarMinijuego();
            }
        }
    }
}
