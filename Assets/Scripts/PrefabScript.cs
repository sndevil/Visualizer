using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour {

    public GameObject shownObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeSize(float size) {
        size = Mathf.Abs(size);
        shownObject.transform.localScale = new Vector3(1, size, size);
    }

    public void ChangeColor(float R, float G, float B, float A = 1f)
    {
        shownObject.GetComponent<Renderer>().material.color = new Color(R, G, B, A);
    }

    public void ChangeColor(float Val)
    {
        float scale = Val * Val;
        Val *= 10f;
        int val = (int)(Val * 256 * 256 * 256);
        int B = (int)((val % 256) * scale);
        int G = (int)((val / 256) % 256 * scale);
        int R = (int)(val / 256 * scale);
        shownObject.GetComponent<Renderer>().material.color = new Color(1f - (256f / R),1f - (256f / G), 1f - (256f / B));

    }
}
