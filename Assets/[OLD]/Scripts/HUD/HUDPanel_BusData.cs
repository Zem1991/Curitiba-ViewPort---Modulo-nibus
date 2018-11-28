using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel_BusData : MonoBehaviour {
    public Text txt_busName;
    public Text txt_currentPos, txt_waypointPos;
    public Text txt_currentSpeed;

    private HeadsUpDisplay hud;

    // Use this for initialization
    void Start () {
        hud = GetComponentInParent<HeadsUpDisplay>();
    }
	
	// Update is called once per frame
	void Update () {
        Bus b = hud.getSceneManager().getFocusedBus();
        if (b)
        {
            txt_busName.text = "Bus: " + b.vehicleName;
            txt_currentPos.text = "Current position: " + b.transform.position;
            WaypointOld w = b.currentWayPoint;
            if (w)
            {
                txt_waypointPos.text = "Waypoint position: " + b.currentWayPoint.transform.position;
            }
            else
            {
                txt_waypointPos.text = "No waypoint to go";
            }
            float speed = b.currentSpeed * 3.6F;
            txt_currentSpeed.text = "Current speed (KM/H): " + (speed >= 0.01 ? speed : 0);
        }
        else
        {
            txt_busName.text = "No bus found";
            txt_currentPos.text = "";
            txt_waypointPos.text = "";
            txt_currentSpeed.text = "";
        }
    }
}
