using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemigo : MonoBehaviour {
    public Animator animator;
    public SpriteRenderer sprite;

    public float speed = 2f;
    private float waitTime;
    public float starWaitTime = 2;

    private int algo = 0;
    private Vector2 posicionActual;

    public Transform[] puntos;

    // Start is called before the first frame update
    void Start() {
        waitTime = starWaitTime;
    }

    // Update is called once per frame
    void Update() {

        StartCoroutine(CheckEnemyMoving());
        transform.position = Vector2.MoveTowards(transform.position, puntos[algo].transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntos[algo].transform.position) < 0.1f) {
            if (waitTime <= 0) {

                if (puntos[algo] != puntos[puntos.Length - 1]) {

                    algo++;
                } else {
                    algo = 0;
                }

                waitTime = starWaitTime;
            } else {
                waitTime -= Time.deltaTime;
            }
        }
    }

    IEnumerator CheckEnemyMoving() {
        posicionActual = transform.position;
        yield return new WaitForSeconds(0.5f);

        if (transform.position.x > posicionActual.x) {
            sprite.flipX = true;
            animator.SetBool("Idle", false);
        } else {
            if (transform.position.x < posicionActual.x) {
                sprite.flipX = false;
                animator.SetBool("Idle", false);
            } else {
                animator.SetBool("Idle", true);
            }
        }
    }
}
