using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAutomatedController : MonoBehaviour {
    private Vehicle vehicle;
    private VehicleWaypointRecorder vwpRecorder;

    public float stoppingDistance;
    public float dynStopDist_WP;
    public float dynStopDist_Obs;
    public float dynBraking_WP;
    public float dynBraking_SP;
    public float dynBraking_Obs;
    public float distanceToTarget;

    public List<GameObject> obstacles = new List<GameObject>();
    public GameObject closestObstacle;
    public GameObject obstaclePosition;
    public GameObject obstaclePositionOffset;

    // Use this for initialization
    void Start () {
        vehicle = GetComponent<Vehicle>();
        vwpRecorder = GetComponent<VehicleWaypointRecorder>();
    }
	
	// Update is called once per frame
	void Update () {
        Update_CheckObstacles();
        closestObstacle = checkClosestObstacle();
    }

    public float calculateMotorThreshold()
    {
        WaypointOld currentWP = ResolveCurrentWaypointToGo(vehicle.currentWayPoint, vehicle.recordedWayPoint);
        if (!currentWP)
            return 0;

        Vector3 myPos = transform.position;
        Vector3 currentWPPos = currentWP.transform.position;
        Vector3 nextWPPos = vehicle.nextWayPoint.transform.position;
        myPos.y = 0;
        currentWPPos.y = 0;
        nextWPPos.y = 0;

        Vector3 directionToCurrentWP = currentWPPos - myPos;
        float distanceToCurrentWP = Vector3.Distance(myPos, currentWPPos);
        float angleToCurrentWP = Vector3.Angle(directionToCurrentWP, transform.forward);
        Vector3 directionToNextWP = nextWPPos - myPos;
        float distanceToNextWP = Vector3.Distance(myPos, nextWPPos);
        float angleToNextWP = Vector3.Angle(directionToNextWP, transform.forward);
        Vector3 directionToObstacle = Vector3.zero;
        float distanceToObstacle = float.NaN;
        float angleToObstacle = float.NaN;

        if (closestObstacle)
        {
            obstaclePosition.transform.position = closestObstacle.transform.position;
            obstaclePosition.transform.rotation = closestObstacle.transform.rotation;
            obstaclePositionOffset.transform.position = closestObstacle.transform.position;
            obstaclePositionOffset.transform.rotation = closestObstacle.transform.rotation;

            Vector3 offset = new Vector3();
            Semaphore s = closestObstacle.GetComponent<Semaphore>();
            if (s)
            {
                offset = HelperFunctions.OffsetBetweenTransforms(transform, obstaclePosition.transform);
                offset.y = 0;
                offset.z = vehicle.bounds.size.z * 2;
                obstaclePositionOffset.transform.position -= offset;
            }

            obstaclePosition.transform.Rotate(closestObstacle.transform.rotation.eulerAngles, Space.World);

            directionToObstacle = obstaclePositionOffset.transform.position - myPos;
            distanceToObstacle = Vector3.Distance(myPos, obstaclePositionOffset.transform.position);
            angleToObstacle = Vector3.Angle(directionToObstacle, transform.forward);
        }
        else
        {
            obstaclePosition.transform.position = transform.position;
            obstaclePositionOffset.transform.position = transform.position;
            obstaclePosition.transform.rotation = transform.rotation;
            obstaclePositionOffset.transform.rotation = transform.rotation;
        }

        dynStopDist_WP = BusCalculations.calculate_DynamicStoppingDistance_Waypoint(vehicle, stoppingDistance, currentWP, vehicle.nextWayPoint);
        dynStopDist_Obs = BusCalculations.calculate_DynamicStoppingDistance_Obstacle(vehicle, stoppingDistance, closestObstacle);
        dynBraking_WP = BusCalculations.calculateDynamicBrakingThresholdWP(
            vehicle.currentSpeed,
            vehicle.maneuverSpeed,
            angleToNextWP,
            vehicle.maxSteeringAngle);
        dynBraking_SP = BusCalculations.calculateDynamicBrakingThresholdSP(
            vehicle.currentSpeed,
            vehicle.maneuverSpeed,
            distanceToCurrentWP,
            dynStopDist_WP
            );
        dynBraking_Obs = BusCalculations.calculateDynamicBrakingThresholdObstacle(vehicle,
            vehicle.currentSpeed,
            vehicle.maneuverSpeed,
            closestObstacle,
            dynStopDist_Obs
            );

        float dynamicStoppingDistance;
        if (closestObstacle && distanceToObstacle <= distanceToCurrentWP)
        {
            distanceToTarget = distanceToObstacle;
            dynamicStoppingDistance = dynStopDist_Obs;
        }
        else
        {
            distanceToTarget = distanceToCurrentWP;
            dynamicStoppingDistance = dynStopDist_WP;
        }

        float result;
        if (distanceToTarget > dynamicStoppingDistance)
        {
            if (vehicle.currentSpeed >= vehicle.maximumSpeed)
            {
                //Cease the positive torque since we achieved maximumSpeed.
                result = 0;
            }
            else
            {
                result = 1;
            }
        }
        else
        {
            if (!closestObstacle)
            {
                if (!currentWP.isStopPoint)
                {
                    result = -dynBraking_WP;
                }
                else
                {
                    if (!vehicle.almostStopped)
                    {
                        result = -dynBraking_SP;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            else
            {
                if (!vehicle.almostStopped)
                {
                    result = -dynBraking_Obs;
                }
                else
                {
                    result = 0;
                }
            }
        }
        return result;
    }

    public float calculateSteeringThreshold()
    {
        WaypointOld currentWP = ResolveCurrentWaypointToGo(vehicle.currentWayPoint, vehicle.recordedWayPoint);
        if (!currentWP)
            return 0;

        float result;
        if (!vehicle.keepStopped)
        {
            // calculate the local-relative position of the target, to steer towards
            Vector3 targetPos = transform.InverseTransformPoint(currentWP.transform.position);

            // work out the local angle towards the target
            float targetAngle = Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg;

            // get the amount of steering needed to aim the car towards the target
            result = Mathf.Clamp(targetAngle, -1, 1); // * Mathf.Sign(m_CarController.CurrentSpeed);

            //if (result > -.5F && result < .5F) result = 0F;

            // feed input to the car controller.
            //m_CarController.Move(steer, accel, accel, 0f);
        }
        else
        {
            result = 0;
        }
        return result;
    }

    public WaypointOld ResolveCurrentWaypointToGo(WaypointOld fromDefaultRoute, WaypointOld fromRecordedRoute)
    {
        if (!fromDefaultRoute && !fromRecordedRoute)
            return null;
        if (fromDefaultRoute && !fromRecordedRoute)
            return fromDefaultRoute;
        if (!fromDefaultRoute && fromRecordedRoute)
            return fromRecordedRoute;

        float distToFDR = Vector3.Distance(vehicle.transform.position, fromDefaultRoute.transform.position);
        float distToFRR = Vector3.Distance(vehicle.transform.position, fromRecordedRoute.transform.position);
        if (distToFDR < distToFRR)
            return fromDefaultRoute;
        else
            return fromRecordedRoute;
    }

    private void Update_CheckObstacles()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Max(stoppingDistance, dynStopDist_WP, dynStopDist_Obs));
        foreach (Collider item in colliders)
        {
            Semaphore_Wrapper sw = item.gameObject.GetComponent<Semaphore_Wrapper>();
            if (sw)
            {
                Semaphore s = sw.getSemaphore();
                Vector3 v1 = transform.rotation.eulerAngles;
                Vector3 v2 = s.transform.rotation.eulerAngles;
                bool sameDirection = HelperFunctions.AngleCloseEnough(v1.y, v2.y, 15);
                bool behindObstacle = (HelperFunctions.OffsetBetweenTransforms(transform, s.transform).z >= 0);

                if (sameDirection && behindObstacle && !s.isOpen)
                    if (obstacles.IndexOf(s.gameObject) == -1)
                        obstacles.Add(s.gameObject);
                    else
                        obstacles.Remove(s.gameObject);
            }

            Vehicle vehicle = item.gameObject.GetComponent<Vehicle>();
            if (vehicle && vehicle != this)
            {
                Vector3 v1 = transform.rotation.eulerAngles;
                Vector3 v2 = vehicle.transform.rotation.eulerAngles;
                bool sameDirection = HelperFunctions.AngleCloseEnough(v1.y, v2.y, 15);
                bool behindObstacle = (HelperFunctions.OffsetBetweenTransforms(transform, vehicle.transform).z >= 0);

                if (sameDirection && behindObstacle)
                    if (obstacles.IndexOf(item.gameObject) == -1)
                        obstacles.Add(item.gameObject);
                    else
                        obstacles.Remove(item.gameObject);
            }
        }
    }

    private GameObject checkClosestObstacle()
    {
        GameObject result = closestObstacle;
        float resultDistance = (result ? Vector3.Distance(transform.position, closestObstacle.transform.position): -1);

        foreach (var item in obstacles)
        {
            float aux = Vector3.Distance(transform.position, item.transform.position);
            Semaphore s = item.GetComponent<Semaphore>();
            Vehicle v = item.GetComponent<Vehicle>();
            if ((s && !s.isOpen) ||
                (v && v != vehicle))
            {
                if (!result || aux < resultDistance)
                {
                    result = item;
                    resultDistance = aux;
                }
            }
        }
        return result;
    }
}
