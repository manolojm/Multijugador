using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class Individual : MonoBehaviourPunCallbacks {
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    override
    public void OnConnectedToMaster() {
        print("conectado");
    }

    public void ButtonConnect() {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom("room1", options, TypedLobby.Default);
        //Empezar();
    }

    override
    public void OnJoinedRoom() {
        Debug.Log("Conectada a la sala " + PhotonNetwork.CurrentRoom.Name);
    }

    private void Update() {

        // Para hacer las pruebas en solitario, igualamos a 1
        //if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            Debug.Log("go");
            PhotonNetwork.LoadLevel(2);
            Destroy(this);
        //}
    }

    private void Empezar() {

        Debug.Log(PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            PhotonNetwork.LoadLevel(3);
            Destroy(this);
        }
    }

}
