using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArrayScript : MonoBehaviour {


    public List<Light> lights;

    public void setColor(Color t)
    {
        foreach (Light L in lights)
            L.color = t;
    }
    public void setIntensity(float t)
    {
        foreach (Light L in lights)
            L.intensity = t;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}