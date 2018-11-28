using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagaoBiarticulado : MonoBehaviour {
    [Header("Animações")]
    public Animator animator;
    public int[] idPortas;
    public int[] idPortasSempreUsadas;

    [Header("Valores estáticos")]
    public Biarticulado biarticulado;
    public Vector3 centroDeMassa;
    public float maxMotorTorque;        // maximum torque the motor can apply to wheel
    public float maxSteeringAngle;      // maximum steer angle the wheel can have
    public float maxBrakeTorque;        // maximum torque applied to wheels when braking
    public bool canSteer;
    public List<VehicleAxle> axles;     // the information about each individual axle
    public bool acelerarSomenteParaPartir;

    [Header("Valores dinâmicos")]
    public float motorTorque;
    public float steeringAngle;
    public float brakeTorque;

    private Rigidbody rigidBody;

    // Use this for initialization
    void Start () {
        if (axles.Count <= 0)
            axles.AddRange(GetComponentsInChildren<VehicleAxle>());

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rigidBody.centerOfMass = centroDeMassa;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Movimentar(float thr_acceleration, float thr_steering, float thr_handbrake)
    {
        foreach (VehicleAxle axleInfo in axles)
        {
            if (axleInfo.motor)
            {
                motorTorque = thr_acceleration * maxMotorTorque;
                axleInfo.leftWheel.motorTorque = motorTorque;
                axleInfo.rightWheel.motorTorque = motorTorque;
            }
            if (canSteer && axleInfo.steering)
            {
                steeringAngle = thr_steering * maxSteeringAngle;
                axleInfo.leftWheel.steerAngle = steeringAngle;
                axleInfo.rightWheel.steerAngle = steeringAngle;
            }
            if (axleInfo.brake)
            {
                brakeTorque = thr_handbrake * maxBrakeTorque;
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }
        }
    }

    public void AnimarPortas(bool abrir, bool usarTodasAsPortas)
    {
        if (!animator)
        {
            Debug.LogWarning("Nenhum objeto Animator foi encontrado para um VagaoBiarticulado!");
            return;
        }

        animator.SetBool((abrir ? "abrirPortas" : "fecharPortas"), true);
        animator.SetBool((!abrir ? "abrirPortas" : "fecharPortas"), false);

        foreach (var i in idPortas)
        {
            animator.SetFloat("porta" + i, 0);

            if (usarTodasAsPortas)
            {
                animator.SetFloat("porta" + i, 1);
            }
            else
            {
                foreach (var j in idPortasSempreUsadas)
                {
                    if (i == j)
                        animator.SetFloat("porta" + i, 1);
                }
            }
        }
        
    }

    public bool VerificaAnimacaoSendoExecutada()
    {
        if (!animator)
        {
            Debug.LogWarning("Nenhum objeto Animator foi encontrado para um VagaoBiarticulado!");
            return false;
        }
        return animator.IsInTransition(0);
    }
}
