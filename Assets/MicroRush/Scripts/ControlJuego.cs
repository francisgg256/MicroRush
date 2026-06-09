using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Clase principal del sistema (Game Manager).
/// Centraliza la lógica de puntuación, progresión y aplica un sistema de 
/// "Mazo de cartas" (Bag System) para evitar la repetición prematura de minijuegos.
/// </summary>
public class ControlJuego : MonoBehaviour
{
    public static ControlJuego instancia;

    [Header("Estado Global del Jugador")]
    public int vidas = 4;
    public int puntuacion = 0;
    public float tiempoMinijuego = 0f;

    [Header("Progresión de Dificultad")]
    public int umbralDificultad = 10;
    public int minijuegosSuperados = 0;
    private bool avisoDificultadMostrado = false;

    [Header("Colecciones de Escenas (Base)")]
    public List<string> minijuegosFaciles = new List<string>();
    public List<string> minijuegosDificiles = new List<string>();

    // --- NUEVAS LISTAS: EL SISTEMA DE BOLSA ---
    private List<string> facilesDisponibles = new List<string>();
    private List<string> dificilesDisponibles = new List<string>();

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
        vidas = 4;
        puntuacion = 0;
        minijuegosSuperados = 0;
        avisoDificultadMostrado = false;

        // --- LLENAMOS LAS BOLSAS AL EMPEZAR ---
        facilesDisponibles = new List<string>(minijuegosFaciles);
        dificilesDisponibles = new List<string>(minijuegosDificiles);
        ultimoMinijuego = "";

        if (ControladorAudio.instancia != null)
        {
            ControladorAudio.instancia.ReanudarMusicaFondo();
        }

        CargarSiguienteMinijuego();
    }

    public void ganarMinijuego()
    {
        puntuacion += 100;
        minijuegosSuperados++;
        ultimoResultado = "Ganado";

        SceneManager.LoadScene("VictoriaMinijuego");
    }

    public void perderMinijuego()
    {
        if (ControladorAudio.instancia != null)
            ControladorAudio.instancia.ReproducirSonidoDerrota();

        vidas--;
        ultimoResultado = "Perdido";

        if (vidas <= 0)
        {
            if (ControladorAudio.instancia != null)
                ControladorAudio.instancia.ReproducirSonidoGameOver();

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
        // 1. Comprobamos si hay que lanzar el aviso de dificultad extrema
        if (minijuegosSuperados == umbralDificultad && !avisoDificultadMostrado)
        {
            avisoDificultadMostrado = true;
            SceneManager.LoadScene("AvisoDificultad");
            return;
        }

        // 2. Decidimos en qué nivel estamos
        bool esDificil = minijuegosSuperados >= umbralDificultad;

        // 3. --- RECARGA DE LA BOLSA (Si está vacía) ---
        if (esDificil && dificilesDisponibles.Count == 0)
        {
            dificilesDisponibles = new List<string>(minijuegosDificiles);
        }
        else if (!esDificil && facilesDisponibles.Count == 0)
        {
            facilesDisponibles = new List<string>(minijuegosFaciles);
        }

        // 4. Elegimos el minijuego de la bolsa correcta
        string siguiente = "";
        int indiceAleatorio = 0;

        if (esDificil)
        {
            indiceAleatorio = Random.Range(0, dificilesDisponibles.Count);
            siguiente = dificilesDisponibles[indiceAleatorio];

            // Control Anti-Repetición al rellenar la bolsa
            if (siguiente == ultimoMinijuego && dificilesDisponibles.Count > 1)
            {
                while (siguiente == ultimoMinijuego)
                {
                    indiceAleatorio = Random.Range(0, dificilesDisponibles.Count);
                    siguiente = dificilesDisponibles[indiceAleatorio];
                }
            }

            // BORRAMOS DE LA LISTA GLOBAL
            dificilesDisponibles.RemoveAt(indiceAleatorio);
        }
        else
        {
            indiceAleatorio = Random.Range(0, facilesDisponibles.Count);
            siguiente = facilesDisponibles[indiceAleatorio];

            // Control Anti-Repetición al rellenar la bolsa
            if (siguiente == ultimoMinijuego && facilesDisponibles.Count > 1)
            {
                while (siguiente == ultimoMinijuego)
                {
                    indiceAleatorio = Random.Range(0, facilesDisponibles.Count);
                    siguiente = facilesDisponibles[indiceAleatorio];
                }
            }

            // BORRAMOS DE LA LISTA GLOBAL
            facilesDisponibles.RemoveAt(indiceAleatorio);
        }

        ultimoMinijuego = siguiente;
        SceneManager.LoadScene(siguiente);
    }
}