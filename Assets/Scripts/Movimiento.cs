using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector2 direccion;
    private Rigidbody2D rb2D;

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
    }

    private void FixedUpdate() {
        if (GetComponent<PhotonView>().IsMine) {
            rb2D.MovePosition(rb2D.position + direccion * velocidad * Time.fixedDeltaTime);
        }
    }
}
