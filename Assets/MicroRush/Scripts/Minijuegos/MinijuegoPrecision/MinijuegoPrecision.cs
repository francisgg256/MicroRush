using UnityEngine;
using UnityEngine.UI;

public class MinijuegoPrecision : MonoBehaviour
{
    public Slider barraObjetivo;
    public float velocidad = 1.5f;
    public float tiempoRestante = 5f;

    public float margenMinimo = 0.45f;
    public float margenMaximo = 0.55f;

    private bool moviendoDerecha = true;
    private bool terminado = false;

    void Start()
    {
        barraObjetivo.value = 0f;
    }

    void Update()
    {
        if (terminado) return;

        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null) ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
            return;
        }

        if (moviendoDerecha)
        {
            barraObjetivo.value += velocidad * Time.deltaTime;
            if (barraObjetivo.value >= 1f) moviendoDerecha = false;
        }
        else
        {
            barraObjetivo.value -= velocidad * Time.deltaTime;
            if (barraObjetivo.value <= 0f) moviendoDerecha = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            terminado = true;

            if (barraObjetivo.value >= margenMinimo && barraObjetivo.value <= margenMaximo)
            {
                ControlJuego.instancia.ganarMinijuego();
            }
            else
            {
                ControlJuego.instancia.perderMinijuego();
            }
        }
    }
}
