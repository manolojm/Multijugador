using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ControlJuego : MonoBehaviour
{
    int restantes = 0;

    [Header("Hud")]
    public int tiempoNivel;
    private float tiempoInicio;
    private int tiempoEmpleado;
    public Canvas canvas;
    private ControlHud hud;
    public GameObject victoria;

    // Start is called before the first frame update
    void Start()
    {
        // Asignamos el tiempo y los restantes al canvas
        tiempoInicio = Time.time;

        hud = canvas.GetComponent<ControlHud>();
        hud.SetRestantes(restantes);
    }

    // Update is called once per framexzxzz
    void Update()
    {
        // Actualizamos el tiempo
        tiempoEmpleado = (int)Time.time - (int)tiempoInicio;
        if (tiempoNivel - tiempoEmpleado < 0)
            //Perdido();
            if (restantes <= 0) {
                //Ganado();
            }
        hud.SetTiempo(tiempoEmpleado);
    }

    public void AumentarRestantes() {
        restantes++;
    }

    public void DisminuirRestantes() {
        restantes--;
        hud.SetRestantes(restantes);

        if (restantes <= 0) {
            Ganar();
        }
    }

    public void Ganar() {
        victoria.SetActive(true);
        Debug.Log("Ha ganado la partida");
    }
}
