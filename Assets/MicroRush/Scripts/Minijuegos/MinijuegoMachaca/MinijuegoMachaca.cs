using UnityEngine;
using UnityEngine.UI; 

public class MinijuegoMachaca : MonoBehaviour
{
    public Slider barraProgreso; 
    public float progreso = 0f;
    public float tiempoRestante = 10f;

    private bool terminado = false;

    void Start()
    {
        barraProgreso.value = 0f; 
    }

    void Update()
    {
        if (terminado) return;

        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null) ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            progreso += 10f;
        }

        progreso -= 15f * Time.deltaTime;

        progreso = Mathf.Clamp(progreso, 0f, 100f);

        barraProgreso.value = progreso / 100f;

        if (progreso >= 100f)
        {
            terminado = true;
            ControlJuego.instancia.ganarMinijuego();
        }
        else if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }
}
