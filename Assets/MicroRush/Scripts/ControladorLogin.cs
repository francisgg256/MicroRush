using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador principal de la interfaz de inicio de sesión (Login).
/// Gestiona la captura de datos del usuario, la validación de campos,
/// el almacenamiento de preferencias locales y la navegación entre escenas.
/// </summary>
public class ControladorLogin : MonoBehaviour
{
    [Header("Elementos de la Interfaz")]

    /// <summary>
    /// Campo de entrada de texto (TextMeshPro) para el nombre de usuario.
    /// </summary>
    public TMP_InputField usuarioInput;

    /// <summary>
    /// Campo de entrada de texto (TextMeshPro) para la contraseña.
    /// Su contenido debe estar ofuscado en la interfaz visual por motivos de seguridad.
    /// </summary>
    public TMP_InputField passwordInput;

    /// <summary>
    /// Casilla de verificación para la opción "Recordarme".
    /// Mejora la usabilidad al evitar que el usuario tenga que introducir sus datos recurrentemente.
    /// </summary>
    public Toggle rememberMeToggle;

    /// <summary>
    /// Método de inicialización del ciclo de vida de Unity.
    /// Comprueba la persistencia de datos locales (PlayerPrefs) para autocompletar la interfaz
    /// aplicando la heurística de usabilidad: "Reconocer en lugar de recordar".
    /// </summary>
    void Start()
    {
        // Si existe un nombre de usuario guardado previamente en la memoria del dispositivo
        if (PlayerPrefs.HasKey("UsuarioRecordado"))
        {
            usuarioInput.text = PlayerPrefs.GetString("UsuarioRecordado");
            rememberMeToggle.isOn = true;
        }
    }

    /// <summary>
    /// Función ejecutada al pulsar el botón principal de acceso (Login).
    /// Valida la entrada de datos, gestiona las preferencias locales de guardado 
    /// y transiciona a la escena principal del juego.
    /// </summary>
    public void OnBotonLogin()
    {
        // Se utiliza Trim() para eliminar espacios accidentales al inicio y final (Prevención de errores)
        string nombreUsuario = usuarioInput.text.Trim();
        string password = passwordInput.text.Trim();

        // Control de excepciones y validación básica: los campos no pueden estar vacíos
        if (nombreUsuario.Length > 0 && password.Length > 0)
        {
            // Gestión de la preferencia "Recordarme"
            if (rememberMeToggle.isOn)
            {
                PlayerPrefs.SetString("UsuarioRecordado", nombreUsuario);
            }
            else
            {
                // Limpieza de datos si el usuario desmarca la opción
                PlayerPrefs.DeleteKey("UsuarioRecordado");
            }

            // Almacena el usuario activo actual para ser utilizado posteriormente en el envío de puntuaciones a Firebase
            PlayerPrefs.SetString("Usuario", nombreUsuario);

            // Carga el menú principal tras una validación exitosa
            SceneManager.LoadScene("Menu");
        }
        else
        {
            // Aviso por consola (idealmente se mostraría un texto de error en rojo en la propia UI)
            Debug.Log("Error de validación: El nombre y la contraseña no pueden estar vacíos.");
        }
    }

    /// <summary>
    /// Función ejecutada al pulsar el enlace o botón de "Sign up" (Registrarse).
    /// Preparado para futuras iteraciones del flujo de autenticación.
    /// </summary>
    public void OnBotonRegistro()
    {
        Debug.Log("Abriendo panel de registro de nuevo usuario...");
        // Futura implementación visual del panel de registro
    }

    /// <summary>
    /// Función de escape o retroceso vinculada al botón de salida.
    /// Proporciona control y libertad al usuario para regresar a la pantalla anterior sin iniciar sesión.
    /// </summary>
    public void OnBotonVolver()
    {
        SceneManager.LoadScene("Menu");
    }
}