using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel_TimeDisplay : MonoBehaviour {
    public Text txt_simulationTime;
    public Text txt_executionTime;

    private HeadsUpDisplay hud;

    // Use this for initialization
    void Start () {
        hud = GetComponentInParent<HeadsUpDisplay>();
	}
	
	// Update is called once per frame
	void Update () {
        txt_simulationTime.text = "Simulation Time = " + hud.getSceneManager().getSimulationTime();
        txt_executionTime.text = "Execution Time = " + hud.getSceneManager().getExecutionTime();
    }
}
