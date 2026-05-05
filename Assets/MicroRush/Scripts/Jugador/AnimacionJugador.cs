using UnityEngine;

public class AnimacionJugador : MonoBehaviour
{
    public Animator animacion; 
    public Rigidbody2D fisica; 
    public Transform puntoSuelo; 

    void Update()
    {
        animarJugador(); 
    }

    private void animarJugador()
    {
        if (!tocarSuelo())
        {
            if (fisica.linearVelocity.y < -0.1f) animacion.Play("jugadorCayendo");
            else animacion.Play("jugadorSaltando");
        }
        else
        {
            if (Mathf.Abs(fisica.linearVelocity.x) > 0.1f)
            {
                animacion.Play("jugadorCorriendo");
            }
            else
            {
                animacion.Play("jugadorParado");
            }
        }
    }

    private bool tocarSuelo()
    {
        RaycastHit2D toca = Physics2D.Raycast(puntoSuelo.position, Vector2.down, 0.2f);

        Debug.DrawRay(puntoSuelo.position, Vector2.down * 0.2f, Color.red);

        if (toca.collider != null && !toca.collider.CompareTag("Jugador"))
        {
            return true;
        }

        return false;
    }
}