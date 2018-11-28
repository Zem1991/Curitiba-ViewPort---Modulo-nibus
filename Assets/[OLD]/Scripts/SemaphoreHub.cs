using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemaphoreHub : MonoBehaviour {
    public List<Semaphore> startsOpen;
    public List<Semaphore> startsClosed;
    public float semaphoreTimer, semaphoreTimerMax;

    // Use this for initialization
    void Start () {
        foreach (Semaphore item in startsOpen)
        {
            item.isOpen = true;
            //item.isOpen = false;
        }
        foreach (Semaphore item in startsClosed)
        {
            item.isOpen = false;
        }
    }
	
	// Update is called once per frame
	void Update () {        
        if (semaphoreTimer <= 0)
        {
            invertSemaphores();
            semaphoreTimer = semaphoreTimerMax;
        }
        else
        {
            semaphoreTimer -= Time.deltaTime;
        }
    }

    private void invertSemaphores()
    {
        foreach (Semaphore item in GetComponentsInChildren<Semaphore>())
        {
            if (item.isOpen)
            {
                item.isOpen = false;
            }
            else
            {
                item.isOpen = true;
            }
        }
    }
}
