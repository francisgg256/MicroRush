using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events; // ¡Añadimos esta librería para los eventos!

/// <summary>
/// Gestiona la aparición y desaparición del cartel de instrucciones.
/// Es universal: usa UnityEvents para avisar a cualquier minijuego.
/// </summary>
public class GestorInstrucciones : MonoBehaviour
{
    [Header("Configuración del Cartel")]
    public GameObject panelVisual;
    public TextMeshProUGUI textoUI;
    public string instruccion = "¡ACCIÓN!";
    public float tiempoEnPantalla = 1.5f;

    [Header("Conexión con el Nivel")]
    /// <summary>Evento que se dispara al desaparecer el cartel. Aquí conectaremos el Manager de cada nivel.</summary>
    public UnityEvent alTerminarInstruccion;

    void Start()
    {
        StartCoroutine(SecuenciaInstruccion());
    }

    private IEnumerator SecuenciaInstruccion()
    {
        panelVisual.SetActive(true);
        textoUI.text = instruccion;

        yield return new WaitForSeconds(tiempoEnPantalla);

        panelVisual.SetActive(false);

        // 4. ¡Avisamos al minijuego que sea mediante el evento universal!
        alTerminarInstruccion.Invoke();
    }
}
