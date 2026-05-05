using UnityEngine;

public class ControlFlipJugador : MonoBehaviour
{
    public Rigidbody2D fisica; 
    public SpriteRenderer sprite; 

    void Update()
    {
        if (fisica.linearVelocity.x > 0.1f) sprite.flipX = false;
        else if (fisica.linearVelocity.x < -0.1f) sprite.flipX = true;
    }
}