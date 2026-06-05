using UnityEngine;

public class CestaGameManager : MonoBehaviour
{
    public GameObject frutaPrefab;
    public Transform[] puntosGeneracion; // Crea varios objetos vacíos arriba de la pantalla

    public float tiempoRestante = 7f;
    private int frutasRecogidas = 0;
    private float tiempoSpawn = 0f;

    void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            tiempoSpawn -= Time.deltaTime;

            if (tiempoSpawn <= 0)
            {
                Instantiate(frutaPrefab, puntosGeneracion[Random.Range(0, puntosGeneracion.Length)].position, Quaternion.identity);
                tiempoSpawn = 0.8f; // Velocidad de caída
            }
        }
        else
        {
            // Fin del juego
            if (frutasRecogidas >= 3)
            {
                if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
            }
            else
            {
                if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
            }
        }
    }

    public void SumarFruta()
    {
        frutasRecogidas++;
    }
}
