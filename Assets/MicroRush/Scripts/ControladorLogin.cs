using TMPro;
using UnityEngine;
using UnityEngine.UI; // ¡Importante para usar el Toggle (casilla de verificación)!
using UnityEngine.SceneManagement;

public class ControladorLogin : MonoBehaviour
{
    [Header("Elementos de la Interfaz")]
    public TMP_InputField usuarioInput;
    public TMP_InputField passwordInput; // Nuevo: Para la contraseña
    public Toggle rememberMeToggle;      // Nuevo: Para la casilla "Remember Me"

    void Start()
    {
        // Si el jugador marcó "Remember Me" la última vez, rellenamos el nombre automáticamente
        if (PlayerPrefs.HasKey("UsuarioRecordado"))
        {
            usuarioInput.text = PlayerPrefs.GetString("UsuarioRecordado");
            rememberMeToggle.isOn = true;
        }
    }

    // Esta es la función que conectaremos al botón rosa gigante de "LOGIN"
    public void OnBotonLogin()
    {
        string nombreUsuario = usuarioInput.text.Trim();
        string password = passwordInput.text.Trim();

        // Comprobamos que haya escrito ALGO en ambos sitios
        if (nombreUsuario.Length > 0 && password.Length > 0)
        {
            // Lógica del "Remember Me"
            if (rememberMeToggle.isOn)
            {
                PlayerPrefs.SetString("UsuarioRecordado", nombreUsuario);
            }
            else
            {
                PlayerPrefs.DeleteKey("UsuarioRecordado"); // Lo borramos si desmarca la casilla
            }

            // Guardamos el usuario para Firebase/Ranking y cargamos la escena
            PlayerPrefs.SetString("Usuario", nombreUsuario);

            // Aquí pones la escena a la que quieras que vaya al entrar
            SceneManager.LoadScene("MenuNuevo");
        }
        else
        {
            Debug.Log("El nombre y la contraseña no pueden estar vacíos.");
        }
    }

    // Función para el texto de "Sign up" (Registrarse)
    public void OnBotonRegistro()
    {
        Debug.Log("Aquí se abriría el panel de registro");
        // Más adelante podemos hacer que esto active otro panel visual
    }
}