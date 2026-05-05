using UnityEngine;
using System.Collections.Generic; 

public class MinijuegoFrutas : MonoBehaviour
{
    public float tiempoRestante = 10f;
    private int frutasTotales = 0;
    private int frutasRecogidas = 0;
    private bool terminado = false;

    private HashSet<GameObject> frutasContadas = new HashSet<GameObject>();

    void Start()
    {
        GameObject[] todasLasFrutas = GameObject.FindGameObjectsWithTag("Frutas");
        frutasTotales = todasLasFrutas.Length;

    }

    void Update()
    {
        if (terminado) return;

        tiempoRestante -= Time.deltaTime;
        if (ControlJuego.instancia != null) ControlJuego.instancia.tiempoMinijuego = tiempoRestante;

        if (tiempoRestante <= 0)
        {
            terminado = true;
            ControlJuego.instancia.perderMinijuego();
        }
    }

    public void FrutaRecogida(GameObject frutaObjeto)
    {
        if (terminado) return;

        if (!frutasContadas.Contains(frutaObjeto))
        {
            frutasContadas.Add(frutaObjeto); 
            frutasRecogidas++;


            if (frutasRecogidas >= frutasTotales)
            {
                terminado = true;
                ControlJuego.instancia.ganarMinijuego();
            }
        }
    }
}
