using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationData : MonoBehaviour {
    /*
     * Esta classe atua como um Banco de Dados na falta das conexões para o WebService a ser utilizado, para fins de testes.
     */
    public Vector3 busSpawnCoordinates;
    public float busSpawnDelay, busSpawnCountdown;

    public List<string> db_BusRoutes = new List<string>();
    public List<List<Vector3>> db_Waypoints = new List<List<Vector3>>();
    public List<List<string>> db_Buses = new List<List<string>>();
    //TODO lista de semaforos

    private List<BusRoute> busRoutes = new List<BusRoute>();
    private List<Bus> buses = new List<Bus>();

    private int busSpawn_busRoutesCounter;
    private int busSpawn_busesCounter;

    private SceneManager sceneManager;
    private SimulationResources simResources;

    // Use this for initialization
    void Start () {
        //busSpawnCountdown = busSpawnDelay;
        simResources = GetComponent<SimulationResources>();
        sceneManager = FindObjectOfType<SceneManager>();

        receiveData();
        initializeBusRoutes();
        initializeWaypoints();
        //initializeBuses();
    }
	
	// Update is called once per frame
	void Update () {
        busSpawnCountdown -= Time.deltaTime;
        spawnBuses();
    }

    public void receiveData()
    {
        db_BusRoutes.Add("Rota Teste 01");
        db_BusRoutes.Add("Rota Teste 02");
        db_BusRoutes.Add("Rota Teste 03");
        db_BusRoutes.Add("Rota Teste 04");

        db_Waypoints.Add(new List<Vector3>());
        db_Waypoints[0].Add(new Vector3(2, 0, 98));
        db_Waypoints[0].Add(new Vector3(98, 0, 98));
        db_Waypoints[0].Add(new Vector3(98, 0, 2));
        db_Waypoints[0].Add(new Vector3(98, 0, -50));       //STOPPOINT!
        db_Waypoints[0].Add(new Vector3(98, 0, -98));
        db_Waypoints[0].Add(new Vector3(-2, 0, -98));
        db_Waypoints[0].Add(new Vector3(-98, 0, -98));
        db_Waypoints[0].Add(new Vector3(-98, 0, -2));
        db_Waypoints[0].Add(new Vector3(-98, 0, 50));       //STOPPOINT!
        db_Waypoints[0].Add(new Vector3(-98, 0, 98));
        db_Waypoints.Add(new List<Vector3>());
        db_Waypoints[1].Add(new Vector3(202, 0, -2));
        db_Waypoints[1].Add(new Vector3(202, 0, 102));
        db_Waypoints[1].Add(new Vector3(-2, 0, 102));
        db_Waypoints[1].Add(new Vector3(-2, 0, -50));       //STOPPOINT!
        db_Waypoints[1].Add(new Vector3(-2, 0, -98));
        db_Waypoints[1].Add(new Vector3(-198, 0, -98));
        db_Waypoints[1].Add(new Vector3(-198, 0, -2));
        db_Waypoints.Add(new List<Vector3>());
        db_Waypoints[2].Add(new Vector3(2, 0, 102));
        db_Waypoints[2].Add(new Vector3(-102, 0, 102));
        db_Waypoints[2].Add(new Vector3(-102, 0, 50));      //STOPPOINT!
        db_Waypoints[2].Add(new Vector3(-102, 0, -2));
        db_Waypoints[2].Add(new Vector3(-102, 0, -102));
        db_Waypoints[2].Add(new Vector3(-2, 0, -102));
        db_Waypoints[2].Add(new Vector3(102, 0, -102));
        db_Waypoints[2].Add(new Vector3(102, 0, -50));      //STOPPOINT!
        db_Waypoints[2].Add(new Vector3(102, 0, 2));
        db_Waypoints[2].Add(new Vector3(102, 0, 102));
        db_Waypoints.Add(new List<Vector3>());
        db_Waypoints[3].Add(new Vector3(2, 0, 183));
        db_Waypoints[3].Add(new Vector3(15, 0, 189.5F));
        db_Waypoints[3].Add(new Vector3(22, 0, 205));
        db_Waypoints[3].Add(new Vector3(15, 0, 220.5F));
        db_Waypoints[3].Add(new Vector3(-2, 0, 227F));
        db_Waypoints[3].Add(new Vector3(-15, 0, 220.5F));
        db_Waypoints[3].Add(new Vector3(-22, 0, 205));
        db_Waypoints[3].Add(new Vector3(-15, 0, 189.5F));
        db_Waypoints[3].Add(new Vector3(-2, 0, 183));
        db_Waypoints[3].Add(new Vector3(-2, 0, -50));       //STOPPOINT!
        db_Waypoints[3].Add(new Vector3(-2, 0, -183));
        db_Waypoints[3].Add(new Vector3(-15, 0, -189.5F));
        db_Waypoints[3].Add(new Vector3(-22, 0, -205));
        db_Waypoints[3].Add(new Vector3(-15, 0, -220.5F));
        db_Waypoints[3].Add(new Vector3(2, 0, -227F));
        db_Waypoints[3].Add(new Vector3(15, 0, -220.5F));
        db_Waypoints[3].Add(new Vector3(22, 0, -205));
        db_Waypoints[3].Add(new Vector3(15, 0, -189.5F));
        db_Waypoints[3].Add(new Vector3(2, 0, -183));
        db_Waypoints[3].Add(new Vector3(2, 0, 50));         //STOPPOINT!

        db_Buses.Add(new List<string>());
        db_Buses[0].Add("Ônibus R01 B01");
        db_Buses[0].Add("Ônibus R01 B02");
        db_Buses[0].Add("Ônibus R01 B03");
        db_Buses.Add(new List<string>());
        db_Buses[1].Add("Ônibus R02 B01");
        db_Buses[1].Add("Ônibus R02 B02");
        db_Buses[1].Add("Ônibus R02 B03");
        db_Buses.Add(new List<string>());
        db_Buses[2].Add("Ônibus R03 B01");
        db_Buses[2].Add("Ônibus R03 B02");
        db_Buses[2].Add("Ônibus R03 B03");
        db_Buses.Add(new List<string>());
        db_Buses[3].Add("Ônibus R04 B01");
        db_Buses[3].Add("Ônibus R04 B02");
    }

    private void initializeBusRoutes()
    {
        for (int i = 0; i < db_BusRoutes.Count; i++)
        {
            BusRoute br = Instantiate(simResources.busRoute_prefab);
            br.routeName = db_BusRoutes[i];
            br.transform.parent = transform;
            busRoutes.Add(br);

            sceneManager.addBusRoute(br);
        }
    }

    private void initializeWaypoints()
    {
        for (int i = 0; i < db_BusRoutes.Count; i++)
        {
            for (int j = 0; j < db_Waypoints[i].Count; j++)
            {
                WaypointOld w = Instantiate(simResources.waypoint_prefab);
                w.transform.position = db_Waypoints[i][j];
                w.transform.parent = busRoutes[i].transform;
                busRoutes[i].addWaypoint(w);

                //TODO habilitar isso somente quando houver detecção de outros ônibus como obstáculos!
                //if (j == 3 || j == 8)
                //    w.isStopPoint = true;
            }
        }
    }

    private void spawnBuses()
    {
        if ((busSpawn_busRoutesCounter < db_BusRoutes.Count) && (busSpawnCountdown <= 0))
        {
            Bus b = Instantiate(simResources.bus_prefab, busSpawnCoordinates, transform.rotation);
            b.vehicleName = db_Buses[busSpawn_busRoutesCounter][busSpawn_busesCounter];
            b.transform.parent = busRoutes[busSpawn_busRoutesCounter].transform;
            busRoutes[busSpawn_busRoutesCounter].addBus(b);

            buses.Add(b);
            sceneManager.addBus(b);

            busSpawnCountdown = busSpawnDelay;
            if (busSpawn_busesCounter + 1 < db_Buses[busSpawn_busRoutesCounter].Count)
            {
                busSpawn_busesCounter++;
            }
            else// if (busSpawn_busRoutesCounter + 1 < busRoutes.Count)
            {
                busSpawn_busRoutesCounter++;
                busSpawn_busesCounter = 0;
            }
        }
    }
}
