using UnityEngine;

public class HeartUI : MonoBehaviour
{
    public Animator anim;
    private bool estaVacio = false;

    void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public bool EstaVacio()
    {
        return estaVacio;
    }

    public void PerderVida()
    {
        if (estaVacio) return;

        // Animacin de perder vida
        anim.SetTrigger("PerderVida");
        estaVacio = true;
    }

    // Llamado por EVENTO DE ANIMACIN
    public void SetVacio()
    {
        anim.SetBool("Vacio", true);
    }
}
