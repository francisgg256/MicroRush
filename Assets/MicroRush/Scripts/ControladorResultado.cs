using UnityEngine;
using TMPro; // Para controlar los textos de los puntos

public class ControladorResultado : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoEspera = 3f;

    [Header("Textos de Información (Loot)")]
    public TMP_Text textoPuntos;
    // public TMP_Text textoTiempo; // Descomenta esto si al final pones el tiempo

    void Start()
    {
        // --- AQUÍ ACTUALIZAREMOS TUS PUNTOS MÁS ADELANTE ---
        // Por ejemplo, si tienes una variable en ControlJuego que guarda los puntos que acaba de ganar:
        if (textoPuntos != null)
        {
            // textoPuntos.text = "+" + ControlJuego.instancia.puntosGanados.ToString();
        }

        // --- CUENTA ATRÁS PARA SALTAR AL SIGUIENTE MINIJUEGO ---
        Invoke("PasarAlSiguiente", tiempoEspera);
    }

    void PasarAlSiguiente()
    {
        // Llamamos a tu sistema central para que cargue el siguiente nivel
        ControlJuego.instancia.CargarSiguienteMinijuego();
    }
}
