using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;            // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir;   // rotate it
        point = dir + pivot;                    // calculate rotated point
        return point;                           // return it
    }

    public static Vector3 OffsetBetweenTransforms(Transform from, Transform to)
    {
        Vector3 fromPos = from.position;
        Vector3 toPos = to.position;
        Vector3 fromRot = from.rotation.eulerAngles;
        Vector3 toRot = to.rotation.eulerAngles;

        Vector3 fromPosAdjusted = RotatePointAroundPivot(fromPos, Vector3.zero, -fromRot);
        Vector3 toPosAdjusted = RotatePointAroundPivot(toPos, Vector3.zero, -toRot);

        Vector3 result = toPosAdjusted - fromPosAdjusted;
        return result;
    }

    public static bool ValueCloseEnough(float value, float compareTo, float variance)
    {
        return ((value >= compareTo - variance) && (value <= compareTo + variance));
    }

    public static bool AngleCloseEnough(float angle, float compareTo, float variance)
    {
        float newAngle = Mathf.DeltaAngle(angle, compareTo);
        return ((newAngle >= -variance) && (newAngle <= +variance));
    }
}
