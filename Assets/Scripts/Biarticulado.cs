using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biarticulado : MonoBehaviour {

    [Header("Valores estáticos")]
    public string nomeDoVeiculo;
    public float tempoEmPonto_Maximo;
    public float toleranciaCasoAtrasado;
    public float toleranciaCasoAdiantado;
    public float velocidadeDeManobra;
    public float velocidadeMaxima;
    public float velocidadeMaximaAdiantado;
    public float velocidadeMaximaAtrasado;
    public float distanciaDeFrenagem;
    public List<VagaoBiarticulado> vagoes;

    [Header("Valores dinâmicos")]
    public float tempoEmPonto;
    public float tempoAtrasadoOuAdiantado;
    public float velocidadeAtual;
    public float velocidadeLimite;
    public float wpp_distancia;
    public float wpp_angulo;
    public Vector3 wpp_direcao;
    public float wps_distancia;
    public float wps_angulo;
    public Vector3 wps_direcao;

    [Header("Rota e Waypoints")]
    public Rota rota;
    public Rota_Waypoint wp_proximo;
    public Rota_Waypoint wp_seguinte;
    public Rota_Waypoint wp_webService;

    [Header("Condições")]
    public bool estaAdiantado;
    public bool estaAtrasado;
    public bool portasAbertas;
    public bool retomandoTrajeto;
    public bool obstaculoPresente;
    public bool veiculoSendoManobrado;
    public bool veiculoSendoParado;
    public bool parando_Ponto;
    public bool parando_Obstaculo;
    public bool paradoEmPonto;
    public bool paradoEmObstaculo;
    //public bool paradoPorObstaculo;

    // Use this for initialization
    void Start () {
        if (vagoes.Count <= 0)
            vagoes.AddRange(GetComponentsInChildren<VagaoBiarticulado>());
        foreach (var item in vagoes)
            item.biarticulado = this;
        AtualizarRota(null);
    }
	
	// Update is called once per frame
	void Update () {
        AtualizarVariaveisDinamicas();
        VerificarVelocidadeLimite();

        VerificarParadaTotal();
        InteragirComPontoDeParada();

        if (retomandoTrajeto)
            RetomarTrajeto();

        VerificarStatusDaRota();
    }

    void FixedUpdate()
    {
        //Usar este com operações físicas (rigidbody)
    }

    public Rigidbody GetRigidBody()
    {
        Rigidbody result = GetComponent<Rigidbody>();
        if (!result)
            result = GetComponentInChildren<Rigidbody>();
        return result;
    }

    public Transform PosicaoReferencia()
    {
        return transform;
        //return posicaoDeReferencia.transform;
    }

    //Usada logo quando o Biarticulado é inicializado, ou quando ele passa por um Waypoint qualquer.
    public void AtualizarRota(Rota_Waypoint wp)
    {
        if (!wp)
        {
            wp_proximo = rota.ProximoWaypoint(null);
            wp_seguinte = rota.ProximoWaypoint(wp_proximo);
        }
        else if (wp == wp_proximo)
        {
            if (wp.tipoWaypoint == CVP.ModuloOnibus.TipoWaypont.PARADA && !paradoEmPonto)
            {
                veiculoSendoParado = true;
                parando_Ponto = true;
            }
            else
            {
                wp_proximo = rota.ProximoWaypoint(wp_proximo);
                wp_seguinte = rota.ProximoWaypoint(wp_proximo);
            }
        }
    }

    private void AtualizarVariaveisDinamicas()
    {
        Vector3 wppPos = (wp_proximo ? wp_proximo.transform.position : Vector3.zero);
        Vector3 wpsPos = (wp_seguinte ? wp_seguinte.transform.position : Vector3.zero);

        wpp_distancia = (wp_proximo ? Vector3.Distance(PosicaoReferencia().position, wppPos) : float.NaN);
        wpp_angulo = (wp_proximo ? Vector3.Angle(wppPos - PosicaoReferencia().position, PosicaoReferencia().forward) : float.NaN);
        wpp_direcao = (wp_proximo ? (wppPos - PosicaoReferencia().position).normalized : Vector3.zero);

        wps_distancia = (wp_seguinte ? Vector3.Distance(PosicaoReferencia().position, wpsPos) : float.NaN);
        wps_angulo = (wp_seguinte ? Vector3.Angle(wpsPos - PosicaoReferencia().position, PosicaoReferencia().forward) : float.NaN);
        wps_direcao = (wp_seguinte ? (wpsPos - PosicaoReferencia().position).normalized : Vector3.zero);
    }

    private void VerificarStatusDaRota()
    {
        if (!wp_proximo)
        {
            Debug.Log("O veículo " + nomeDoVeiculo + " finalizou a sua rota.");
            return;
        }
    }

    private void VerificarVelocidadeLimite()
    {
        if (!wp_proximo)
        {
            return;
        }

        velocidadeAtual = GetComponentInChildren<Rigidbody>().velocity.magnitude * 3.6F;

        //TODO adicionar checagem se tal obstáculo está realmente próximo
        if (wp_proximo.tipoWaypoint == CVP.ModuloOnibus.TipoWaypont.PARADA || obstaculoPresente)
            veiculoSendoManobrado = true;
        else
            veiculoSendoManobrado = false;

        if (tempoAtrasadoOuAdiantado < toleranciaCasoAdiantado * -1)
            estaAdiantado = true;
        else
            estaAdiantado = false;

        if (tempoAtrasadoOuAdiantado > toleranciaCasoAtrasado)
            estaAtrasado = true;
        else
            estaAtrasado = false;

        if (veiculoSendoParado || !wp_proximo)          //OU está parando (por algum motivo) OU não tem mais pra onde ir
            velocidadeLimite = 0;
        else if (veiculoSendoManobrado)                 //Está manobrando, logo deve se mover com parcimônia
            velocidadeLimite = velocidadeDeManobra;
        else if (estaAdiantado)                         //Está ADIANTADO (valor negativo) na rota que está seguindo, logo deve ir mais devagar
            velocidadeLimite = velocidadeMaximaAdiantado;
        else if (estaAtrasado)                          //Está ATRASADO (valor positivo) na rota que está seguindo, logo deve ir mais rápido
            velocidadeLimite = velocidadeMaximaAtrasado;
        else                                            //Nenhuma condição está afetando a velocidade máxima que o veículo deve trafegar
            velocidadeLimite = velocidadeMaxima;
    }

    private void VerificarParadaTotal()
    {
        if (veiculoSendoParado && velocidadeAtual < 0.1F)
        {
            if (parando_Ponto)
            {
                paradoEmPonto = true;
                AbrirPortas();
            }
            else if (parando_Obstaculo)
            {
                paradoEmObstaculo = true;
            }
        }
    }

    private void AbrirPortas()
    {
        foreach (var item in vagoes)
            item.AnimarPortas(true, wp_proximo.usarTodasAsPortas);
        portasAbertas = true;
    }

    private void InteragirComPontoDeParada()
    {
        if (!paradoEmPonto)
            return;

        if (tempoEmPonto >= tempoEmPonto_Maximo)
            FecharPortas();
        else
            tempoEmPonto += Time.deltaTime;
    }

    private void FecharPortas()
    {
        foreach (var item in vagoes)
            item.AnimarPortas(false, wp_proximo.usarTodasAsPortas);
        portasAbertas = false;

        tempoEmPonto = 0;
        retomandoTrajeto = true;
    }

    private void RetomarTrajeto()
    {
        int animacoesEmAndamento = 0;
        foreach (var item in vagoes)
            animacoesEmAndamento += (item.VerificaAnimacaoSendoExecutada() ? 1 : 0);
        if (animacoesEmAndamento > 0)
            return;

        retomandoTrajeto = false;

        AtualizarRota(wp_proximo);
        paradoEmPonto = false;
        parando_Ponto = false;
        veiculoSendoParado = false;
    }

    public void Movimentar(float thr_acceleration, float thr_steering, float thr_handbrake)
    {
        //Foi descoberto empiricamente que, mesmo que todos os inputs sejam zeros, o veículo ficará "tremendo" no próprio lugar.
        //Logo, aqui está sendo forçada uma parada toda do veículo.
        if (thr_acceleration == 0 && thr_steering == 0 && velocidadeAtual < 0.1F)
        {
            foreach (VagaoBiarticulado v in vagoes)
            {
                v.GetComponent<Rigidbody>().velocity = Vector3.zero;
                v.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        else
        {
            if (velocidadeAtual >= velocidadeLimite || veiculoSendoParado)
                thr_acceleration = 0;

            if (veiculoSendoParado)
                thr_steering = 0;

            foreach (VagaoBiarticulado v in vagoes)
            {
                if (v.acelerarSomenteParaPartir && velocidadeAtual >= 1)
                    v.Movimentar(0, thr_steering, thr_handbrake);
                else
                    v.Movimentar(thr_acceleration, thr_steering, thr_handbrake);
            }
        }
    }

    public string Status()
    {
        string retorno = "Em trânsito";
        if (paradoEmPonto || paradoEmObstaculo)// || paradoPorObstaculo)
        {
            if (paradoEmPonto)
            {
                retorno = "Parado em Ponto";
                if (portasAbertas)
                    retorno += " (" + (tempoEmPonto_Maximo - tempoEmPonto) + ")";
            }
            else if (paradoEmObstaculo)
                retorno = "Parado em Obstáculo";
            //else if (paradoPorObstaculo)
            //    retorno = "Parado por Obstáculo";
        }
        else
        {
            if (veiculoSendoManobrado)
                retorno += " / Manobrando";
            else if (veiculoSendoParado)
                retorno += " / Parando";
        }

        if (estaAdiantado)
            retorno += " [Adiantado]";
        else if (estaAtrasado)
            retorno += " [Atrasado]";
        return retorno;
    }
}
