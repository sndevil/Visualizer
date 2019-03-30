using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardScript : MonoBehaviour {


    public List<Camera> cameras; 

	// Use this for initialization
	void Start () {
        for (int i = 0; i < cameras.Count; i++)
            cameras[i].gameObject.SetActive(i == 0);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            for (int i = 0; i < cameras.Count; i++)
                cameras[i].gameObject.SetActive(i == 0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            for (int i = 0; i < cameras.Count; i++)
                cameras[i].gameObject.SetActive(i == 1);

    }
}
