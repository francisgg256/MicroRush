using UnityEngine;

/// <summary>
/// Gestor del minijuego de cerrar pop-ups.
/// Controla el tiempo límite y verifica si todos los anuncios han sido destruidos.
/// </summary>
public class MinijuegoAnuncios : MonoBehaviour
{
    [Header("Control de Inicio")]
    /// <summary>Candado lógico. Evita que el nivel y el tiempo funcionen mientras se lee el cartel.</summary>
    public bool juegoIniciado = false;

    [Header("Configuración del Nivel")]
    /// <summary>Tiempo límite para cerrar todos los pop-ups.</summary>
    public float tiempoRestante = 6f;

    /// <summary>Cuántos anuncios hay en pantalla al empezar.</summary>
    public int anunciosRestantes = 5;

    private bool terminado = false;

    /// <summary>Método llamado por el cartel universal de UI para desbloquear el minijuego.</summary>
    public void IniciarMinijuego()
    {
        juegoIniciado = true;
    }

    void Update()
    {
        // Candado lógico
        if (terminado || !juegoIniciado) return;

        // Cronómetro hacia atrás
        tiempoRestante -= Time.deltaTime;

        // Actualizamos el HUD del tiempo si existe el ControlJuego
        if (ControlJuego.instancia != null)
            ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        // Derrota si se acaba el tiempo y aún quedan anuncios
        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }

    /// <summary>
    /// Evento que se ejecuta al hacer clic en el botón invisible de la 'X'.
    /// </summary>
    /// <param name="anuncio">El GameObject entero del anuncio que queremos borrar de la pantalla.</param>
    public void CerrarAnuncio(GameObject anuncio)
    {
        // Si el juego ha terminado o no ha empezado, ignoramos los clics
        if (terminado || !juegoIniciado) return;

        // Destruye el objeto del anuncio de la interfaz gráfica
        Destroy(anuncio);

        // Restamos uno al contador interno
        anunciosRestantes--;

        // Comprobación de Victoria: ¿Hemos limpiado la pantalla?
        if (anunciosRestantes <= 0)
        {
            terminado = true;
            ControlJuego.instancia.ganarMinijuego();
        }
    }
}
