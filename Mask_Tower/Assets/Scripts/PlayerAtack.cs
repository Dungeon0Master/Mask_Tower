using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    // (Variables igual que antes)
    public GameObject visualAtaque;      
    public float duracionAtaque = 0.15f; 
    public float cooldownAtaque = 0.3f;  

    public bool estaAtacando { get; private set; } // El filtro de seguridad
    private Animator anim; 

    void Start() { anim = GetComponent<Animator>(); }

    public void RealizarAtaque()
    {
        if (!estaAtacando)
        {
            estaAtacando = true; // ACTIVAMOS EL FILTRO
            if(anim != null) anim.SetTrigger("atacar"); 
        }
    }

   
    public void EventoGolpe()
    {
        // VERIFICACIÓN: Solo activamos el hitbox si REALMENTE estamos en modo melee
        if (estaAtacando) 
        {
            StartCoroutine(RutinaHitbox());
        }
    }

    private IEnumerator RutinaHitbox()
    {
        if (visualAtaque != null) visualAtaque.SetActive(true);
        yield return new WaitForSeconds(duracionAtaque);
        if (visualAtaque != null) visualAtaque.SetActive(false);
        yield return new WaitForSeconds(cooldownAtaque);
        estaAtacando = false; // APAGAMOS EL FILTRO
    }
}