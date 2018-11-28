using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusWaypointLoader : MonoBehaviour {
    public List<Vector3> preLoadedWaypoints;
    public List<WaypointOld> recordedWaypoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool addRecordedWaypoint(Vector3 position)
    {
        SimulationResources sr = FindObjectOfType<SimulationResources>();
        WaypointOld wp = Instantiate(sr.waypoint_prefab, position, Quaternion.identity);
        wp.GetComponent<Renderer>().material = sr.waypointRecorded_material;
        recordedWaypoints.Add(wp);
        return true;
    }

    public bool removeRecordedWaypoint(WaypointOld waypoint)
    {
        return recordedWaypoints.Remove(waypoint);
    }
}
