using UnityEngine;

/// <summary>
/// Controla el movimiento de caída de las frutas y pinchos, 
/// y los autodestruye si salen de la pantalla para no consumir memoria.
/// </summary>
public class ObjetoRana : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadCaida = 3f;

    [Tooltip("Marca esta casilla SOLO si este objeto es un pincho/bomba")]
    public bool esPincho = false;

    void Update()
    {
        // Movimiento simple hacia abajo
        transform.Translate(Vector3.down * velocidadCaida * Time.deltaTime);

        // Si el objeto cae por debajo de la pantalla (ej. Y = -6), se destruye
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
