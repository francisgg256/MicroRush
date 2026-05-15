using UnityEngine;

public class ObjetoArrastrable : MonoBehaviour
{
    [Header("Configuración")]
    public bool esFruta = true; // Marca esto en la fresa, desmárcalo en los pinchos

    private Rigidbody2D rb;
    private string tagCajaDondeEstoy = "";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Mientras arrastras con el ratón
    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    // Al soltar el ratón
    void OnMouseUp()
    {
        if (tagCajaDondeEstoy == "") return; // Si lo sueltas en el aire, no pasa nada

        if (ManagerClasificador.instancia == null) return;

        // Lógica de ganar o perder
        if (esFruta && tagCajaDondeEstoy == "CajaBuena") ManagerClasificador.instancia.Acierto();
        else if (!esFruta && tagCajaDondeEstoy == "CajaMala") ManagerClasificador.instancia.Acierto();
        else ManagerClasificador.instancia.Fallo();

        Destroy(gameObject);
    }

    // Detectar si el objeto está encima de una caja
    void OnTriggerEnter2D(Collider2D otro)
    {
        // ---> ¡AQUÍ ESTÁ EL CHIVATO! <---
        Debug.Log("El objeto " + gameObject.name + " tocó a: " + otro.gameObject.name + " (Tag: " + otro.tag + ")");

        if (otro.CompareTag("CajaBuena") || otro.CompareTag("CajaMala"))
        {
            tagCajaDondeEstoy = otro.tag;
        }
    }

    void OnTriggerExit2D(Collider2D otro)
    {
        // Añadimos esta barrera: Si la variable está vacía, no hacemos nada.
        if (tagCajaDondeEstoy != "" && otro.CompareTag(tagCajaDondeEstoy))
        {
            tagCajaDondeEstoy = "";
        }
    }
}
