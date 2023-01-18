using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        if (PhotonNetwork.IsMasterClient) {
            //PhotonNetwork.Instantiate("Frog", new Vector3(1, 0.2f, 0), Quaternion.identity);
        } else {
            //PhotonNetwork.Instantiate("VirtualGuy", new Vector3(0.5f, 0.2f, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
