using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase encargada de controlar la interfaz gráfica y el flujo de navegación del Menú Principal.
/// Gestiona la visualización del perfil del usuario activo y los eventos de los botones 
/// para acceder a los diferentes módulos del juego o finalizar la aplicación.
/// </summary>
public class ControladorMenu : MonoBehaviour
{
    [Header("Elementos de la Interfaz")]

    /// <summary>
    /// Componente de texto (TextMeshPro) que muestra el nombre del jugador actual en pantalla.
    /// </summary>
    public TMP_Text nombreUsuario;

    /// <summary>
    /// Método de inicialización del ciclo de vida de Unity.
    /// Recupera la persistencia del usuario y aplica un control de diseño visual adaptativo
    /// para asegurar la correcta visualización de la interfaz.
    /// </summary>
    void Start()
    {
        // Rescatamos el nombre que guardamos localmente en la pantalla de Login (o "Jugador" por defecto)
        string usuario = PlayerPrefs.GetString("Usuario", "Jugador");

        // Control de excepciones: Verifica que la referencia del texto esté asignada en el Inspector
        if (nombreUsuario != null)
        {
            // Ajuste de Usabilidad (DI): Se asigna directamente la cadena 'usuario' sin prefijos.
            // Esto optimiza el espacio horizontal y previene el desbordamiento visual (Text Overflow)
            // dentro del recuadro de la interfaz de usuario arriba a la izquierda.
            nombreUsuario.text = usuario;
        }
    }

    /// <summary>
    /// Evento vinculado al botón de inicio. 
    /// Realiza un paso de mensajes hacia el Singleton global para arrancar el ciclo de minijuegos.
    /// </summary>
    public void OnBotonJugar()
    {
        // Comunicación entre clases mediante el patrón de diseño Singleton
        ControlJuego.instancia.IniciarPartida();
    }

    /// <summary>
    /// Evento vinculado al botón de clasificaciones.
    /// Carga de forma síncrona la escena que conecta con Firebase para mostrar el Top 5.
    /// </summary>
    public void OnBotonRanking()
    {
        SceneManager.LoadScene("Ranking");
    }

    /// <summary>
    /// Evento vinculado al botón de Login.
    /// Permite al usuario regresar a la pantalla de autenticación para cambiar de cuenta.
    /// </summary>
    public void OnBotonLogin()
    {
        SceneManager.LoadScene("Login");
    }

    /// <summary>
    /// Evento vinculado al botón de cierre.
    /// Finaliza la ejecución del juego de forma segura liberando los procesos del sistema.
    /// </summary>
    public void OnBotonSalir()
    {
        // Registro en la consola de Unity para pruebas de depuración en el editor
        Debug.Log("Saliendo de la aplicación y liberando procesos...");

        // Cierra el ejecutable compilado (No tiene efecto dentro del editor de Unity)
        Application.Quit();
    }
}