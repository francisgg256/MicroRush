using UnityEngine;

/// <summary>
/// Clase encargada de gestionar la mecánica de salto del jugador.
/// Implementa un sistema seguro que separa la lectura de la entrada del usuario 
/// de la ejecución física, y verifica el contacto con el suelo para evitar saltos infinitos.
/// </summary>
public class SaltoJugador : MonoBehaviour
{
    /// <summary>
    /// Fuerza vertical que se aplicará al jugador al saltar.
    /// </summary>
    public int fuerzaSalto;

    /// <summary>
    /// Referencia al componente Rigidbody2D del jugador.
    /// </summary>
    public Rigidbody2D fisica;

    /// <summary>
    /// Punto de origen para el lanzamiento del rayo detector de suelo.
    /// </summary>
    public Transform puntoSuelo;

    /// <summary>
    /// Referencia al componente AudioSource que reproducirá el sonido.
    /// </summary>
    public AudioSource sonidoSalto;

    private bool entradaSalto;

    private void FixedUpdate()
    {
        if (entradaSalto)
        {
            fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            entradaSalto = false;
        }
    }

    void Update()
    {
        // Comprueba si se pulsa la barra espaciadora Y si el personaje está tocando el suelo
        if (Input.GetKeyDown(KeyCode.Space) && tocarSuelo())
        {
            entradaSalto = true;

            // Reproducir sonido al saltar
            if (sonidoSalto != null)
            {
                sonidoSalto.Play();
            }
        }
    }

    private bool tocarSuelo()
    {
        RaycastHit2D toca = Physics2D.Raycast(puntoSuelo.position, Vector2.down, 0.2f);
        Debug.DrawRay(puntoSuelo.position, Vector2.down * 0.2f, Color.red);

        if (toca.collider != null && !toca.collider.CompareTag("Jugador"))
        {
            return true;
        }

        return false;
    }
}