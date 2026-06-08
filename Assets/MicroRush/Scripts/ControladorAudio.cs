using UnityEngine;

/// <summary>
/// Gestor global de audio (Singleton). Sobrevive a los cambios de escena y centraliza
/// la reproducción de efectos de sonido (SFX) y Música para mantener el código limpio.
/// </summary>
public class ControladorAudio : MonoBehaviour
{
    public static ControladorAudio instancia;

    [Header("Reproductores")]
    [Tooltip("El componente que reproducirá los efectos cortos (SFX)")]
    public AudioSource reproductorSFX;

    [Tooltip("El componente que reproducirá la música de fondo en bucle")]
    public AudioSource reproductorMusica; // --- NUEVO HUECO ---

    [Header("Clips de Sonido")]
    public AudioClip sonidoBoton;
    public AudioClip sonidoDerrota;
    public AudioClip sonidoGameOver;
    public AudioClip sonidoFruta;
    public AudioClip sonidoAvisoDificultad;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- CONTROL DE LA MÚSICA DE FONDO ---

    /// <summary>
    /// Vuelve a encender la música principal si estaba apagada (ej: al reiniciar partida).
    /// </summary>
    public void ReanudarMusicaFondo()
    {
        if (reproductorMusica != null && !reproductorMusica.isPlaying)
        {
            reproductorMusica.Play();
        }
    }

    // --- EFECTOS DE SONIDO ---

    public void ReproducirSonidoBoton()
    {
        if (reproductorSFX != null && sonidoBoton != null)
            reproductorSFX.PlayOneShot(sonidoBoton);
    }

    public void ReproducirSonidoDerrota()
    {
        if (reproductorSFX != null && sonidoDerrota != null)
            reproductorSFX.PlayOneShot(sonidoDerrota);
    }

    public void ReproducirSonidoGameOver()
    {
        if (reproductorSFX != null && sonidoGameOver != null)
        {
            reproductorSFX.Stop();

            // --- APAGAMOS LA MÚSICA DE FONDO AQUÍ ---
            if (reproductorMusica != null)
            {
                reproductorMusica.Stop();
            }
            // ----------------------------------------

            reproductorSFX.PlayOneShot(sonidoGameOver);
        }
    }

    public void ReproducirSonidoFruta()
    {
        if (reproductorSFX != null && sonidoFruta != null)
            reproductorSFX.PlayOneShot(sonidoFruta);
    }

    public void ReproducirSonidoAvisoDificultad()
    {
        if (reproductorSFX != null && sonidoAvisoDificultad != null)
        {
            reproductorSFX.Stop();
            reproductorSFX.PlayOneShot(sonidoAvisoDificultad);
        }
    }
}