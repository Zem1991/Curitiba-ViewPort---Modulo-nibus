using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BusCalculations
{
    public static float calculate_DynamicStoppingDistance_Waypoint(Vehicle vehicle, float stoppingDistance, WaypointOld currentWP, WaypointOld nextWP)
    {
        //Initial stopping distance is used, since we know the bus can effectively brake at full speed at this distance from the target point.
        float result = stoppingDistance;
        //But if the bus is not at its maximum speed, then the distance needed will be proportional to its current speed.
        result *= (vehicle.currentSpeed / vehicle.maximumSpeed);

        //However, since waypoints can be in the same line, we don't need to brake if thats the case.
        //Here we define the distance based on how steep is the angle between the bus and the next waypoint (after the current one).
        if (!currentWP.isStopPoint)
        {
            Vector3 directionToNextWP = nextWP.transform.position - vehicle.transform.position;
            float angleToNextWP = Vector3.Angle(directionToNextWP, vehicle.transform.forward);
            //TODO Change the '0' to some static variable higher than 0 (like MINIMUM_ANGLE_REQUIRED), to stop the 'trembling'
            result *= (Mathf.Clamp(angleToNextWP, 0, vehicle.maxSteeringAngle) / vehicle.maxSteeringAngle);
        }

        return result;
    }

    public static float calculate_DynamicStoppingDistance_Obstacle(Vehicle vehicle, float stoppingDistance, GameObject obstacle)
    {
        //Initial stopping distance is used, since we know the bus can effectively brake at full speed at this distance from the target point.
        float result = stoppingDistance;
        //But if the bus is not at its maximum speed, then the distance needed will be proportional to its current speed.
        result *= (vehicle.currentSpeed / vehicle.maximumSpeed);

        //THIS ENTIRE BLOCKIS USELESS
        //if (obstacle)
        //{
        //    Bounds obstacleBounds = new Bounds(obstacle.transform.position, Vector3.zero);
        //    if (obstacle.GetComponent<Renderer>())
        //        obstacleBounds.Encapsulate(obstacle.GetComponent<Renderer>().bounds);
        //    foreach (Renderer item in obstacle.GetComponentsInChildren<Renderer>())
        //        obstacleBounds.Encapsulate(item.bounds);

        //    result -= (bus.getBounds().size.z + obstacleBounds.size.z + 1);
        //    result = Mathf.Clamp(result, 0, bus.stoppingDistance);
        //}

        return result;
    }
    
    /*
    * Dynamic Braking :    based on one of two parameters
    * 1 -  how steep is the angle torwards the next waypoint - no need to apply brakes if the next waypoint is lined up with the current waypoint;
    * OR
    * 2 -  the remaining distance torward the current waypoint IF is a stoppoint.
    */
    public static float calculateDynamicBrakingThresholdWP(float currentSpeed, float maneuveringSpeed, float angleToNext, float maxSteeringAngle)
    {
        float result;
        if (currentSpeed > maneuveringSpeed)
        {
            //Will brake proportionally to the angle torwards the next waypoint.
            result = Mathf.Clamp(angleToNext, 0, maxSteeringAngle) / maxSteeringAngle;
        }
        else
        {
            //Will keep a steady torque to mantain itself close to the the maneuvering speed.
            result = -1 + (Mathf.Clamp(currentSpeed, 0, maneuveringSpeed) / maneuveringSpeed);
        }
        return result;
    }

    public static float calculateDynamicBrakingThresholdSP(float currentSpeed, float maneuveringSpeed, float distanceToCurrent, float dynamicStoppingDistance)
    {
        float result;
        if (currentSpeed > 1 && distanceToCurrent > 1)
        {
            result = 1;
        }
        else
        {
            result = Mathf.Clamp(currentSpeed, 0, maneuveringSpeed) / maneuveringSpeed;
            result *= Mathf.Clamp(distanceToCurrent, 0, dynamicStoppingDistance) / dynamicStoppingDistance;

            //float a = Mathf.Clamp(currentSpeed, 0, maneuveringSpeed) / maneuveringSpeed;
            //float b = Mathf.Clamp(distanceToCurrent, 0, dynamicStoppingDistance) / dynamicStoppingDistance;
            //result = Mathf.Max(a, b);
        }
        return result;
    }

    public static float calculateDynamicBrakingThresholdObstacle(Vehicle vehicle, float currentSpeed, float maneuveringSpeed, GameObject obstacle, float dynamicStoppingDistance)
    {
        float result = 0;
        if (obstacle)
        {
            float distanceToObstacle = Vector3.Distance(vehicle.transform.position, obstacle.transform.position);
            if (currentSpeed > 1 && distanceToObstacle > 1)
            {
                result = 1;
            }
            else
            {
                result = Mathf.Clamp(currentSpeed, 0, maneuveringSpeed) / maneuveringSpeed;
                result *= Mathf.Clamp(distanceToObstacle, 0, dynamicStoppingDistance) / dynamicStoppingDistance;
            }
        }
        return result;
    }
}


//float result;
////if (distanceToCurrent > Mathf.Min(stoppingDistance, dynamicStoppingDistance))
//if (!obstacle)
//{
//    if (distanceToCurrentWP > dynamicStoppingDistance)
//    {
//        if (currentSpeed > maximumSpeed)
//        {
//            result = 0;
//        }
//        else
//        {
//            result = 1;
//        }
//    }
//    else
//    {
//        if (!currentWayPoint.isStopPoint)
//        {
//            result = -dynBraking_WP;
//        }
//        else
//        {
//            if (!almostStopped)
//            {
//                result = -dynBraking_SP;
//            }
//            else
//            {
//                result = 0;
//            }
//        }
//    }
//}
//else    //if (!obstacle)
//{
//    if (!almostStopped)
//    {
//        result = -dynBraking_Obs;
//    }
//    else
//    {
//        result = 0;
//    }
//}
//return result;