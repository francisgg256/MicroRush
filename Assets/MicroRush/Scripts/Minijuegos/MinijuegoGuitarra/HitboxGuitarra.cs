using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detecta si hay notas dentro del recuadro y lee el teclado del jugador.
/// </summary>
public class HitboxGuitarra : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("La tecla física (A,S,W,D) que debe escuchar este recuadro")]
    public KeyCode teclaActuar;

    // Guarda las notas que están pasando por encima ahora mismo
    private List<NotaGuitarra> notasEnZona = new List<NotaGuitarra>();

    void Update()
    {
        if (MinijuegoGuitarra.instancia == null || !MinijuegoGuitarra.instancia.juegoIniciado) return;

        // Limpiamos la lista por si alguna nota se destruyó y se quedó guardada por error
        notasEnZona.RemoveAll(nota => nota == null);

        // Cuando el jugador machaca la tecla correspondiente a esta pista
        if (Input.GetKeyDown(teclaActuar))
        {
            if (notasEnZona.Count > 0)
            {
                // ACIERTO: Destruimos la nota más antigua que entró al recuadro
                NotaGuitarra notaAfectada = notasEnZona[0];

                if (notaAfectada.teclaAsociada == teclaActuar)
                {
                    notaAfectada.Esfumarse();
                    notasEnZona.RemoveAt(0);
                }
            }
            else
            {
                // FALLO: Pulsó la tecla pero el recuadro estaba vacío
                MinijuegoGuitarra.instancia.PerderPorFalloInput();
            }
        }
    }

    // Detectamos cuando la nota TOCA el recuadro
    void OnTriggerEnter2D(Collider2D collision)
    {
        NotaGuitarra nota = collision.GetComponent<NotaGuitarra>();
        if (nota != null && nota.teclaAsociada == teclaActuar)
        {
            notasEnZona.Add(nota);
        }
    }

    // Detectamos cuando la nota SALE del recuadro
    void OnTriggerExit2D(Collider2D collision)
    {
        NotaGuitarra nota = collision.GetComponent<NotaGuitarra>();
        if (nota != null && notasEnZona.Contains(nota))
        {
            notasEnZona.Remove(nota);
        }
    }
}
