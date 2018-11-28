using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrupoSemaforo : MonoBehaviour {
    public List<Semaforo> semaforos;
    public float timerAtual;
    public float timerMaximo;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timerAtual >= timerMaximo)
        {
            InverterSemaforos();
            timerAtual = 0;
        }
        else
        {
            timerAtual += Time.deltaTime;
        }
    }

    private void InverterSemaforos()
    {
        foreach (Semaforo item in semaforos)
        {
            if (item.estaAberto)
                item.estaAberto = false;
            else
                item.estaAberto = true;
        }
    }
}
