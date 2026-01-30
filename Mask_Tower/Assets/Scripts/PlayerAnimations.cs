using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController controller;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        // 1. Velocidad (Para pasar de Idle a Run)
        // Usamos el valor absoluto (siempre positivo) para que funcione a izquierda y derecha
        anim.SetFloat("Velocidad", Mathf.Abs(rb.velocity.x));

        // 2. EnSuelo (Para saber si empezar a saltar o aterrizar)
        anim.SetBool("EnSuelo", controller.enSuelo);

        // 3. VelocidadY (Para diferenciar Salto de Caída)
        // Positivo = Saltando, Negativo = Cayendo
        anim.SetFloat("VelocidadY", rb.velocity.y);
    }
}