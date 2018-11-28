using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rota : MonoBehaviour {
    public string nomeDaRota;
    public List<Rota_Waypoint> waypoints = new List<Rota_Waypoint>();
    public bool waypointsEmCiclo;

	// Use this for initialization
	void Start () {
        CarregaWaypoints();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CarregaWaypoints()
    {
        if (waypoints.Count <= 0)
            waypoints.AddRange(GetComponentsInChildren<Rota_Waypoint>());
    }

    public Rota_Waypoint ProximoWaypoint(Rota_Waypoint atual)
    {
        if (!atual)
        {
            CarregaWaypoints();
            return waypoints[0];
        }

        int aux = waypoints.IndexOf(atual);
        if (aux + 1 >= waypoints.Count)
            if (waypointsEmCiclo)
                return waypoints[0];
            else
                return null;
        else
            return waypoints[aux + 1];
    }
}
