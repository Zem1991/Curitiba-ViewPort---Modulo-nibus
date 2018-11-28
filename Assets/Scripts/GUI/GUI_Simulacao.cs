using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Simulacao : MonoBehaviour {
    public GerenciadorDeCena gerenciadorDeCena;
    public Text txtSimulationTime;
    public Text txtExecutionTime;
    public Button btnStop;
    public Button btnPlayPause;
    public Button btnSpeed_0_5;
    public Button btnSpeed_1;
    public Button btnSpeed_2;
    public Button btnSpeed_4;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        txtSimulationTime.text = gerenciadorDeCena.simulationCurrentTime.ToString();
        txtExecutionTime.text = gerenciadorDeCena.executionCurrentTime.ToString();
    }
}
