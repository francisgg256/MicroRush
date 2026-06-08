using System.Collections;
using UnityEngine;

/// <summary>
/// Controlador para la pantalla de transición de "¡Nivel 2 / Dificultad Aumentada!".
/// </summary>
public class PantallaAviso : MonoBehaviour
{
    [Tooltip("Tiempo en segundos que el jugador verá esta pantalla antes de continuar.")]
    public float tiempoDeEspera = 2.5f;

    void Start()
    {
        // --- NUEVA LÍNEA DE AUDIO ---
        // Hacemos sonar la sirena de Smash Bros nada más cargar la pantalla
        if (ControladorAudio.instancia != null)
        {
            ControladorAudio.instancia.ReproducirSonidoAvisoDificultad();
        }
        // ----------------------------

        // Iniciamos la cuenta atrás 
        StartCoroutine(EsperarYContinuar());
    }

    IEnumerator EsperarYContinuar()
    {
        // Esperamos los segundos configurados
        yield return new WaitForSeconds(tiempoDeEspera);

        // Le decimos al Gestor Global que cargue el primer minijuego difícil
        if (ControlJuego.instancia != null)
        {
            ControlJuego.instancia.CargarSiguienteMinijuego();
        }
    }
}
