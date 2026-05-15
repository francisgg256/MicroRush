using UnityEngine;

public class CamaraCorredor : MonoBehaviour
{
    [Header("A quién seguimos")]
    public Transform jugador;

    [Header("Configuración")]
    public float distanciaAdelante = 3f; // Para que la cámara vaya un poco por delante y veas las trampas con tiempo

    void LateUpdate()
    {
        if (jugador != null)
        {
            // La cámara sigue al jugador en la X, pero mantiene su propia altura (Y) fija.
            // El -10f en la Z es súper importante para que la cámara no se meta dentro del escenario en 2D
            transform.position = new Vector3(jugador.position.x + distanciaAdelante, transform.position.y, -10f);
        }
    }
}
