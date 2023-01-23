using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class Connection : MonoBehaviourPunCallbacks {

    #region Variables Públicas

    [Header("Paneles")]
    public GameObject panelInicio;    // Pantalla de inicio
    public GameObject panelPrincipal; // Pantalla de inicio multijugador
    public GameObject panelCrearSala; // Panel cuando un jugador quiere crear una sala nueva
    public GameObject panelUnirSala;  // Panel para unirse a una partica creada
    public GameObject panelDeSala;    // Panel que muestra el contenido    
    //public GameObject panelAvatar;

    [Header("Botones")]
    public Button btnCrearSala;
    public Button btnUnirSala;
    public Button btnIniciarPartida;

    [Header("InputField")]
    public TMP_InputField inputNickName;
    public TMP_InputField inputMaxPlayers;
    public TMP_InputField inputNombreSala;
    public Toggle chkPrivada;
    public TMP_InputField inputNombreSalaAUnir;

    [Header("Textos")]
    public TMP_Text txtBienvenida;
    public TMP_Text txtCapacidad;
    public TMP_Text txtNombreSala;
    public TMP_Text txtEstado;
    public TMP_Text txtAvatar;

    [Header("ListaJugadores")]
    public GameObject contenedorJugadores;
    public GameObject elemJugador;
    public int avatarSeleccionado;

    [Header("ListaSalas")]
    public GameObject contenedorSalas;
    public GameObject elemSala;

    #endregion

    #region Variables Privadas

    Dictionary<string, RoomInfo> listaSalas;
    ExitGames.Client.Photon.Hashtable propiedadesJugador;

    static string[] listaAvatar = { "Frog", "VirtualGuy" };

    #endregion

    private void Start() {
        // Mostramos el panel de inicio.
        CambiarPanel(panelInicio);

        // Declaramos listas para jugadores y salas.
        propiedadesJugador = new ExitGames.Client.Photon.Hashtable();
        avatarSeleccionado = -1;
        listaSalas = new Dictionary<string, RoomInfo>();

        // Conectamos al servidor utilizando la configuración del proyecto.
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    #region Métodos de Photon

    override
    public void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
        Estado("Conectado al servidor");
    }


    override
    public void OnJoinedRoom() {
        Estado("Conectado a la sala " + PhotonNetwork.CurrentRoom.Name);

        // Almacenamos en las propiedades del jugador el avatar seleccionado.
        propiedadesJugador["avatar"] = -1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(propiedadesJugador);
        CambiarPanel(panelDeSala);
        ActualizarPanelDeJugadores();
        Estado("Te has unido a la sala " + PhotonNetwork.CurrentRoom.Name);
    }

    override
    public void OnJoinRoomFailed(short returnCode, string message) {
        Estado("No ha sido posible conectar a la sala: " + message);
    }

    override
    public void OnCreateRoomFailed(short returnCode, string message) {
        Estado("No ha sido posible crear la sala: " + message);
    }

    override
    public void OnPlayerEnteredRoom(Player newPlayer) {
        ActualizarPanelDeJugadores();
    }

    override
    public void OnPlayerLeftRoom(Player otherPlayer) {
        ActualizarPanelDeJugadores();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomlist) {
        // Comprobar los elemenos uno por uno de lista roomlist
        // Si es un elemento borrado, lo buscamos en nuestra lista y lo borramos.
        // Si es un elemento nuevo, lo añadimos.
        // Si es un elemento modificado, machacamos su información.

        foreach (RoomInfo r in roomlist) {
            if (r.RemovedFromList || !r.IsOpen || r.IsVisible) {

                listaSalas.Remove(r.Name);
            }

            if (listaSalas.ContainsKey(r.Name)) {
                if (r.PlayerCount > 0)
                    listaSalas[r.Name] = r;
                else  // Lo podemos comentar si no queremos borrar salas vacías.
                    listaSalas.Remove(r.Name);
            } else {
                listaSalas.Add(r.Name, r);
            }

        }

        ActualizarPanelDeSalas();
    }

    #endregion

    #region Métodos Propios

    public void Estado(string texto) {
        txtEstado.text = texto;
    }

    public void AbandonarSala() {
        PhotonNetwork.LeaveRoom();
        CambiarPanel(panelPrincipal);
    }

    public void ComprobarNombre() {
        if (!inputNickName.text.Contains(' ') && !String.IsNullOrEmpty(inputNickName.text)) {
            btnCrearSala.interactable = true;
            btnUnirSala.interactable = true;
        } else {
            btnCrearSala.interactable = false;
            btnUnirSala.interactable = false;
        }
    }

    public void CambiarPanel(GameObject nombre) {
        // Desactivamos todos los paneles
        panelInicio.SetActive(false);
        panelCrearSala.SetActive(false);
        panelPrincipal.SetActive(false);
        panelUnirSala.SetActive(false);
        panelDeSala.SetActive(false);
        //panelAvatar.SetActive(false);

        // Activamos el panel correcto.
        nombre.SetActive(true);
        Debug.Log("Cambiado el panel");
    }

    public void ActualizarPanelDeJugadores() {
        // Editamos mensajes del Panel
        txtNombreSala.SetText("Sala " + PhotonNetwork.CurrentRoom.Name);
        txtCapacidad.text = "Capacidad: " + PhotonNetwork.CurrentRoom.PlayerCount +
                        "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Eliminamor todos los jugadores de la lista para empezar desde 0
        while (contenedorJugadores.transform.childCount > 0) {
            DestroyImmediate(contenedorJugadores.transform.GetChild(0).gameObject);
        }

        foreach (Player jugador in PhotonNetwork.PlayerList) {
            // Instanciamos un nuevo boton y los colgamos en el contenedor
            GameObject nuevoElemento = Instantiate(elemJugador);
            nuevoElemento.transform.SetParent(contenedorJugadores.transform, false);

            // Localizamos y actualizamos etiquetas
            nuevoElemento.transform.Find("txtNickName").GetComponent<TextMeshProUGUI>().text = jugador.NickName;
            // Obtenemos el avatar
            string avatar;
            object avatarJugador = jugador.CustomProperties["avatar"];

            if ((int)avatarJugador < 0) {
                avatar = "Selecciona Avatar";
            } else {
                avatar = listaAvatar[(int)avatarJugador];
            }

            nuevoElemento.transform.Find("txtAvatar").GetComponent<TextMeshProUGUI>().text = avatar;

        }

        if (avatarSeleccionado > -1 && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers
            && PhotonNetwork.IsMasterClient) {
            btnIniciarPartida.interactable = true;
        } else {
            btnIniciarPartida.interactable = false;
            Estado("Esparando que el master client inice la partida");
        }


    }
    public void ActualizarPanelDeSalas() {
        while (contenedorSalas.transform.childCount > 0) {
            DestroyImmediate(contenedorSalas.transform.GetChild(0).gameObject);
        }

        foreach (RoomInfo r in listaSalas.Values) {
            // Creamos un nuevo botón y lo añadimos al contenedor
            GameObject nuevoElemento = Instantiate(elemSala);
            nuevoElemento.transform.SetParent(contenedorSalas.transform, false);

            // Localizamos las etiquetas y las actualizamos
            nuevoElemento.transform.Find("txtNombreSala").GetComponent<TextMeshProUGUI>().text = r.Name;
            nuevoElemento.transform.Find("txtCpacidadSala").GetComponent<TextMeshProUGUI>().text =
                r.PlayerCount + " / " + r.MaxPlayers;

            //Añadimos la acción de selección en la lista.
            nuevoElemento.GetComponent<Button>().onClick.AddListener(() => { AlPulsarBtnUnirseASalaDeLista(r.Name); });
        }
    }

    #endregion

    #region Métodos de Botones

    #region Botones Panel Principal
    public void AlPulsarBtnCearPartidaNuevaPP() {
        CambiarPanel(panelCrearSala);
        txtBienvenida.text = "Bienvenido " + inputNickName.text;
        PhotonNetwork.NickName = inputNickName.text;

        Estado("Creando nueva partida...");
    }

    public void AlPulsarBtnUnirseAUnaSalaPP() {
        CambiarPanel(panelUnirSala);
        PhotonNetwork.NickName = inputNickName.text;
    }

    #endregion

    #region Botones Panel Partida Nueva
    public void AlPulsarBtnCrearSala() {
        byte maxJugadores;

        if (!String.IsNullOrEmpty(inputMaxPlayers.text)) {
            maxJugadores = byte.Parse(inputMaxPlayers.text);
            if (!String.IsNullOrEmpty(inputNombreSala.text) && (maxJugadores > 1) && (maxJugadores <= 20)) {
                RoomOptions opcionesDeSala = new RoomOptions();
                opcionesDeSala.MaxPlayers = maxJugadores;
                opcionesDeSala.IsVisible = !chkPrivada.isOn; // Es visible si no es privada

                PhotonNetwork.CreateRoom(inputNombreSala.text, opcionesDeSala, TypedLobby.Default);
            } else {
                Estado("Opciones de sala incorrectas");
            }
        } else {
            Estado("Número de Jugadores incorrecto");
        }
    }

    #endregion

    #region Botones Panel de Sala
    public void AlPulsarIniciarPartida() {
        PhotonNetwork.LoadLevel(1);
        Destroy(this);
    }

    public void AlPulsarBtnUnirseASalaDeLista(string nombreSala) {
        PhotonNetwork.JoinRoom(nombreSala);
    }

    public void AlPulsarBtnUnirseASala() {
        if (!String.IsNullOrEmpty(inputNombreSalaAUnir.text)) {
            PhotonNetwork.JoinRoom(inputNombreSalaAUnir.text);
            CambiarPanel(panelDeSala);
        } else {
            Debug.Log("Introduzca un nombre correcto para la sala");
        }
    }

    #endregion

    public void AlPulsarBtnAvatar(string nombre) {
        switch (nombre) {
            case "Frog":
                avatarSeleccionado = 0;
                break;

            case "VirtualGuy":
                avatarSeleccionado = 1;
                break;
        }
        txtAvatar.text = nombre;
    }

    public void AlPulsarBtnAceptarAvatar() {
        // Almacenamos en las propiedades del jugador el avatar seleccionado.
        propiedadesJugador["avatar"] = avatarSeleccionado;
        PhotonNetwork.LocalPlayer.SetCustomProperties(propiedadesJugador);

        // Actualizamos panel de jugadores
        ActualizarPanelDeJugadores();

        // Mensaje depuración
        Estado("Avatar Seleccionado: " + listaAvatar[avatarSeleccionado]);
    }

    #endregion

}
