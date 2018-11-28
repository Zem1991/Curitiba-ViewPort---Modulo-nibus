using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceGrafica : MonoBehaviour {
    [Header("Painéis")]
    public GUI_Simulacao simulacao;
    public GUI_Iluminacao iluminacao;
    public GUI_Veiculo veiculo;
    public GUI_Rota_WebService rotaWebService;

    [Header("Elementos adicionados dinamicamente")]
    public GUI_Rota_WebService_Dados_WP dadosWPAlcancado;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
