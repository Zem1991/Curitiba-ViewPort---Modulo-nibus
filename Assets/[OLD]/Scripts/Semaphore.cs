using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore : MonoBehaviour {
    public bool isOpen;
    public string identifier;

    private Semaphore_Stoppoint stoppoint;

    // Use this for initialization
	void Start () {
        stoppoint = GetComponentInChildren<Semaphore_Stoppoint>();
    }
	
	// Update is called once per frame
	void Update () {
        foreach (Renderer item in GetComponentsInChildren<Renderer>())
        {
            if (isOpen)
            {
                item.material.color = Color.green;
            }
            else
            {
                item.material.color = Color.red;
            }
        }
	}

    public Vector3 getStopPoint()
    {
        return stoppoint.transform.position;
    }
}
