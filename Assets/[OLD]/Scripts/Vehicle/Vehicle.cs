using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {
    public Bounds bounds;
    public VehicleAutomatedController vehicleAutoCon;
    public VehicleWaypointRecorder waypointRecorder;

    [Header("Static values")]
    public string vehicleName;
    public float maneuverSpeed;
    public float maximumSpeed;
    public float maxMotorTorque;            // maximum torque the motor can apply to wheel
    public float maxSteeringAngle;          // maximum steer angle the wheel can have
    public List<VehicleAxle> vehicleAxles;  // the information about each individual axle

    [Header("Dynamic values")]
    public float currentSpeed;
    public float motor;
    public float steering;
    public bool almostStopped;
    public bool keepStopped = false;
    public bool canRunSimulation = true;

    [Header("Other known objects")]
    public BusRoute busRoute;
    public WaypointOld recordedWayPoint;
    public WaypointOld currentWayPoint;
    public WaypointOld nextWayPoint;

    // Use this for initialization
    public virtual void Start () {
        vehicleAutoCon = GetComponent<VehicleAutomatedController>();
        waypointRecorder = GetComponent<VehicleWaypointRecorder>();

        Start_DefineBounds();
        Start_InitializeRoute();
    }

    // Update is called once per frame
    public virtual void Update () {
        Update_CheckVariables();
        Update_CheckWaypoints();
    }

    public virtual void FixedUpdate ()
    {
        FixedUpdate_VehicleMovement();
        FixedUpdate_CheckVehicleAxles();
    }

    private void Start_DefineBounds()
    {
        bounds = new Bounds(transform.position, Vector3.zero);
        if (GetComponent<Renderer>())
            bounds.Encapsulate(GetComponent<Renderer>().bounds);
        foreach (Renderer item in GetComponentsInChildren<Renderer>())
            bounds.Encapsulate(item.bounds);
    }

    private void Start_InitializeRoute()
    {
        if (!busRoute)
            busRoute = GetComponentInParent<BusRoute>();
        currentWayPoint = busRoute.getNextWayPoint(null);
        nextWayPoint = busRoute.getNextWayPoint(currentWayPoint);
    }

    private void Update_CheckVariables()
    {
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;

        //if ((currentSpeed * 3.6F <= 0.5) || 
        //    (distanceToTarget <= 0.05))
        if (currentSpeed * 3.6F <= 0.5)
            almostStopped = true;
        else
            almostStopped = false;
    }

    private void Update_CheckWaypoints()
    {
        if (!currentWayPoint)
        {
            currentWayPoint = busRoute.getNextWayPoint(null);
            if (!currentWayPoint)
            {
                Debug.LogWarning("A rota " + busRoute.routeName + " não possui Waypoints suficientes!");
                canRunSimulation = false;
                return;
            }
            nextWayPoint = busRoute.getNextWayPoint(currentWayPoint);
        }
    }

    private void FixedUpdate_VehicleMovement()
    {
        //motor = maxMotorTorque * Input.GetAxis("Vertical");           //Use this for player input (manual control of vehicle)
        //steering = maxSteeringAngle * Input.GetAxis("Horizontal");    //Use this for player input (manual control of vehicle)

        if (keepStopped)
        {
            motor = 0;
            steering = 0;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            motor = maxMotorTorque * vehicleAutoCon.calculateMotorThreshold();
            steering = maxSteeringAngle * vehicleAutoCon.calculateSteeringThreshold();
            //if (steering < 0.25F && steering > -0.25F)    //TODO aplicar suavização das curvas
            //{
            //    steering = 0;
            //}
        }

        //if (!(keepStopped || (vehicleAutoCon && vehicleAutoCon.closestObstacle) || !almostStopped))
        //{
            
        //}

        if (GetComponent<Rigidbody>().velocity.magnitude > maximumSpeed)
        {
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            velocity = velocity.normalized * maximumSpeed;
            GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    private void FixedUpdate_CheckVehicleAxles()
    {
        foreach (VehicleAxle axleInfo in vehicleAxles)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            //axleInfo.ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            //axleInfo.ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void defineNextWaypoints(bool calledFromRecordedWP)
    {
        if (!calledFromRecordedWP)
        {
            WaypointOld wp = currentWayPoint;
            currentWayPoint = busRoute.getNextWayPoint(wp);
            nextWayPoint = busRoute.getNextWayPoint(currentWayPoint);
        }
        else
        {
            waypointRecorder.removeRecordedWaypoint(recordedWayPoint);
            recordedWayPoint = null;
        }
    }
}
