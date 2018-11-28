using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAxle : MonoBehaviour {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;              // is this wheel attached to motor?
    public bool steering;           // does this wheel apply steer angle?
    public bool brake;
    //public bool reverseMotor;
    //public bool reverseSteering;

    [Header("Roda Esquerda")]
    public float leftWheelMotorTorque;
    public float leftWheelSteerAngle;
    public float leftWheelBrakeTorque;
    public float leftWheelRPM;

    [Header("Roda Direita")]
    public float rightWheelMotorTorque;
    public float rightWheelSteerAngle;
    public float rightWheelBrakeTorque;
    public float rightWheelRPM;

    void Start()
    {
        leftWheel.ConfigureVehicleSubsteps(5, 12, 15);
        rightWheel.ConfigureVehicleSubsteps(5, 12, 15);
    }

    void FixedUpdate()
    {
        ApplyLocalPositionToVisuals(leftWheel);
        ApplyLocalPositionToVisuals(rightWheel);

        leftWheelMotorTorque = leftWheel.motorTorque;
        rightWheelMotorTorque = rightWheel.motorTorque;
        leftWheelSteerAngle = leftWheel.steerAngle;
        rightWheelSteerAngle = rightWheel.steerAngle;
        leftWheelBrakeTorque = leftWheel.brakeTorque;
        rightWheelBrakeTorque = rightWheel.brakeTorque;
        leftWheelRPM = leftWheel.rpm;
        rightWheelRPM = rightWheel.rpm;
    }

    // Finds the corresponding visual wheel and correctly applies the transform.
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
            return;

        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
