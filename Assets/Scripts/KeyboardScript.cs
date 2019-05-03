using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardScript : MonoBehaviour {


    public List<GameObject> Cubes; 

	// Use this for initialization
	void Start () {
        for (int i = 0; i < Cubes.Count; i++)
            Cubes[i].SetActive(i == 0);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            for (int i = 0; i < Cubes.Count; i++)
                Cubes[i].SetActive(i == 0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            for (int i = 0; i < Cubes.Count; i++)
                Cubes[i].SetActive(i == 1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            for (int i = 0; i < Cubes.Count; i++)
                Cubes[i].SetActive(i == 2);

    }
}
