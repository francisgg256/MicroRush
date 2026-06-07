using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Clase principal del sistema (Game Manager).
/// Centraliza la lógica de puntuación, vidas, transiciones y la progresión de dificultad.
/// </summary>
public class ControlJuego : MonoBehaviour
{
    public static ControlJuego instancia;

    [Header("Estado Global del Jugador")]
    public int vidas = 3;
    public int puntuacion = 0;
    public float tiempoMinijuego = 0f;

    [Header("Progresión de Dificultad")]
    /// <summary>Cuántos minijuegos debe superar el jugador para que empiecen a salir los difíciles.</summary>
    public int umbralDificultad = 10;

    /// <summary>Contador interno de victorias en la sesión actual.</summary>
    public int minijuegosSuperados = 0;

    [Header("Colecciones de Escenas")]
    /// <summary>Lista de escenas de Nivel 1 (Fáciles).</summary>
    public List<string> minijuegosFaciles = new List<string>();

    /// <summary>Lista de escenas de Nivel 2 (Difíciles/Troll).</summary>
    public List<string> minijuegosDificiles = new List<string>();

    [Header("Control de Flujo de Escenas")]
    public string ultimoResultado = "";
    public string ultimoMinijuego = "";

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
        minijuegosSuperados = 0; // Reseteamos la cuenta de progreso al empezar
        CargarSiguienteMinijuego();
    }

    public void ganarMinijuego()
    {
        puntuacion += 100;
        minijuegosSuperados++; // Sumamos una victoria al progreso
        ultimoResultado = "Ganado";

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

            SceneManager.LoadScene("Resultados");
        }
        else
        {
            SceneManager.LoadScene("VictoriaMinijuego");
        }
    }

    public void CargarSiguienteMinijuego()
    {
        // 1. Decidimos qué lista usar basándonos en las victorias del jugador
        List<string> listaActual = (minijuegosSuperados >= umbralDificultad) ? minijuegosDificiles : minijuegosFaciles;

        // 2. Control de seguridad por si olvidaste llenar las listas en Unity
        if (listaActual.Count == 0)
        {
            Debug.LogError("ERROR: La lista de minijuegos está vacía. ¡Añade escenas en el Inspector!");
            return;
        }

        // 3. Si solo hay 1 minijuego, lo cargamos directo para no colgar el juego en el bucle 'while'
        if (listaActual.Count == 1)
        {
            ultimoMinijuego = listaActual[0];
            SceneManager.LoadScene(listaActual[0]);
            return;
        }

        // 4. Elegimos uno aleatorio
        string siguiente = listaActual[Random.Range(0, listaActual.Count)];

        // Bucle para no repetir el último minijuego jugado
        while (siguiente == ultimoMinijuego)
        {
            siguiente = listaActual[Random.Range(0, listaActual.Count)];
        }

        ultimoMinijuego = siguiente;
        SceneManager.LoadScene(siguiente);
    }
}