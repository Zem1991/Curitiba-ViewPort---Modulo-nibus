using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleManual : MonoBehaviour {
    public Biarticulado biarticulado;
    public float vertical;
    public float horizontal;
    public float freio;

    // Use this for initialization
    void Start () {
        biarticulado = GetComponent<Biarticulado>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        freio = Input.GetAxis("Jump");
        biarticulado.Movimentar(vertical, horizontal, freio);
    }
}
