using UnityEngine;

public class ManagerClasificador : MonoBehaviour
{
    public static ManagerClasificador instancia;

    [Header("Configuración")]
    public int aciertosParaGanar = 5; // Cuántos objetos hay que clasificar bien
    private int aciertosActuales = 0;

    private void Awake()
    {
        instancia = this;
    }

    public void Acierto()
    {
        aciertosActuales++;
        Debug.Log("¡Bien hecho! Llevas: " + aciertosActuales + "/" + aciertosParaGanar);

        if (aciertosActuales >= aciertosParaGanar)
        {
            if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
        }
    }

    public void Fallo()
    {
        Debug.Log("¡Caja equivocada! Perdiste.");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
