using UnityEngine;

public class SaltoJugador : MonoBehaviour
{
    public int fuerzaSalto;
    public Rigidbody2D fisica; 
    public Transform puntoSuelo; 

    private bool entradaSalto;

    private void FixedUpdate()
    {
        if (entradaSalto)
        {
            fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            entradaSalto = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && tocarSuelo())
        {
            entradaSalto = true;
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