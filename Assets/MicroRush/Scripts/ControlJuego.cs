using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControlJuego : MonoBehaviour
{
    public static ControlJuego instancia;
    public int vidas = 3;
    public int puntuacion = 0;
    public float tiempoMinijuego = 0f;
    public string ultimoResultado = "";
    public string ultimoMinijuego = "";
    public List<string> minijuegos = new List<string>() { "MinijuegoSaltos", "MinijuegoMachaca" };

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

    public void IniciarPartida()
    {
        vidas = 3;
        puntuacion = 0;
        CargarSiguienteMinijuego(); 
    }

    public void ganarMinijuego()
    {
        puntuacion += 100;
        ultimoResultado = "Ganado";

        // Apuntamos a la escena única de resultados del minijuego
        SceneManager.LoadScene("VictoriaMinijuego");
    }

    public void perderMinijuego()
    {
        vidas--;
        ultimoResultado = "Perdido";

        if (vidas <= 0)
        {
            string nombre = PlayerPrefs.GetString("Usuario", "Jugador");
            if (ControladorFirebase.instancia != null)
                ControladorFirebase.instancia.GuardarPuntuacion(nombre, puntuacion);

            // Si se queda sin vidas, va al Game Over total
            SceneManager.LoadScene("Resultados");
        }
        else
        {
            // Si le quedan vidas, va a la misma escena de transición
            SceneManager.LoadScene("VictoriaMinijuego");
        }
    }

    public void CargarSiguienteMinijuego()
    {
        if (minijuegos.Count <= 1)
        {
            SceneManager.LoadScene(minijuegos[0]);
            return;
        }

        string siguiente = minijuegos[Random.Range(0, minijuegos.Count)];

        while (siguiente == ultimoMinijuego)
        {
            siguiente = minijuegos[Random.Range(0, minijuegos.Count)];
        }

        ultimoMinijuego = siguiente;

        SceneManager.LoadScene(siguiente);
    }
}