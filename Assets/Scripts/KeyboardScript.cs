using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardScript : MonoBehaviour {


    public List<GameObject> Cubes;

    int changetimer = 30;
    int framecounter = 0;

	// Update is called once per frame
	void Update () {
        if (framecounter-- < 0)
        {
            framecounter = changetimer * 30;
            ActivateCamera(Random.Range(0, 3));
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ActivateCamera(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ActivateCamera(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ActivateCamera(2);

    }

    void ActivateCamera(int index)
    {
        for (int i = 0; i < Cubes.Count; i++)
            Cubes[i].SetActive(i == index);
    }
}
