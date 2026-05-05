using UnityEngine;
using TMPro;

public class ControlHUD : MonoBehaviour
{
    public TMP_Text textoVidas;
    public TMP_Text textoPuntos;
    public TMP_Text textoTiempo;

    void Update()
    {
        if (ControlJuego.instancia == null) return;

        if (textoVidas != null) textoVidas.text = "Vidas: " + ControlJuego.instancia.vidas;
        if (textoPuntos != null) textoPuntos.text = "Puntos: " + ControlJuego.instancia.puntuacion;
        if (textoTiempo != null) textoTiempo.text = "Tiempo: " + Mathf.Ceil(ControlJuego.instancia.tiempoMinijuego).ToString();
    }
}