using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : Vehicle {
    [Header("Bus variables")]
    public float doorsOpenTimer;
    public float doorsOpenTimerMax;
    public bool doorsOpen = false;

    // Use this for initialization
    public override void Start () {
        base.Start();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        
        if (keepStopped && almostStopped && !doorsOpen)
        {
            Bus_OpenDoors();
        }

        if (doorsOpen)
        {
            if (doorsOpenTimer > 0)
            {
                doorsOpenTimer -= Time.deltaTime;
            }
            else
            {
                Bus_CloseDoors();
            }
        }

        //BEGIN DEBUG
        Vector3 myPos = transform.position;
        Vector3 currentWPPos = currentWayPoint.transform.position;
        Vector3 nextWPPos = nextWayPoint.transform.position;
        myPos.y = 0;
        currentWPPos.y = 0;
        nextWPPos.y = 0;
        Vector3 dirToCurrent = currentWPPos - myPos;
        Vector3 dirToNext = nextWPPos - myPos;
        //float angleToCurrent = Vector3.Angle(dirToCurrent, transform.forward);
        float angleToNext = Vector3.Angle(dirToNext, transform.forward);
        Debug.DrawRay(transform.position, dirToCurrent, Color.blue);
        if      (angleToNext < -90  || angleToNext > 90)    Debug.DrawRay(transform.position, dirToNext, Color.red);
        else if (angleToNext < -60  || angleToNext > 60)    Debug.DrawRay(transform.position, dirToNext, Color.yellow);
        else if (angleToNext < -30  || angleToNext > 30)    Debug.DrawRay(transform.position, dirToNext, Color.green);
        else if (angleToNext >= -5  && angleToNext <= 5)    Debug.DrawRay(transform.position, dirToNext, Color.cyan);
        //END DEBUG
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        if (!canRunSimulation || !currentWayPoint)// || !recordedWayPoint)
            return;    
    }

    public void Bus_StopMovement()
    {
        Debug.Log("BUSAO " + this.vehicleName + " CHEGOU NO PONTO " + currentWayPoint.name + " DA ROTA " + currentWayPoint.busRoute.name);
        keepStopped = true;
    }

    public void Bus_OpenDoors()
    {
        Debug.Log("BUSAO " + this.vehicleName + " ABRIU SUAS PORTAS");
        doorsOpenTimer = doorsOpenTimerMax;
        doorsOpen = true;
    }

    public void Bus_CloseDoors()
    {
        Debug.Log("BUSAO " + this.vehicleName + " FECHOU SUAS PORTAS");
        doorsOpenTimer = 0;
        doorsOpen = false;
        Bus_BusStart();
    }

    public void Bus_BusStart()
    {   
        Debug.Log("BUSAO " + this.vehicleName + " SAIU DO PONTO " + currentWayPoint.name + " DA ROTA " + currentWayPoint.busRoute.name);
        defineNextWaypoints(false);
        keepStopped = false;
    }

    public bool GUI_GetObstacleData(out GameObject obstacle, out float dotDirections, out float dotPositions)
    {
        if (vehicleAutoCon && vehicleAutoCon.closestObstacle)
        {
            obstacle = vehicleAutoCon.closestObstacle;
            Vector3 v1 = transform.position;
            Vector3 v2 = obstacle.transform.position;
            Vector3 direction = (v2 - v1).normalized;
            dotDirections = Vector3.Dot(direction, transform.forward);
            dotPositions = Vector3.Dot(v1.normalized, v2.normalized);
            return true;
        }
        else
        {
            obstacle = null;
            dotDirections = float.NaN;
            dotPositions = float.NaN;
            return false;
        }
    }
}
