using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointOld : MonoBehaviour {
    public bool isStopPoint = false;
    public bool isRecordedWP = false;
    public BusRoute busRoute;

	// Use this for initialization
	void Start () {
        busRoute = GetComponentInParent<BusRoute>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Vehicle vehicle = other.gameObject.GetComponent<Vehicle>();
        Bus_Wrapper busW = other.gameObject.GetComponent<Bus_Wrapper>();
        if (!vehicle && busW)
        {
            vehicle = busW.getBus();
        }

        if (vehicle && (vehicle.currentWayPoint == this || vehicle.recordedWayPoint == this))
        {
            if (isStopPoint && vehicle as Bus)
            {
                (vehicle as Bus).Bus_StopMovement();
            }
            else
            {
                vehicle.defineNextWaypoints(isRecordedWP);
            }
        }
    }

    public void changeMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }
}
