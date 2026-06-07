using UnityEngine;

/// <summary>
/// Script genérico aplicable a cualquier elemento que caiga del cielo en el minijuego de la cesta.
/// Discrimina mediante una bandera lógica si el objeto premia o castiga al jugador.
/// </summary>
public class ObjetoCayendo : MonoBehaviour
{
    [Header("Configuración del Objeto")]
    /// <summary>Si está activo, actúa como pincho (derrota inmediata). Si está desactivo, actúa como fruta buena.</summary>
    public bool esLetal = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // El objeto entra en contacto con el activador de la cesta
        if (collision.CompareTag("Cesta"))
        {
            CestaGameManager manager = FindFirstObjectByType<CestaGameManager>();

            if (manager != null)
            {
                if (esLetal)
                {
                    // ¡Trampa! El jugador atrapó un pincho
                    manager.RegistrarDerrotaInmediata();
                }
                else
                {
                    // Éxito: El jugador atrapó una fruta buena
                    manager.SumarFruta();
                }
            }

            Destroy(gameObject);
        }
        // El objeto cae al suelo sin ser atrapado
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}