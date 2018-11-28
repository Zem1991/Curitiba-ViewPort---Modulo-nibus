using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Veiculo : MonoBehaviour {
    public Biarticulado biarticulado;
    public Text txt_idVeiculo;
    public Text txt_velAtual;
    public Text txt_velMaxima;
    public Text txt_status;
    public Text txt_idRota;
    public Text txt_WPProximoPos;
    public Text txt_WPProximoDist;
    public Text txt_WPSeguintePos;
    public Text txt_WPSeguinteDist;

    // Use this for initialization
    void Start () {
        if (!biarticulado)
            biarticulado = FindObjectOfType<Biarticulado>();
    }
	
	// Update is called once per frame
	void Update () {
        txt_idVeiculo.text = biarticulado.nomeDoVeiculo;
        txt_velAtual.text = "Velocidade Atual: " + (int) biarticulado.velocidadeAtual + " km/h";
        txt_velMaxima.text = "Velocidade Máxima: " + (int) biarticulado.velocidadeLimite + " km/h";
        txt_status.text = "Status: " + biarticulado.Status();
        txt_idRota.text = biarticulado.rota.nomeDaRota;
        txt_WPProximoPos.text = (biarticulado.wp_proximo ? biarticulado.wp_proximo.transform.position.ToString() : "Sem Waypoint");
        txt_WPProximoDist.text = (biarticulado.wp_proximo ? biarticulado.wpp_distancia.ToString() : "");
        txt_WPSeguintePos.text = (biarticulado.wp_seguinte ? biarticulado.wp_seguinte.transform.position.ToString() : "Sem Waypoint");
        txt_WPSeguinteDist.text = (biarticulado.wp_seguinte ? biarticulado.wps_distancia.ToString() : "");
    }
}
