using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsUpDisplay : MonoBehaviour {
    public HUDPanel_TimeDisplay timeDisplay;
    public HUDPanel_RouteData routeData;
    public HUDPanel_BusData busData;

    private SceneManager sceneManager;

	// Use this for initialization
	void Start () {
        sceneManager = GetComponentInParent<SceneManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public SceneManager getSceneManager()
    {
        return sceneManager;
    }
}
