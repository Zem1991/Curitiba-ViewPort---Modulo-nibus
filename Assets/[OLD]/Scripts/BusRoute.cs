using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusRoute : MonoBehaviour {
    public string routeName;
    public List<WaypointOld> waypoints = new List<WaypointOld>();
    public List<Bus> buses = new List<Bus>();

    // Use this for initialization
    void Start () {
        if (waypoints.Count == 0)
            waypoints.AddRange(GetComponentsInChildren<WaypointOld>());
        if (buses.Count == 0)
            buses.AddRange(GetComponentsInChildren<Bus>());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addWaypoint(WaypointOld w)
    {
        waypoints.Add(w);
        w.busRoute = this;
    }

    public void addBus(Bus b)
    {
        buses.Add(b);
        b.busRoute = this;
    }

    public WaypointOld getNextWayPoint(WaypointOld current)
    {
        if(waypoints.Count > 1)
        {
            if (current)
            {
                foreach (WaypointOld wp in waypoints)
                {
                    if (current == wp)
                    {
                        int aux = waypoints.IndexOf(wp);
                        if (aux + 1 >= waypoints.Count)
                            aux = 0;    //Volta para o primeiro waypoint da lista.
                        else
                            aux += 1;   //Vai para o próximo waypoint da lista.
                        return waypoints[aux];
                    }
                }
            }
            //By default, return the first Waypoint
            return waypoints[0];
        }
        else
        {
            return null;
        }
    }

    public void recolorWaypoints(Material waypoint, Material stoppoint)
    {
        foreach (WaypointOld item in waypoints)
        {
            if (!item.isStopPoint)
            {
                item.changeMaterial(waypoint);
            }
            else
            {
                item.changeMaterial(stoppoint);
            }
        }
    }
}
