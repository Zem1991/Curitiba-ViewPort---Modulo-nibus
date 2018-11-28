using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDeCena : MonoBehaviour {
    public enum ModoDeInput
    {
        BIARTICULADO,
        CIDADAO
    }
    public ModoDeInput modoDeInput;
    public GameObject objetoInput;

    [Header("Objetos")]
    public InterfaceGrafica interfaceGrafica;

    [Header("Constantes")]
    public string formatoDateTime = "dd/MM/yyyy HH:mm:ss";

    [Header("Valores estáticos")]
    public DateTime dt_simulationStartTime;     //Not visible in Inspector.
    public DateTime dt_executionStartTime;      //Not visible in Inspector.
    public int simulationYear;
    public int simulationMonth;
    public int simulationDay;
    public int simulationHour;
    public int simulationMinute;
    public int simulationSecond;

    [Header("Valores dinâmicos")]
    public DateTime dt_simulationCurrentTime;   //Not visible in Inspector.
    public DateTime dt_executionCurrentTime;    //Not visible in Inspector.
    public float simulationSpeed = 1;
    public string simulationStartTime;
    public string executionStartTime;
    public string simulationCurrentTime;
    public string executionCurrentTime;

    [Header("Condições")]
    public bool usarLuzNoturna;

    // Use this for initialization
    void Start () {
        dt_simulationStartTime = new DateTime(simulationYear, simulationMonth, simulationDay, simulationHour, simulationMinute, simulationSecond);
        dt_executionStartTime = new DateTime();
        dt_simulationCurrentTime = dt_simulationStartTime;
        dt_executionCurrentTime = dt_executionStartTime;
    }

    // Update is called once per frame
    void FixedUpdate () {
        dt_simulationCurrentTime = dt_simulationCurrentTime.AddSeconds(Time.deltaTime);
        dt_executionCurrentTime = dt_executionCurrentTime.AddSeconds(Time.deltaTime);

        simulationStartTime = dt_simulationStartTime.ToString();
        executionStartTime = dt_executionStartTime.ToString(formatoDateTime);
        simulationCurrentTime = dt_simulationCurrentTime.ToString(formatoDateTime);
        executionCurrentTime = dt_executionCurrentTime.ToString(formatoDateTime);
    }

    public void Simulacao_Stop()
    {
        Debug.Log("Btn_StopSimulation()");
    }

    public void Simulacao_PlayPause()
    {
        if (Time.timeScale != 0)
            Time.timeScale = 0;
        else
            Time.timeScale = simulationSpeed;
    }

    public void Simulacao_Velocidade(float simulationSpeed)
    {
        this.simulationSpeed = simulationSpeed;
        Time.timeScale = simulationSpeed;
    }

    public void Iluminacao_LuzNoturna(bool ativar)
    {
        usarLuzNoturna = ativar;
    }

    public void ListarWaypointWebService(Rota_Waypoint rota_Waypoint)
    {
        GUI_Rota_WebService_Dados_WP obj = 
            Instantiate(
                interfaceGrafica.dadosWPAlcancado, 
                interfaceGrafica.rotaWebService.scrollViewContent.transform);
        obj.waypoint = rota_Waypoint;

        interfaceGrafica.rotaWebService.dadosDosWPWeb.Add(obj);
        int id = interfaceGrafica.rotaWebService.dadosDosWPWeb.IndexOf(obj);

        obj.AjustarTextos(false, id+1, 
            obj.waypoint.posOrigGPS, obj.waypoint.posOrigXYZ, obj.waypoint.posAjustXYZ,
            obj.waypoint.dt_horarioParaAlcancar.ToString(formatoDateTime), null, float.NaN);
    }

    public void WaypointWebServiceAlcancado(Rota_Waypoint waypoint, Biarticulado biarticulado)
    {
        foreach (var item in interfaceGrafica.rotaWebService.dadosDosWPWeb)
        {
            if (item.waypoint == waypoint)
            {
                item.waypoint.dt_horarioAlcancado = dt_simulationCurrentTime;
                TimeSpan ts = item.waypoint.dt_horarioAlcancado - item.waypoint.dt_horarioParaAlcancar;
                float adiantamentoOuAtraso = (float) ts.TotalSeconds;

                int id = interfaceGrafica.rotaWebService.dadosDosWPWeb.IndexOf(item);
                item.AjustarTextos(true, id + 1,
                    item.waypoint.posOrigGPS, item.waypoint.posOrigXYZ, item.waypoint.posAjustXYZ,
                    item.waypoint.dt_horarioParaAlcancar.ToString(formatoDateTime), item.waypoint.dt_horarioAlcancado.ToString(formatoDateTime), adiantamentoOuAtraso);

                biarticulado.tempoAtrasadoOuAdiantado += adiantamentoOuAtraso;
                break;
            }
        }
    }
}
