using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

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
    //public GameObject principal;

    public Boolean esPilla;
    public Boolean esPricipal;

    private int restantes = 4;
    private Boolean ganado = false;

    private void Start() {
        if (GetComponent<PhotonView>().IsMine) {
            rb2D = GetComponent<Rigidbody2D>();
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

        animacion.SetFloat("velocityX", Mathf.Abs(rb2D.velocity.x));
        animacion.SetFloat("velocityY", rb2D.velocity.y);
    }

    private void FixedUpdate() {
        if (GetComponent<PhotonView>().IsMine) {
            if (esPricipal){
                rb2D.MovePosition(rb2D.position + direccion * velocidad * Time.fixedDeltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            MeHaPillado();
            restantes--;

            if (restantes <= 0) {
                Ganar();
            }
        }
    }

    private void MeHaPillado() {
        noPilla.SetActive(false);
        pilla.SetActive(true);
        esPilla = true;
    }

    private void Ganar() {
        Debug.Log("Ha ganado la partida");
    }
}
