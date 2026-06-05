using UnityEngine;

/// <summary>
/// Controla el descenso de la tecla y avisa si se sale de la pantalla (derrota).
/// </summary>
public class NotaGuitarra : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadDescenso = 5f;

    [Tooltip("La tecla del teclado que corresponde a este dibujo")]
    public KeyCode teclaAsociada;

    void Update()
    {
        // La nota baja constantemente
        transform.Translate(Vector3.down * velocidadDescenso * Time.deltaTime);

        // Si la nota pasa de largo de la diana y se cae por abajo de la pantalla (ej: Y < -6)
        if (transform.position.y < -6f)
        {
            // Avisamos al manager de que hemos fallado
            if (MinijuegoGuitarra.instancia != null)
            {
                MinijuegoGuitarra.instancia.PerderPorNotaPerdida();
            }
            Destroy(gameObject);
        }
    }

    /// <summary>Destruye la nota cuando el jugador acierta.</summary>
    public void Esfumarse()
    {
        // Aquí puedes añadir en el futuro partículas o un sonido de "Perfect"
        Destroy(gameObject);
    }
}
