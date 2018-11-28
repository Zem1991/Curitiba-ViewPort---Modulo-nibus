using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiarticuladoPassageiros : MonoBehaviour {
    public Biarticulado biarticulado;
    public List<Cidadao> passageiros = new List<Cidadao>();

	// Use this for initialization
	void Start () {
        if (!biarticulado)
            biarticulado = GetComponentInParent<VagaoBiarticulado>().biarticulado;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Cidadao cid = other.gameObject.GetComponent<Cidadao>();
        if (cid && cid.biarticulado == null)
        {
            cid.transform.parent = transform;
            cid.biarticulado = biarticulado;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        Cidadao cid = other.gameObject.GetComponent<Cidadao>();
        if (cid && cid.biarticulado == biarticulado)
        {
            cid.transform.parent = null;
            cid.biarticulado = null;
        }
    }
}
