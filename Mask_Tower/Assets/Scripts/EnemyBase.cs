using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SistemaVida))]
public class EnemyBase : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    protected Rigidbody2D rb;

    [Header("Jugador")]
    protected Transform player;
    public float distanciaDeteccion = 6f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected bool JugadorEnRango()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= distanciaDeteccion;
    }

    protected void MirarAlJugador()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
