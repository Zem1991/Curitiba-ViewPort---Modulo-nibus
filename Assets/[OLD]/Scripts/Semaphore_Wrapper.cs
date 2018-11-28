using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore_Wrapper : MonoBehaviour {
    private Semaphore semaphore;

    // Use this for initialization
    void Start () {
        semaphore = GetComponentInParent<Semaphore>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Semaphore getSemaphore()
    {
        return semaphore;
    }
}
