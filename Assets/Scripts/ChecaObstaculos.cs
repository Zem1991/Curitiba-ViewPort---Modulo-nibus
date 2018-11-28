using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChecaObstaculos : MonoBehaviour {
    [Header("Valores estáticos")]
    public Biarticulado biarticulado;
    public ControleAutomatico conAuto;
    public float anguloDeVarredura;
    public float distanciaDeSeguranca;

    [Header("Valores dinâmicos")]
    //public GameObject obstaculoMaisProximo;
    public Semaforo semaforoProximo;
    public float semaforo_angulo;
    public float semaforo_dot;
    public Semaforo semaforoParadaAtual;

    // Use this for initialization
    void Start () {
        biarticulado = GetComponent<Biarticulado>();
        conAuto = GetComponent<ControleAutomatico>();

        if (!conAuto)
        {
            Debug.LogWarning("ChecaObstaculos não pode ser utilizado sem um ControleAutomatico, e por isso foi DESTRUIDO!");
            Destroy(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
        ProcuraObstaculos();
        //IdentificaObstaculoMaisProximo();
        ReagirAosObstaculos();
	}

    private void ProcuraObstaculos()
    {
        Collider[] colliders = Physics.OverlapSphere(biarticulado.PosicaoReferencia().position, Mathf.Max(biarticulado.distanciaDeFrenagem, conAuto.dyn_distDeParada));
        List<Semaforo> semaforos = new List<Semaforo>();

        foreach (var item in colliders)
        {
            semaforoProximo = item.GetComponent<Semaforo>();
            if (!semaforoProximo)
                semaforoProximo = item.GetComponentInParent<Semaforo>();
            if (semaforoProximo)
                semaforos.Add(semaforoProximo);
        }

        FiltraSemaforos(semaforos);
    }

    private void FiltraSemaforos(List<Semaforo> semaforos)
    {
        if (semaforos.Count <= 0)
            return;

        semaforos = semaforos.OrderBy(x => Vector3.Distance(biarticulado.PosicaoReferencia().position, x.transform.position)).ToList();
        foreach (var item in semaforos)
        {
            semaforo_angulo = Vector3.Angle(biarticulado.PosicaoReferencia().forward, item.transform.forward);
            semaforo_dot = Vector3.Dot((item.transform.position - biarticulado.PosicaoReferencia().position).normalized, biarticulado.PosicaoReferencia().forward);
            if (semaforo_angulo <= anguloDeVarredura &&
                semaforo_dot > 0)
            {
                semaforoProximo = item;
                break;
            }
        }

        if (!semaforoProximo)
        {
            semaforo_angulo = float.NaN;
            semaforo_dot = float.NaN;
        }
    }

    //private void IdentificaObstaculoMaisProximo()
    //{

    //}

    private void ReagirAosObstaculos()      //TODO ADICIONAR FUTURAMENTE FORMAS DE REAGIR A DIVERSOS OUTROS TIPOS DE OBSTÁCULOS
    {
        if (!biarticulado.paradoEmPonto)
        {
            if (semaforoProximo)
            {
                if (semaforoProximo.estaAberto)
                {
                    biarticulado.obstaculoPresente = false;
                    biarticulado.veiculoSendoParado = false;
                    biarticulado.parando_Obstaculo = false;
                }
                else
                {
                    biarticulado.obstaculoPresente = true;
                    if (Vector3.Distance(biarticulado.PosicaoReferencia().position, semaforoProximo.transform.position) <= distanciaDeSeguranca)
                    {
                        biarticulado.veiculoSendoParado = true;
                        biarticulado.parando_Obstaculo = true;
                        semaforoParadaAtual = semaforoProximo;
                    }
                    else
                    {
                        biarticulado.veiculoSendoParado = false;
                        biarticulado.parando_Obstaculo = false;
                    }
                }
            }

            //Este if serve para impedir que o véículo pare no meio de um cruzamento quando o semáforo que acabou de atravessar se fechar durante a travessia.
            if (biarticulado.obstaculoPresente && ((semaforoParadaAtual && semaforoParadaAtual.estaAberto) || !semaforoProximo))
            {
                semaforoParadaAtual = null;
                biarticulado.obstaculoPresente = false;
                if (biarticulado.veiculoSendoParado)
                    biarticulado.veiculoSendoParado = false;
            }
        }
    }
}
