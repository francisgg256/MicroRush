using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Súper importante para poder controlar Imágenes de UI

public class MinijuegoMemoria : MonoBehaviour
{
    [Header("Configuración")]
    public Image[] luces; // Aquí arrastraremos los 4 botones: 0=Rojo, 1=Azul, 2=Verde, 3=Amarillo
    public int rondasParaGanar = 4; // Cuántos colores tienes que recordar para ganar
    public float velocidadBrillo = 0.5f;

    private List<int> secuencia = new List<int>();
    private int pasoJugador = 0;
    private bool turnoJugador = false;
    private bool terminado = false;

    void Start()
    {
        // Al empezar, "apagamos" las luces poniéndolas medio transparentes (Alpha a 0.3)
        foreach (Image luz in luces)
        {
            luz.color = new Color(luz.color.r, luz.color.g, luz.color.b, 0.3f);
        }

        // Empezamos la primera ronda tras 1 segundo de pausa
        StartCoroutine(SiguienteRonda());
    }

    IEnumerator SiguienteRonda()
    {
        turnoJugador = false;
        pasoJugador = 0;
        yield return new WaitForSeconds(1f); // Esperamos un poco antes de empezar

        // El juego se inventa un número nuevo del 0 al 3 y lo guarda en su memoria
        secuencia.Add(Random.Range(0, luces.Length));

        // El juego te enseña la secuencia completa hasta ahora
        foreach (int indiceColor in secuencia)
        {
            yield return StartCoroutine(DestelloLuz(indiceColor));
        }

        // Ahora te toca a ti
        turnoJugador = true;
    }

    // Esta es la animación del brillo de los botones
    IEnumerator DestelloLuz(int indice)
    {
        Image luz = luces[indice];

        // ENCENDIDO (Transparencia al 100%)
        luz.color = new Color(luz.color.r, luz.color.g, luz.color.b, 1f);
        yield return new WaitForSeconds(velocidadBrillo);

        // APAGADO (Transparencia al 30%)
        luz.color = new Color(luz.color.r, luz.color.g, luz.color.b, 0.3f);
        yield return new WaitForSeconds(velocidadBrillo / 2f);
    }

    // ESTO ES LO QUE LLAMARÁN LOS BOTONES CUANDO HAGAS CLIC
    public void BotonPulsado(int indicePulsado)
    {
        if (!turnoJugador || terminado) return; // Si no es tu turno, no puedes pulsar

        // Hacemos que brille el botón que acabas de tocar
        StartCoroutine(DestelloLuz(indicePulsado));

        // ¿Has tocado el color correcto?
        if (indicePulsado == secuencia[pasoJugador])
        {
            pasoJugador++; // Acierto. Pasas al siguiente color de la lista

            // ¿Has terminado de repetir toda la secuencia de esta ronda?
            if (pasoJugador >= secuencia.Count)
            {
                if (secuencia.Count >= rondasParaGanar)
                {
                    Ganar();
                }
                else
                {
                    // Si aún no hemos ganado, vamos a la siguiente ronda
                    StartCoroutine(SiguienteRonda());
                }
            }
        }
        else
        {
            // Fallo, tocaste el color equivocado
            Perder();
        }
    }

    void Ganar()
    {
        terminado = true;
        Debug.Log("¡Memoria perfecta! GANASTE");
        if (ControlJuego.instancia != null) ControlJuego.instancia.ganarMinijuego();
    }

    void Perder()
    {
        terminado = true;
        Debug.Log("Error de memoria... PERDISTE");
        if (ControlJuego.instancia != null) ControlJuego.instancia.perderMinijuego();
    }
}
