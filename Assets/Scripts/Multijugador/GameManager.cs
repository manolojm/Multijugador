using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    string[] jugadores = { "Jugador1", "Jugador2", "Jugador3", "Jugador4", "Jugador5" };
    int random;
    private ControlJuego control;

    // Start is called before the first frame update
    void Start() {

        control = GameObject.Find("SceneManager").GetComponent<ControlJuego>();
        // Creamos los personajes
        if (PhotonNetwork.IsMasterClient) {
            //random = Random.Range(1, 6);
            PhotonNetwork.Instantiate("Player1", new Vector3(0, 2, 0), Quaternion.identity);
        } else {
            random = Random.Range(2, 6);
            PhotonNetwork.Instantiate("Player" + random + "", new Vector3(0, -2, 0), Quaternion.identity);
        }

        // Añadimos un jugador más a la lista de restantes
        control.AumentarRestantes();
    }

    private void Update() {
    }

}
