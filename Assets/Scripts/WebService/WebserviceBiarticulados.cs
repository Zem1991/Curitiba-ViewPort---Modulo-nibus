using ModuloOnibus.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebserviceBiarticulados : MonoBehaviour {
    public GerenciadorDeCena gerenciadorDeCena;
    public Rota rota;
    public Rota_Waypoint prefab_Waypoint;
    public List<Rota_Waypoint> waypoints = new List<Rota_Waypoint>();

    public WebAPI_Connector webAPICon;
    public float tempoProximaConsulta;
    public float tempoEntreConsultas;
    public float itensParaRetornar;
    public float toleranciaDistanciaParaRota;
    public List<PosicaoGPS> posicoesGPS = new List<PosicaoGPS>();

    // Use this for initialization
    void Start () {
        gerenciadorDeCena = FindObjectOfType<GerenciadorDeCena>();
        if (!gerenciadorDeCena)
        {
            Debug.LogWarning("WebserviceBiarticulados não pode ser utilizado sem um GerenciadorDeCena, e por isso foi DESTRUIDO!");
            Destroy(this);
        }
        webAPICon = GetComponent<WebAPI_Connector>();
    }
	
	// Update is called once per frame
	void Update () {
        tempoProximaConsulta -= Time.deltaTime;
        if (tempoProximaConsulta <= 0)
        {
            tempoProximaConsulta = tempoEntreConsultas;
            GerarWaypoints();
        }
    }

    private void GerarWaypoints()
    {
        List<PosicaoGPS> newPosicoesGPS = new List<PosicaoGPS>();
        int count = 0;

        string idBus = "BD126";
        string dataHora = "";
        //dataHora = "2016-06-29%2020:07:13";
        //dataHora = gerenciadorDeCena.dt_simulationCurrentTime.ToShortDateString() + "%20" + gerenciadorDeCena.dt_simulationCurrentTime.ToShortTimeString();
        dataHora += gerenciadorDeCena.dt_simulationCurrentTime.Year;
        dataHora += "-";
        dataHora += gerenciadorDeCena.dt_simulationCurrentTime.Month;
        dataHora += "-";
        dataHora += gerenciadorDeCena.dt_simulationCurrentTime.Day;
        dataHora += "%20";
        dataHora += gerenciadorDeCena.dt_simulationCurrentTime.Hour;
        dataHora += ":";
        dataHora += gerenciadorDeCena.dt_simulationCurrentTime.Minute;
        dataHora += ":";
        dataHora += gerenciadorDeCena.dt_simulationCurrentTime.Second;
        dataHora += ".";
        dataHora += gerenciadorDeCena.dt_simulationCurrentTime.Millisecond;

        List<PosicaoGPS> retornoWebservice = 
            webAPICon.
            CallWebService(idBus, dataHora);

        if (retornoWebservice == null || retornoWebservice.Count <= 0)
        {
            Debug.LogWarning("O WebService não pode ser contactado, ou o mesmo não retornou posição alguma.");
            return;
        }

        foreach (var item in retornoWebservice)
        {
            //DateTime dt = DateTime.ParseExact(item.DTHR, gerenciadorDeCena.formatoDateTime, System.Globalization.CultureInfo.InvariantCulture);
            //if (DateTime.Compare(dt, gerenciadorDeCena.dt_simulationCurrentTime) >= 0)
            if (DateTime.Compare(item.DTHR, gerenciadorDeCena.dt_simulationCurrentTime) >= 0)
            {
                newPosicoesGPS.Add(item);
                count++;
            }

            if (count >= itensParaRetornar)
            {
                posicoesGPS.AddRange(newPosicoesGPS);
                break;
            }
        }

        Vector3 posOrigGPS = new Vector3();
        Vector3 posOrigXYZ = new Vector3();
        Vector3 posAjustXYZ = new Vector3();
        Quaternion rot = new Quaternion();
        foreach (var item in newPosicoesGPS)
        {
            float lat = float.Parse(item.LAT.Replace(',', '.'));
            float lon = float.Parse(item.LON.Replace(',', '.'));
            posOrigGPS = new Vector3(lat, lon, 0);
            posOrigXYZ = CVP_Calculos.GPS_to_XYZ(posOrigGPS.x, posOrigGPS.y, posOrigGPS.z);
            if (CVP_Calculos.AjustarPosicoesParaRotaPadrao(rota, posOrigXYZ, toleranciaDistanciaParaRota, out posAjustXYZ))
            {
                //posAjustXYZ
                //Rota_Waypoint wp = Instantiate(prefab_Waypoint, posOrigXYZ, rot, this.transform);
                Rota_Waypoint wp = Instantiate(prefab_Waypoint, posAjustXYZ, rot, this.transform);

                wp.go_posOrig.gameObject.SetActive(true);
                wp.go_posOrig.transform.position = posOrigXYZ;
                wp.go_posAjust.gameObject.SetActive(true);
                wp.go_posAjust.transform.position = posAjustXYZ;

                wp.rota = rota;
                wp.tipoWaypoint = CVP.ModuloOnibus.TipoWaypont.WEBSERVICE;

                wp.dt_spawn = gerenciadorDeCena.dt_simulationCurrentTime;
                wp.dt_horarioParaAlcancar = item.DTHR;
                wp.PreencherParametrosDataHora();

                wp.posOrigGPS = posOrigGPS;
                wp.posOrigXYZ = posOrigXYZ;
                wp.posAjustXYZ = posAjustXYZ;

                waypoints.Add(wp);
                gerenciadorDeCena.ListarWaypointWebService(wp);
            }
            else
            {
                Debug.Log("Uma posição GPS foi descartada, pos durante o ajuste esta ficou longe demais da Rota.");
            }
        }
    }
}
