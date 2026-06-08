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
    // Estas listas son temporales y se irán vaciando conforme el jugador avance.
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
        // Copiamos la lista original a la lista de "disponibles"
        facilesDisponibles = new List<string>(minijuegosFaciles);
        dificilesDisponibles = new List<string>(minijuegosDificiles);
        // --------------------------------------

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
        List<string> listaBase = esDificil ? minijuegosDificiles : minijuegosFaciles;
        List<string> listaDisponibles = esDificil ? dificilesDisponibles : facilesDisponibles;

        if (listaBase.Count == 0)
        {
            Debug.LogError("ERROR: La lista base de minijuegos está vacía en el Inspector.");
            return;
        }

        // 3. --- RECARGA DE LA BOLSA ---
        // Si ya hemos jugado TODOS los minijuegos y la bolsa está vacía, la rellenamos.
        if (listaDisponibles.Count == 0)
        {
            listaDisponibles.AddRange(listaBase);
        }

        // 4. Elegimos uno al azar de los que QUEDAN en la bolsa
        int indiceAleatorio = Random.Range(0, listaDisponibles.Count);
        string siguiente = listaDisponibles[indiceAleatorio];

        // 5. Pequeño control por si, al rellenar la bolsa, el nuevo juego sacado 
        // resulta ser exactamente el mismo que el último jugado (para evitar repetición de choque).
        if (siguiente == ultimoMinijuego && listaDisponibles.Count > 1)
        {
            while (siguiente == ultimoMinijuego)
            {
                indiceAleatorio = Random.Range(0, listaDisponibles.Count);
                siguiente = listaDisponibles[indiceAleatorio];
            }
        }

        // 6. ¡Súper importante! SACAMOS el minijuego de la bolsa para que no vuelva a salir
        listaDisponibles.RemoveAt(indiceAleatorio);

        ultimoMinijuego = siguiente;
        SceneManager.LoadScene(siguiente);
    }
}