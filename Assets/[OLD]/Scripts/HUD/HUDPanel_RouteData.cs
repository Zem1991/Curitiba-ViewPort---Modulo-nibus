using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel_RouteData : MonoBehaviour {
    public Text txt_routeName;

    private HeadsUpDisplay hud;

    // Use this for initialization
    void Start () {
        hud = GetComponentInParent<HeadsUpDisplay>();
    }
	
	// Update is called once per frame
	void Update () {
        BusRoute br = hud.getSceneManager().getFocusedBusRoute();
        if (br)
        {
            txt_routeName.text = "Route: " + br.routeName;
        }
    }
}
