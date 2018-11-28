using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel_ObstacleData : MonoBehaviour {
    public Text txt_obstacleName;
    public Text txt_obstaclePos;
    public Text txt_obstacleStatus;
    public Text txt_dotDirections;
    public Text txt_dotPositions;

    private HeadsUpDisplay hud;

    // Use this for initialization
    void Start () {
        hud = GetComponentInParent<HeadsUpDisplay>();
    }
	
	// Update is called once per frame
	void Update () {
        Bus bus = hud.getSceneManager().getFocusedBus();
        GameObject obstacle;
        float dotDirections;
        float dotPositions;
        if (bus && bus.GUI_GetObstacleData(out obstacle, out dotDirections, out dotPositions))
        {
            string status = "";
            if (obstacle.GetComponent<Semaphore>())
            {
                status += "Status: ";
                status += (obstacle.GetComponent<Semaphore>().isOpen ? "Open" : "Closed");
            }
            txt_obstacleName.text = "Obstacle: " + obstacle.name;
            txt_obstaclePos.text = "Position: " + obstacle.transform.position;
            txt_obstacleStatus.text = status;
            txt_dotDirections.text = "Dot Directions: " + dotDirections;
            txt_dotPositions.text = "Dot Positions: " + dotPositions;
        }
        else
        {
            txt_obstacleName.text = "No obstacle found";
            txt_obstaclePos.text = "";
            txt_obstacleStatus.text = "";
            txt_dotDirections.text = "";
            txt_dotPositions.text = "";
        }
    }
}
