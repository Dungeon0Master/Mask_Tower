using UnityEngine;

public class EnemyPatrullero : EnemyBase
{
    [Header("Patrulla")]
    public float velocidadPatrulla = 2f;
    public Transform detectorPared;
    public float distanciaPared = 0.3f;
    public LayerMask layerObstaculos;

    private int direccion = 1;

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direccion * velocidadPatrulla, rb.velocity.y);

        RaycastHit2D hit = Physics2D.Raycast(
            detectorPared.position,
            Vector2.right * direccion,
            distanciaPared,
            layerObstaculos
        );

        if (hit.collider != null)
        {
            CambiarDireccion();
        }
    }

    void CambiarDireccion()
    {
        direccion *= -1;

        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * direccion;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        if (detectorPared == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            detectorPared.position,
            detectorPared.position + Vector3.right * direccion * distanciaPared
        );
    }
}
