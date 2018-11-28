using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus_Wrapper : MonoBehaviour {
    private Bus bus;

    // Use this for initialization
    void Start () {
        bus = GetComponentInParent<Bus>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Bus getBus()
    {
        return bus;
    }
}
