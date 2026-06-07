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
    public int maxAnunciosSimultaneos = 4;

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
                ControlJuego.instancia.ganarMinijuego();
            }
            else
            {
                ControlJuego.instancia.perderMinijuego();
            }
        }
    }

    /// <summary>
    /// Instancia anuncios calculando sus medidas reales para mantenerlos siempre dentro del recuadro.
    /// </summary>
    IEnumerator GeneradorAnuncios()
    {
        if (pantallaOrdenador == null)
        {
            Debug.LogError("Error: No has asignado el objeto 'Ordenador' al script.");
            yield break;
        }

        // Medimos el tamaño de la pantalla del ordenador
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

            // --- NUEVA LÓGICA DE BORDES PERFECTOS ---
            // 1. Medimos el tamaño EXACTO del anuncio que acaba de nacer
            RectTransform rectAnuncio = nuevoAnuncio.GetComponent<RectTransform>();
            float anchoAnuncio = rectAnuncio.rect.width;
            float altoAnuncio = rectAnuncio.rect.height;

            // 2. El margen es exactamente la mitad de su tamaño, más 15 píxeles de "respiro" estético
            float margenX = (anchoAnuncio / 2f) + 15f;
            float margenY = (altoAnuncio / 2f) + 15f;

            // 3. Calculamos la zona segura
            float rangeX = (anchoPantalla / 2f) - margenX;
            float rangeY = (altoPantalla / 2f) - margenY;

            // Freno de seguridad por si el anuncio es más grande que la propia pantalla
            rangeX = Mathf.Max(0, rangeX);
            rangeY = Mathf.Max(0, rangeY);

            // 4. Aplicamos la posición matemática perfecta
            Vector3 offsetAleatorio = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
            nuevoAnuncio.transform.localPosition = offsetAleatorio;

            // Conexión del botón
            Button botonAnuncio = nuevoAnuncio.GetComponentInChildren<Button>();
            if (botonAnuncio != null)
            {
                botonAnuncio.onClick.RemoveAllListeners();
                botonAnuncio.onClick.AddListener(() => CerrarAnuncio(nuevoAnuncio));
            }

            anunciosEnPantalla++;

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
