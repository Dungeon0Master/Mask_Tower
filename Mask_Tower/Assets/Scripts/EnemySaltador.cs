using UnityEngine;

public class EnemySaltador : EnemyBase
{
    [Header("Salto")]
    public float fuerzaSalto = 7f;
    public float fuerzaHorizontal = 4f;
    public float cooldownSalto = 2f;

    private bool puedeSaltar = true;

    void Update()
    {
        if (!JugadorEnRango() || !puedeSaltar || player == null)
            return;

        MirarAlJugador();
        SaltarHaciaJugador();
    }

    void SaltarHaciaJugador()
    {
        puedeSaltar = false;

        // Reset de velocidad para salto limpio
        rb.velocity = Vector2.zero;

        float direccion = Mathf.Sign(player.position.x - transform.position.x);

        Vector2 fuerza = new Vector2(direccion * fuerzaHorizontal, fuerzaSalto);
        rb.AddForce(fuerza, ForceMode2D.Impulse);

        Invoke(nameof(RecargarSalto), cooldownSalto);
    }

    void RecargarSalto()
    {
        puedeSaltar = true;
    }
}
