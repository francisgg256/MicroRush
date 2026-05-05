using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public int velocidad;
    public Rigidbody2D fisica; 

    private float entradaX;

    void Update()
    {
        entradaX = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        fisica.linearVelocity = new Vector2(entradaX * velocidad, fisica.linearVelocity.y);
    }
}