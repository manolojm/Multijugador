using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    string[] jugadores = { "Jugador1", "Jugador2", "Jugador3", "Jugador4", "Jugador5" };
    int random;

    // Start is called before the first frame update
    void Start() {

        if (PhotonNetwork.IsMasterClient) {
            random = Random.Range(1, 6);
            PhotonNetwork.Instantiate("Player" + random + "", new Vector3(0, 2, 0), Quaternion.identity);
        } else {
            random = Random.Range(1, 6);
            PhotonNetwork.Instantiate("Player" + random + "", new Vector3(0, -2, 0), Quaternion.identity);
        }
    }
}
