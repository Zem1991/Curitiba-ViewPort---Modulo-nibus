using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    public float simulationSpeed = 1;
    public int simulationYear, simulationMonth, simulationDay;
    public int simulationHour, simulationMinute, simulationSecond;
    
    private DateTime simulationStartTime, simulationTime;
    private DateTime executionStartTime, executionTime;

    public List<BusRoute> allBusRoutes = new List<BusRoute>();
    public List<Bus> allBuses = new List<Bus>();
    public BusRoute focusedBusRoute;
    public Bus focusedBus;
    private CameraFocus cameraFocus;

    private SimulationData simData;
    private SimulationResources simResources;

    // Use this for initialization
    void Start () {
        simulationStartTime = new DateTime(
            simulationYear, simulationMonth, simulationDay,
            simulationHour, simulationMinute, simulationSecond);
        executionStartTime = DateTime.Today;
        simulationTime = simulationStartTime;
        executionTime = executionStartTime;

        cameraFocus = FindObjectOfType<CameraFocus>();

        simData = FindObjectOfType<SimulationData>();
        simResources = FindObjectOfType<SimulationResources>();

        recolorWaypoints();
    }
	
	// Update is called once per frame
	void Update () {
        bool focusChanged = input_changeCameraFocus();
        if (focusChanged)
            recolorWaypoints();

        if (allBusRoutes.Count == 0)
            manualData();
    }

    private void FixedUpdate()
    {
        executionTime = executionTime.AddSeconds(Time.deltaTime);
        simulationTime = simulationTime.AddSeconds(Time.deltaTime);
    }

    public string getSimulationTime()
    {
        //return simulationTime.ToShortDateString() + " " + simulationTime.ToShortTimeString(); ;
        return simulationTime.ToString("dd/MM/yyyy HH:mm:ss");
    }

    public string getExecutionTime()
    {
        //return executionTime.ToShortDateString() + " " + executionTime.ToShortTimeString();
        //return executionTime.ToString("dd/MM/yyyy HH:mm:ss");
        return executionTime.ToString("HH:mm:ss");
    }

    public void addBusRoute(BusRoute br)
    {
        allBusRoutes.Add(br);
        if (!focusedBusRoute)
        {
            focusedBusRoute = br;
        }
    }

    public void addBus(Bus b)
    {
        allBuses.Add(b);
        if (!focusedBus)
        {
            focusedBus = b;
        }
    }

    public Bus getFocusedBus()
    {
        return focusedBus;
    }

    public BusRoute getFocusedBusRoute()
    {
        return focusedBusRoute;
    }

    private bool input_changeCameraFocus()
    {
        bool previousBusRoute = Input.GetKeyDown(KeyCode.DownArrow);
        bool nextBusRoute = Input.GetKeyDown(KeyCode.UpArrow);
        bool previousBus = Input.GetKeyDown(KeyCode.LeftArrow);
        bool nextBus = Input.GetKeyDown(KeyCode.RightArrow);

        bool busRouteChanged = false;
        bool busChanged = false;
        if (previousBusRoute && !nextBusRoute)
        {
            int prev = allBusRoutes.IndexOf(focusedBusRoute);
            prev--;
            if (prev < 0)
                prev = allBusRoutes.Count - 1;
            focusedBusRoute = allBusRoutes[prev];
            busRouteChanged = true;
        }
        if (!previousBusRoute && nextBusRoute)
        {
            int next = allBusRoutes.IndexOf(focusedBusRoute);
            next++;
            if (next >= allBusRoutes.Count)
                next = 0;
            focusedBusRoute = allBusRoutes[next];
            busRouteChanged = true;
        }

        if (previousBus && !nextBus)
        {
            int prev = allBuses.IndexOf(focusedBus);
            prev--;
            if (prev < 0)
                prev = allBuses.Count - 1;
            focusedBus = allBuses[prev];
            busChanged = true;
        }
        if (!previousBus && nextBus)
        {
            int next = allBuses.IndexOf(focusedBus);
            next++;
            if (next >= allBuses.Count)
                next = 0;
            focusedBus = allBuses[next];
            busChanged = true;
        }


        if (busRouteChanged)
        {
            if (focusedBusRoute && focusedBusRoute.buses.Count > 0)
            {
                focusedBus = focusedBusRoute.buses[0];
            }
            else
            {
                focusedBus = null;
            }
        }
        if (focusedBusRoute && focusedBus)
        {
            cameraFocus.transform.position = focusedBus.transform.position;
            cameraFocus.transform.rotation = focusedBus.transform.rotation;
        }
        else
        {
            cameraFocus.transform.position = cameraFocus.transform.position;
            cameraFocus.transform.rotation = cameraFocus.transform.rotation;
        }

        return (busRouteChanged || busChanged);
    }

    private void recolorWaypoints()
    {
        foreach (BusRoute item in allBusRoutes)
        {
            if (item != focusedBusRoute)
            {
                item.recolorWaypoints(simResources.waypoint_material, simResources.stoppoint_material);
            }
            else
            {
                item.
                    recolorWaypoints(
                    simResources.waypointFocused_material, 
                    simResources.stoppointFocused_material);
            }
        }
    }

    private void manualData()
    {
        Debug.LogWarning("USANDO PREFABS ADICIONADOS MANUALMENTE!");
        foreach (var item in FindObjectsOfType<BusRoute>())
        {
            addBusRoute(item);
        }
        foreach (var item in FindObjectsOfType<Bus>())
        {
            addBus(item);
        }
    }

    public void Btn_StopSimulation()
    {
        Debug.Log("Btn_StopSimulation()");
    }

    public void Btn_PlayOrPauseSimulation()
    {
        if (Time.timeScale != 0)
            Time.timeScale = 0;
        else
            Time.timeScale = simulationSpeed;
    }

    public void Btn_SimulationSpeed(float simulationSpeed)
    {
        this.simulationSpeed = simulationSpeed;
        Time.timeScale = simulationSpeed;
    }
}
