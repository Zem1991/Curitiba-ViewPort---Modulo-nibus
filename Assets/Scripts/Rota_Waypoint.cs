using CVP.ModuloOnibus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rota_Waypoint : MonoBehaviour {
    public GameObject go_posOrig;
    public GameObject go_posAjust;

    public Rota rota;
    public TipoWaypont tipoWaypoint;
    public bool usarTodasAsPortas;

    public int fw_spawn_hora;
    public int fw_spawn_minuto;
    public int fw_spawn_segundo;
    public int fw_alcancar_hora;
    public int fw_alcancar_minuto;
    public int fw_alcancar_segundo;
    public DateTime dt_spawn;
    public DateTime dt_horarioParaAlcancar;
    public DateTime dt_horarioAlcancado;
    //public float fw_registro_hora;
    //public float fw_registro_minuto;
    //public float fw_registro_segundo;

    public GerenciadorDeCena gerenciadorDeCena;

    public Vector3 posOrigGPS;
    public Vector3 posOrigXYZ;
    //public Vector3 posAjustGPS;   //Não é necessária
    public Vector3 posAjustXYZ;

    // Use this for initialization
    void Start () {
        if (!rota)
            rota = GetComponentInParent<Rota>();
        gerenciadorDeCena = FindObjectOfType<GerenciadorDeCena>();

        //if (go_posOrig)
        //    go_posOrig.gameObject.SetActive(false);
        //if (go_posAjust)
        //    go_posAjust.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Biarticulado biarticulado = other.gameObject.GetComponent<Biarticulado>();
        if (!biarticulado)
            biarticulado = other.gameObject.GetComponentInParent<Biarticulado>();

        if (biarticulado)
        {
            //Waypoints tipo WEBSERVICE não determinam o trajeto específico e nem aonde cada ônibus para.
            //A função destes é ajudar a determinar se o veíuclo está adiantado ou atrasado em relação aos posicionamentos.
            if (tipoWaypoint == TipoWaypont.WEBSERVICE && biarticulado.rota == rota)
            {
                gerenciadorDeCena.WaypointWebServiceAlcancado(this, biarticulado);
            }
            else
            {
                biarticulado.AtualizarRota(this);
            }
        }
    }

    public void PreencherParametrosDataHora()
    {
        fw_spawn_hora = dt_spawn.Hour;
        fw_spawn_minuto = dt_spawn.Minute;
        fw_spawn_segundo = dt_spawn.Second;
        fw_alcancar_hora = dt_horarioParaAlcancar.Hour;
        fw_alcancar_minuto = dt_horarioParaAlcancar.Minute;
        fw_alcancar_segundo = dt_horarioParaAlcancar.Second;
    }
}
