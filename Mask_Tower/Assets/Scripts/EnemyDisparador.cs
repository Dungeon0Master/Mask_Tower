using UnityEngine;

public class EnemyDisparador : EnemyBase
{
    [Header("Disparo")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public float cooldownDisparo = 2f;

    private bool puedeDisparar = true;

    void Update()
    {
        if (!JugadorEnRango() || !puedeDisparar || player == null)
            return;

        MirarAlJugador();
        Disparar();
    }

    void Disparar()
    {
        puedeDisparar = false;

        if (prefabProyectil != null && puntoDisparo != null)
        {
            GameObject obj = Instantiate(
                prefabProyectil,
                puntoDisparo.position,
                Quaternion.identity
            );

            Proyectil proyectil = obj.GetComponent<Proyectil>();

            if (proyectil != null)
            {
                float dir = Mathf.Sign(player.position.x - transform.position.x);
                proyectil.SetDireccion(new Vector2(dir, 0));
            }
        }

        Invoke(nameof(RecargarDisparo), cooldownDisparo);
    }


    void RecargarDisparo()
    {
        puedeDisparar = true;
    }
}
