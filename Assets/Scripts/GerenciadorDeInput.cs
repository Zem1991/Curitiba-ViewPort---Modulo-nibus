using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDeInput : MonoBehaviour {
    public bool moveUp;
    public bool moveLeft;
    public bool moveDown;
    public bool moveRight;
    public bool rotClock;
    public bool rotCounter;
    public bool zoomingOut;
    public bool zoomingIn;
    public bool shift;
    public bool tab;
    public bool sim_parar;
    public bool sim_pausarOuRetomar;
    public bool sim_velocidade_025;
    public bool sim_velocidade_05;
    public bool sim_velocidade_1;
    public bool sim_velocidade_2;
    public bool sim_velocidade_4;
    public bool sim_velocidade_8;
    public bool sim_velocidade_16;
    public bool sim_velocidade_32;

    private GerenciadorDeCena gerCena;

    // Use this for initialization
    void Start () {
        gerCena = GetComponent<GerenciadorDeCena>();
    }
	
	// Update is called once per frame
	void Update () {
        moveUp = Input.GetKey(KeyCode.W);
        moveLeft = Input.GetKey(KeyCode.A);
        moveDown = Input.GetKey(KeyCode.S);
        moveRight = Input.GetKey(KeyCode.D);
        rotClock = Input.GetKey(KeyCode.Q);
        rotCounter = Input.GetKey(KeyCode.E);
        zoomingOut = Input.GetKey(KeyCode.F);
        zoomingIn = Input.GetKey(KeyCode.R);
        shift = Input.GetKey(KeyCode.LeftShift);
        tab = Input.GetKey(KeyCode.Tab);
        sim_parar = Input.GetKey(KeyCode.Escape);
        sim_pausarOuRetomar = Input.GetKey(KeyCode.Comma);
        sim_velocidade_025 = Input.GetKey(KeyCode.Alpha1);
        sim_velocidade_05 = Input.GetKey(KeyCode.Alpha2);
        sim_velocidade_1 = Input.GetKey(KeyCode.Alpha3);
        sim_velocidade_2 = Input.GetKey(KeyCode.Alpha4);
        sim_velocidade_4 = Input.GetKey(KeyCode.Alpha5);
        sim_velocidade_8 = Input.GetKey(KeyCode.Alpha6);
        sim_velocidade_16 = Input.GetKey(KeyCode.Alpha7);
        sim_velocidade_32 = Input.GetKey(KeyCode.Alpha8);

        switch (gerCena.modoDeInput)
        {
            case GerenciadorDeCena.ModoDeInput.BIARTICULADO:
                InputParaBiarticulado();
                break;
            case GerenciadorDeCena.ModoDeInput.CIDADAO:
                InputParaCidadao();
                break;
        }
        InputParaSimulacao();
    }

    private void InputParaBiarticulado()
    {
        //throw new NotImplementedException();
    }

    private void InputParaCidadao()
    {
        //throw new NotImplementedException();
    }

    private void InputParaSimulacao()
    {
        //throw new NotImplementedException();
    }
}