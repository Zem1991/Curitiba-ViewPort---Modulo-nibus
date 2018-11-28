using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalsoWebservice : MonoBehaviour {
    public GerenciadorDeCena gerenciadorDeCena;
    public Rota rota;
    public List<Rota_Waypoint> waypoints = new List<Rota_Waypoint>();

    // Use this for initialization
    void Start () {
        if (FindObjectOfType<WebserviceBiarticulados>())
        {
            Debug.Log("FalsoWebservice não será utilizado pois existe um  objeto WebserviceBiarticulados em cena.");
            Destroy(this.gameObject);
        }

        gerenciadorDeCena = FindObjectOfType<GerenciadorDeCena>();
        if (!gerenciadorDeCena)
        {
            Debug.LogWarning("FalsoWebservice não pode ser utilizado sem um GerenciadorDeCena, e por isso foi DESTRUIDO!");
            Destroy(this.gameObject);
        }

        PrepararWaypoints();
    }
	
	// Update is called once per frame
	void Update () {
        GerarWaypoints();
	}

    private void PrepararWaypoints()
    {
        if (waypoints.Count <= 0)
            waypoints.AddRange(GetComponentsInChildren<Rota_Waypoint>());

        foreach (var item in waypoints)
        {
            item.rota = rota;
            item.tipoWaypoint = CVP.ModuloOnibus.TipoWaypont.WEBSERVICE;
            item.gameObject.SetActive(false);
        }
    }

    private void GerarWaypoints()
    {
        foreach (var item in waypoints)
        {
            if (!item.gameObject.activeInHierarchy && 
                ConfereHorarioParaGerarWaypoint(item.fw_spawn_hora, item.fw_spawn_minuto, item.fw_spawn_segundo))
            {
                item.gameObject.SetActive(true);
                item.dt_spawn = gerenciadorDeCena.dt_simulationCurrentTime;

                item.dt_horarioParaAlcancar = new DateTime(item.dt_spawn.Year, item.dt_spawn.Month, item.dt_spawn.Day,
                    item.fw_alcancar_hora, item.fw_alcancar_minuto, item.fw_alcancar_segundo); ;

                gerenciadorDeCena.ListarWaypointWebService(item);
            }
        }
    }

    private bool ConfereHorarioParaGerarWaypoint(int hora, int minuto, int segundo)
    {
        if (gerenciadorDeCena.dt_simulationCurrentTime.Hour >= hora)
            if (gerenciadorDeCena.dt_simulationCurrentTime.Minute >= minuto)
                if (gerenciadorDeCena.dt_simulationCurrentTime.Second >= segundo)
                    return true;
                else
                    return false;
            else
                return false;
        else
            return false;
    }
}
