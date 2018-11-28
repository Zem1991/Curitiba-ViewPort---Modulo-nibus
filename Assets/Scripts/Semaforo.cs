using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaforo : MonoBehaviour {
    public bool estaAberto;
    public string identificacao;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Renderer item in GetComponentsInChildren<Renderer>())
        {
            if (estaAberto)
                item.material.color = Color.green;
            else
                item.material.color = Color.red;
        }
    }
}
