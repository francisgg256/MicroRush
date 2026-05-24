using UnityEngine;

/// <summary>
/// Clase que implementa el paradigma de interacción "Drag and Drop" (Arrastrar y Soltar).
/// Transforma coordenadas de la pantalla (Input) al espacio del mundo 2D, permitiendo 
/// al usuario clasificar objetos manipulándolos de forma directa e intuitiva.
/// </summary>
public class ObjetoArrastrable : MonoBehaviour
{
    [Header("Configuración")]

    /// <summary>
    /// Define la naturaleza lógica del objeto para el sistema de validación.
    /// true = Fruta (Objetivo Positivo); false = Peligro/Basura (Objetivo Negativo).
    /// </summary>
    public bool esFruta = true;

    /// <summary>Referencia al motor de físicas para neutralizar inercias durante el arrastre.</summary>
    private Rigidbody2D rb;

    /// <summary>Memoria de estado espacial. Registra temporalmente la zona de caída sobre la que levita el objeto.</summary>
    private string tagCajaDondeEstoy = "";

    /// <summary>
    /// Método de inicialización. Obtiene las referencias necesarias del motor de físicas.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Evento nativo del sistema de interfaces que se ejecuta continuamente 
    /// mientras el usuario mantiene pulsado el botón del ratón sobre el Collider del objeto.
    /// </summary>
    void OnMouseDrag()
    {
        // Transformación de Coordenadas (UI a Mundo): 
        // Convierte la posición de los píxeles del monitor en coordenadas espaciales dentro de Unity.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Fija la profundidad para evitar que el objeto desaparezca detrás de la cámara 2D
        mousePos.z = 0;

        // Actualiza la posición del objeto en tiempo real para que siga al cursor
        transform.position = mousePos;

        // Anula cualquier fuerza física residual para que el objeto no "tiemble" o caiga mientras se sujeta
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    /// <summary>
    /// Evento nativo que se dispara en el fotograma exacto en el que el usuario libera el botón del ratón.
    /// Actúa como validador de la acción (Drop).
    /// </summary>
    void OnMouseUp()
    {
        // Prevención de Errores: Si el usuario suelta el objeto fuera de una zona válida, la acción se ignora
        if (tagCajaDondeEstoy == "") return;

        // Control de dependencias: Verifica que el gestor lógico esté presente
        if (ManagerClasificador.instancia == null) return;

        // Árbol de decisión y validación de la regla de negocio
        if (esFruta && tagCajaDondeEstoy == "CajaBuena")
            ManagerClasificador.instancia.Acierto();
        else if (!esFruta && tagCajaDondeEstoy == "CajaMala")
            ManagerClasificador.instancia.Acierto();
        else
            ManagerClasificador.instancia.Fallo();

        // Limpieza de memoria: El objeto ha sido procesado y ya no es necesario
        Destroy(gameObject);
    }

    /// <summary>
    /// Detección de entrada en áreas interactivas. Registra cuándo el objeto levita sobre un contenedor válido.
    /// </summary>
    /// <param name="otro">El colisionador del área interceptada.</param>
    void OnTriggerEnter2D(Collider2D otro)
    {
        // Trazabilidad de eventos para facilitar la depuración en fase de desarrollo
        Debug.Log("El objeto " + gameObject.name + " levita sobre: " + otro.gameObject.name + " (Tag: " + otro.tag + ")");

        // Filtrado por etiquetas: Solo memoriza las zonas que forman parte de la mecánica de juego
        if (otro.CompareTag("CajaBuena") || otro.CompareTag("CajaMala"))
        {
            tagCajaDondeEstoy = otro.tag;
        }
    }

    /// <summary>
    /// Detección de salida de áreas interactivas. Limpia la memoria de estado si el usuario arrastra el objeto fuera del contenedor.
    /// </summary>
    /// <param name="otro">El colisionador del área abandonada.</param>
    void OnTriggerExit2D(Collider2D otro)
    {
        // Barrera de seguridad: Garantiza que solo se borra la variable si realmente 
        // estamos saliendo de la caja que habíamos registrado previamente.
        if (tagCajaDondeEstoy != "" && otro.CompareTag(tagCajaDondeEstoy))
        {
            tagCajaDondeEstoy = "";
        }
    }
}
