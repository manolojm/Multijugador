using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour
{
    [Header("Personaje")]
    [SerializeField] private float velocidad;
    [SerializeField] private Vector2 direccion;
    private Rigidbody2D rb2D;
    public Animator animacion;

    [Header("Extras")]
    public GameObject pilla;
    public GameObject noPilla;
    public GameObject principal;

    public Boolean esPilla;
    public Boolean esPricipal;

    private ControlJuego control;

    private void Start() {
        control = GameObject.Find("SceneManager").GetComponent<ControlJuego>();
        if (GetComponent<PhotonView>().IsMine) {
            rb2D = GetComponent<Rigidbody2D>();
            principal.SetActive(true);
        }
    }

    private void Update() {
        if (GetComponent<PhotonView>().IsMine) {
            direccion = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            // FlipxX
            if (rb2D.velocity.x > 0.1f) {
                GetComponent<SpriteRenderer>().flipX = false;
            } else {
                if (rb2D.velocity.x < -0.1f) {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }

        //animacion.SetFloat("velocityX", Mathf.Abs(rb2D.velocity.x));
        //animacion.SetFloat("velocityY", rb2D.velocity.y);
    }

    private void FixedUpdate() {
        if (GetComponent<PhotonView>().IsMine) {
            rb2D.MovePosition(rb2D.position + direccion * velocidad * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        // Solo cuenta si uno pilla y el otro no pilla
        if (collision.gameObject.CompareTag("Player") && 
            (esPilla != collision.gameObject.GetComponent<Movimiento>().esPilla)) {

            // Solo envía la señal el que le han pillado para que no sea doble
            if (!esPilla) {
                MeHaPillado();
            }
        }
    }

    private void MeHaPillado() {
        // Actualizamos el personaje
        noPilla.SetActive(false);
        pilla.SetActive(true);
        esPilla = true;

        // Uno menos restante
        control.DisminuirRestantes();
    }

}
