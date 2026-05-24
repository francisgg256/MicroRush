using UnityEngine;

/// <summary>
/// Clase responsable de gestionar los controles, las físicas y las animaciones del jugador 
/// en el minijuego de "Luz Roja, Luz Verde".
/// Interactúa directamente con el estado global del semáforo para validar la legalidad de los movimientos.
/// </summary>
public class JugadorSemaforo : MonoBehaviour
{
    [Header("Configuración")]

    /// <summary>Velocidad de desplazamiento del personaje en unidades de Unity por segundo.</summary>
    public float velocidad = 5f;

    /// <summary>
    /// Referencia espacial (Transform) que representa la línea de meta. 
    /// Si la coordenada X del jugador supera este punto, se registra la victoria.
    /// </summary>
    public Transform meta;

    [Header("Componentes")]

    /// <summary>Referencia al motor de físicas 2D para aplicar movimiento sin atravesar colisionadores.</summary>
    public Rigidbody2D rb;

    /// <summary>Referencia al renderizador visual para gestionar la orientación (Flip) del personaje.</summary>
    public SpriteRenderer sprite;

    /// <summary>Referencia al controlador de animaciones para cambiar entre estados (Parado/Corriendo).</summary>
    public Animator anim;

    /// <summary>
    /// Bucle principal del jugador. 
    /// Procesa la entrada del usuario, consulta el estado de la Máquina de Estados (Semaforo),
    /// aplica las fuerzas físicas y evalúa la condición de victoria espacial.
    /// </summary>
    void Update()
    {
        // Programación defensiva: Evita errores críticos si falta el componente de físicas
        if (rb == null) return;

        // Captura de entradas nativas de Unity (Input System) sin suavizado (GetAxisRaw)
        // para garantizar una respuesta de control táctil y precisa (estilo Arcade).
        float movH = Input.GetAxisRaw("Horizontal");
        float movV = Input.GetAxisRaw("Vertical");
        bool moviendoEspacio = Input.GetKey(KeyCode.Space);

        // Evaluación booleana: Determina si el jugador está enviando alguna orden de movimiento
        bool intentandoMoverse = Mathf.Abs(movH) > 0.1f || Mathf.Abs(movV) > 0.1f || moviendoEspacio;

        // Vector base inicializado a cero (reposo)
        Vector2 velocidadFinal = Vector2.zero;

        if (intentandoMoverse)
        {
            // REGLA DE ORO (Validación de Estado): 
            // Consulta la instancia del semáforo. Si el estado es 2 (Rojo), el movimiento es una infracción.
            if (MinijuegoSemaforo.instancia.estadoSemaforo == 2)
            {
                Debug.Log("Infracción detectada: Movimiento durante la fase de Luz Roja.");

                // Frena en seco al personaje reseteando su inercia
                rb.linearVelocity = Vector2.zero;

                // Feedback visual inmediato: Fuerza la animación de reposo
                if (anim != null) anim.Play("jugadorParado");

                // Notifica la derrota al gestor del minijuego y corta la ejecución del frame
                MinijuegoSemaforo.instancia.Perder();
                return;
            }

            // Lógica de movimiento permitido (Estados 0: Verde o 1: Amarillo)
            if (moviendoEspacio)
            {
                // Avance rápido en línea recta mediante la barra espaciadora
                velocidadFinal = new Vector2(velocidad, 0);
            }
            else
            {
                // Movimiento libre. Se aplica ".normalized" para evitar que el movimiento diagonal
                // sume los vectores y resulte en una velocidad matemáticamente superior a la recta.
                velocidadFinal = new Vector2(movH, movV).normalized * velocidad;
            }
        }

        // Aplicación del cálculo al motor de físicas
        rb.linearVelocity = velocidadFinal;

        // Delegación de responsabilidades: Actualiza la interfaz visual del personaje
        ActualizarVisuales(movH, velocidadFinal.magnitude);

        // Condición de Victoria Espacial: El jugador cruza el umbral de la meta
        if (meta != null && rb.position.x >= meta.position.x)
        {
            MinijuegoSemaforo.instancia.Ganar();
        }
    }

    /// <summary>
    /// Método auxiliar que separa la lógica visual de la lógica matemática.
    /// Actualiza la dirección del sprite y la máquina de estados de las animaciones.
    /// </summary>
    /// <param name="movH">Valor de entrada horizontal para determinar la orientación izquierda/derecha.</param>
    /// <param name="rapidez">Magnitud de la velocidad actual para determinar si debe reproducirse la animación de carrera.</param>
    void ActualizarVisuales(float movH, float rapidez)
    {
        // Volteo del sprite (Flip) sin necesidad de rotar el objeto 3D
        if (movH > 0.1f) sprite.flipX = false;
        else if (movH < -0.1f) sprite.flipX = true;

        if (anim != null)
        {
            // Transición de animaciones basada en la inercia real del personaje
            if (rapidez > 0.1f)
            {
                anim.Play("jugadorCorriendo");
            }
            else
            {
                anim.Play("jugadorParado");
            }
        }
    }
}
