using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDatosJuego : MonoBehaviour {
    public int puntuacion;
    public int tiempoEmpleado;
    public bool ganado;

    public int Puntuacion {
        get => puntuacion; set => puntuacion = value;
    }

    public int TiempoEmpleado {
        get => tiempoEmpleado; set => tiempoEmpleado = value;
    }

    public bool Ganado {
        get => ganado; set => ganado = value;
    }

    private void Awake() {
        int numimInstancias = FindObjectsOfType<ControlDatosJuego>().Length;
        if (numimInstancias != 1) {
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
