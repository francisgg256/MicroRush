using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador de la pantalla de título inicial (Splash Screen).
/// Gestiona la primera interacción del usuario con la aplicación implementando 
/// el patrón clásico de diseño arcade "Press Any Key to Start".
/// </summary>
public class ControladorInicio : MonoBehaviour
{
    /// <summary>
    /// Método del ciclo de vida que se ejecuta en cada frame.
    /// Queda a la espera del primer input del jugador para realizar la transición al Menú Principal.
    /// </summary>
    void Update()
    {
        // Usabilidad y Accesibilidad: Input.anyKeyDown detecta cualquier tecla del teclado
        // o clic del ratón. Esto elimina la fricción inicial, permitiendo al usuario 
        // avanzar a la siguiente pantalla de la forma más rápida y cómoda posible.
        if (Input.anyKeyDown)
        {
            // Carga de forma síncrona la escena del Menú Principal
            SceneManager.LoadScene("Menu");
        }
    }
}
