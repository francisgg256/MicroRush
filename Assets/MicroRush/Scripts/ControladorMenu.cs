using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    [Header("Elementos de la Interfaz")]
    public TMP_Text nombreUsuario;

    void Start()
    {
        // Rescatamos el nombre que guardamos en la pantalla de Login
        string usuario = PlayerPrefs.GetString("Usuario", "Jugador");

        if (nombreUsuario != null)
        {
            // CAMBIO AQUÍ: Quitamos el "Jugador: " para que solo ponga el nombre 
            // y no se salga del recuadro visual que tienes arriba a la izquierda.
            nombreUsuario.text = usuario;
        }
    }

    public void OnBotonJugar()
    {
        ControlJuego.instancia.IniciarPartida();
    }

    public void OnBotonRanking()
    {
        SceneManager.LoadScene("RankingNuevo");
    }

    public void OnBotonLogin()
    {
        SceneManager.LoadScene("LoginNuevo");
    }

    public void OnBotonSalir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}