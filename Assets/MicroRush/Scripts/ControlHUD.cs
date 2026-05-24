using UnityEngine;
using TMPro;

/// <summary>
/// Clase responsable de actualizar el Heads-Up Display (HUD) en tiempo real.
/// Cumple con la heurística de usabilidad "Visibilidad del estado del sistema", 
/// garantizando que el jugador conozca sus Vidas, Puntos y Tiempo restante en todo momento.
/// </summary>
public class ControlHUD : MonoBehaviour
{
    /// <summary>
    /// Componente de texto dinámico (TextMeshPro) que visualiza las vidas restantes del jugador.
    /// </summary>
    public TMP_Text textoVidas;

    /// <summary>
    /// Componente de texto dinámico (TextMeshPro) que muestra la puntuación acumulada actual.
    /// </summary>
    public TMP_Text textoPuntos;

    /// <summary>
    /// Componente de texto dinámico (TextMeshPro) que indica la cuenta atrás del minijuego activo.
    /// </summary>
    public TMP_Text textoTiempo;

    /// <summary>
    /// Método del ciclo de vida que se ejecuta en cada frame.
    /// Sincroniza la interfaz gráfica con los datos globales del gestor de juego (Singleton).
    /// </summary>
    void Update()
    {
        // Programación defensiva y control de excepciones: 
        // Si el gestor global aún no se ha instanciado, abortamos la ejecución para evitar un NullReferenceException.
        if (ControlJuego.instancia == null) return;

        // Comprobaciones de seguridad independientes para cada elemento de la UI.
        // Permite que el HUD siga funcionando incluso si falta algún texto por asignar en el Inspector.
        if (textoVidas != null)
            textoVidas.text = "Vidas: " + ControlJuego.instancia.vidas;

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + ControlJuego.instancia.puntuacion;

        // Mejora de Usabilidad (UX) y Formato Visual:
        // Se utiliza Mathf.Ceil() para redondear el tiempo restante hacia arriba (ej. de 4.2 a 5).
        // Esto evita mostrar decimales caóticos en pantalla y facilita la lectura rápida durante la acción.
        if (textoTiempo != null)
            textoTiempo.text = "Tiempo: " + Mathf.Ceil(ControlJuego.instancia.tiempoMinijuego).ToString();
    }
}