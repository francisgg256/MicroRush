using UnityEngine;

public class MusicPersistent : MonoBehaviour
{
    private static MusicPersistent instance;

    void Awake()
    {
        // Esto evita que haya dos músicas sonando a la vez si vuelves al menú principal
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            // ESTA ES LA LÍNEA MÁGICA: No destruye el objeto al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
    }
}
