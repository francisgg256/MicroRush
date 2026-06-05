using UnityEngine;

public class Frutas : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cesta"))
        {
            // Hemos cambiado FindObjectOfType por FindFirstObjectByType
            FindFirstObjectByType<CestaGameManager>().SumarFruta();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}