using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlHud : MonoBehaviour {
    public TextMeshProUGUI restantesTxt;
    public TextMeshProUGUI tiempoTxt;

    public void SetRestantes(int vidas) {
        restantesTxt.text = "Restantes: " + vidas;
    }

    public void SetTiempo(int tiempo) {

        int segundos = tiempo % 60;
        int minutos = tiempo / 60;

        tiempoTxt.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

}
