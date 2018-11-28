using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleWaypointRecorder : MonoBehaviour {
    private Vehicle vehicle;

    public List<Vector3> recordedCoordinates = new List<Vector3>();
    public List<Vector3> usedCoordinates = new List<Vector3>();
    public float queryTimeCurrent = 0F;
    public float queryTimeMax = 15F;
    public List<WaypointOld> recordedWaypoints = new List<WaypointOld>();

	// Use this for initialization
	void Start () {
        vehicle = GetComponent<Vehicle>();
	}
	
	// Update is called once per frame
	void Update () {
        if (queryTimeCurrent <= 0)
        {
            WaypointOld wp = askNextWaypoint();
            if (wp != null)
            {
                addRecordedWaypoint(wp);
            }
            queryTimeCurrent = queryTimeMax;
        }
        queryTimeCurrent -= Time.deltaTime;

        if (recordedWaypoints.Count > 0 && vehicle.recordedWayPoint != recordedWaypoints[0])
        {
            vehicle.recordedWayPoint = recordedWaypoints[0];
        }
    }

    public void addRecordedWaypoint(WaypointOld waypoint)
    {
        recordedWaypoints.Add(waypoint);
    }

    public bool removeRecordedWaypoint(WaypointOld waypoint)
    {
        return recordedWaypoints.Remove(waypoint);
    }

    public WaypointOld askNextWaypoint()
    {
        //TODO Mudar esta função para realmente contactar o webservice ou outro repositório de dados.
        if (recordedCoordinates.Count > 0)
        {
            Vector3 position = recordedCoordinates[0];
            usedCoordinates.Add(position);
            recordedCoordinates.RemoveAt(0);

            SimulationResources sr = FindObjectOfType<SimulationResources>();
            WaypointOld wp = Instantiate(sr.waypoint_prefab, position, Quaternion.identity);
            wp.GetComponent<Renderer>().material = sr.waypointRecorded_material;
            wp.isRecordedWP = true;
            return wp;
        }
        return null;
    }
}
