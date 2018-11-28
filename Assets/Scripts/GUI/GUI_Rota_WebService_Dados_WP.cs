using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Rota_WebService_Dados_WP : MonoBehaviour {
    public Text txt_identificacao;
    public Text txt_posOriginal;
    public Text txt_posAjustada;
    public Text txt_horaGerado;
    public Text txt_horaAlcancado;
    public Text txt_diferenca;
    public Rota_Waypoint waypoint;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AjustarTextos(bool pontoAlcancado, int id, Vector3 posOrigGPS, Vector3 posOrigXYZ, Vector3 posAjustXYZ, 
        string horaParaAlcancar, string horaAlcancado, float diferença)
    {
        txt_identificacao.text = "Waypoint nº " + id;
        txt_posOriginal.text = "Pos. Original: " + posOrigXYZ + " (GPS " + posOrigGPS + ")";
        txt_posAjustada.text = "Pos. Ajustada: " + posAjustXYZ;
        txt_horaGerado.text = "Horário a Alcançar: " + horaParaAlcancar;
        if (pontoAlcancado)
        {
            txt_horaAlcancado.text = "Horário Alcançado: " + horaAlcancado;
            txt_diferenca.text = "Diferença: " + diferença + " segundos";
        }
        else
        {
            txt_horaAlcancado.text = "Horário Alcançado: pendente";
            txt_diferenca.text = "Diferença: pendente";
        }
    }
}
