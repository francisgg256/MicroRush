using UnityEngine;

/// <summary>
/// Script de utilidad para colocar en cualquier botón de cualquier escena.
/// Se comunica automáticamente con el Gestor de Audio global sin necesidad de arrastrar referencias.
/// </summary>
public class BotonSonido : MonoBehaviour
{
    public void HacerSonarClick()
    {
        // Comprobamos que el gestor global existe (por si estamos probando la escena suelta)
        if (ControladorAudio.instancia != null)
        {
            ControladorAudio.instancia.ReproducirSonidoBoton();
        }
        else
        {
            Debug.LogWarning("El ControladorAudio no está en la escena. Recuerda empezar desde el Menú.");
        }
    }
}
