using UnityEngine;

public class EnemyVolador : EnemyBase
{
    [Header("Movimiento Volador")]
    public float velocidadMaxima = 4f;
    public float aceleracion = 6f;
    public float desaceleracion = 8f;

    [Header("Límites de Movimiento")]
    public Transform limiteIzquierdo;
    public Transform limiteDerecho;

    private float velocidadActual = 0f;
    private int direccion = 1; // 1 = derecha | -1 = izquierda

    void FixedUpdate()
    {
        if (limiteIzquierdo == null || limiteDerecho == null)
            return;

        // Detectar si llegamos a un límite
        if (direccion == 1 && transform.position.x >= limiteDerecho.position.x)
        {
            direccion = -1;
        }
        else if (direccion == -1 && transform.position.x <= limiteIzquierdo.position.x)
        {
            direccion = 1;
        }

        // Aceleración / desaceleración suave
        float objetivoVelocidad = direccion * velocidadMaxima;

        if (Mathf.Abs(velocidadActual) < Mathf.Abs(objetivoVelocidad))
        {
            velocidadActual = Mathf.MoveTowards(
                velocidadActual,
                objetivoVelocidad,
                aceleracion * Time.fixedDeltaTime
            );
        }
        else
        {
            velocidadActual = Mathf.MoveTowards(
                velocidadActual,
                objetivoVelocidad,
                desaceleracion * Time.fixedDeltaTime
            );
        }

        rb.velocity = new Vector2(velocidadActual, 0);

        // Flip visual
        if (velocidadActual != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(velocidadActual),
                1,
                1
            );
        }
    }

    private void OnDrawGizmos()
    {
        if (limiteIzquierdo == null || limiteDerecho == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(limiteIzquierdo.position, limiteDerecho.position);
    }
}
