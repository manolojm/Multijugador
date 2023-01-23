using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.Instantiate("Player1", new Vector3(0, 2, 0), Quaternion.identity);
        } else {
            PhotonNetwork.Instantiate("Player2", new Vector3(0, -2, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
