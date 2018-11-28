using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleAutomatico : MonoBehaviour {
    public Biarticulado biarticulado;
    [Header("Input Gerado")]
    public float vertical;
    public float horizontal;
    public float freio;

    [Header("Parametros Dinâmicos")]
    public float dyn_distParaAlvo;
    public float dyn_distDeParada;
    public float dyn_Fremagem;
    public float VALOR;

    // Use this for initialization
    void Start () {
        biarticulado = GetComponent<Biarticulado>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        ParametrosDinamicos();
        DefineMotorThreshold();
        DefineSteeringAngle();
        DefineBrakeTorque();
        biarticulado.Movimentar(vertical, horizontal, freio);
    }

    private void ParametrosDinamicos()
    {
        dyn_distParaAlvo = biarticulado.wpp_distancia;
        dyn_distDeParada = CVP_Calculos.DistanciaDeParada(biarticulado);
        //dyn_Fremagem = CVP_Calculos.Frenagem(biarticulado);
    }

    private void DefineMotorThreshold()
    {
        if (biarticulado.veiculoSendoParado)
        {
            vertical = 0;
            return;
        }

        if (dyn_distParaAlvo > dyn_distDeParada && biarticulado.velocidadeAtual < biarticulado.velocidadeLimite)
            vertical = 1;
        else
            vertical = 0;
    }

    private void DefineSteeringAngle()
    {
        if (!biarticulado.wp_proximo || biarticulado.veiculoSendoParado)
        {
            horizontal = 0;
            return;
        }

        // calculate the local-relative position of the target, to steer towards
        Vector3 targetPos = biarticulado.PosicaoReferencia().InverseTransformPoint(biarticulado.wp_proximo.transform.position);
        // work out the local angle towards the target
        float targetAngle = Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg;
        // get the amount of steering needed to aim the car towards the target

        VALOR = targetAngle;
        horizontal = targetAngle / CVP_Calculos.MenorValor_SteeringAngle(biarticulado);
        horizontal = Mathf.Clamp(horizontal, -1, 1); // * Mathf.Sign(m_CarController.CurrentSpeed);
    }

    private void DefineBrakeTorque()
    {
        if (biarticulado.veiculoSendoParado)
        {
            freio = 1;
            return;
        }

        if (dyn_distParaAlvo > dyn_distDeParada && biarticulado.velocidadeAtual < biarticulado.velocidadeLimite)
            freio = 0;
        else
            freio = 1;
    }
}
