//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BusObstacleChecker : MonoBehaviour
//{
//    private Bus bus;
//    private new Collider collider;

//    // Use this for initialization
//    void Start()
//    {
//        bus = GetComponentInParent<Bus>();
//        collider = GetComponent<Collider>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector3 newScale = collider.transform.localScale;
//        newScale.z = bus.stoppingDistance * 2;
//        collider.transform.localScale = newScale;
//    }

//    //private void OnTriggerEnter(Collider other)
//    private void OnTriggerStay(Collider other)
//    {
//        Semaphore_Wrapper sw = other.gameObject.GetComponent<Semaphore_Wrapper>();
//        if (sw)
//        {
//            Semaphore s = sw.getSemaphore();
//            Vector3 v1 = bus.transform.rotation.eulerAngles;
//            Vector3 v2 = s.transform.rotation.eulerAngles;

//            Vector3 busToSemaphoreDirection = (v2 - v1);
//            //bool sameDirection = HelperFunctions.ValueCloseEnough(v1.y, v2.y, 15);
//            bool sameDirection = HelperFunctions.AngleCloseEnough(v1.y, v2.y, 15);
//            bool behindObstacle = (HelperFunctions.OffsetBetweenTransforms(bus.transform, s.transform).z >= 0);

//            ////float dotDirections = Vector3.Dot(direction, bus.transform.forward);
//            ////float dotPositions = Vector3.Dot(v1.normalized, v2.normalized);
//            ////float angle = Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * 180 / Mathf.PI;
//            //Debug.Log("getSemaphore() = " + s.identifier + " v2 - v1 = " + (v2 - v1));      //      <<<< THIS
//            ////Debug.Log("getSemaphore() = " + s.identifier + " angle = " + angle + " | v1 = " + v1 + " | v2 = " + v2 + " | dotDirections = " + dotDirections + " | dotPositions = " + dotPositions);
//            //if (dotDirections >= 0.25F && dotPositions >= 0.95F)

//            if (sameDirection && behindObstacle && !s.isOpen)// && HelperFunctions.ValueCloseEnough(v2.y, v1.y, 15))
//            {
//                bus.considerObstacle(s.gameObject);
//            }
//            else
//            {
//                bus.disconsiderObstacle(s.gameObject);
//            }
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        Semaphore_Wrapper sw = other.gameObject.GetComponent<Semaphore_Wrapper>();
//        if (sw)
//        {
//            Semaphore s = sw.getSemaphore();
//            bus.disconsiderObstacle(s.gameObject);
//        }
//    }
//}
