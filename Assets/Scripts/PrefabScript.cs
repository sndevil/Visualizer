using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour {

    public GameObject shownObject;
    public Light lightObject;

    private float speed = 0.04f;
    // Use this for initialization
	void Start () {
        Destroy(this.gameObject, 3f);
	}

    public Color CurrentColor;

	void Update () {
        //gameObject.transform.
        gameObject.transform.position = gameObject.transform.position - new Vector3(0, speed, 0);

    }

    public void ChangeSize(float size) {
        size = (2 * Mathf.Sqrt(Mathf.Abs(size))+ Mathf.Abs(size)) / 2f;
        var diameter = Random.Range(0.5f, 2.5f);
        shownObject.transform.localScale = new Vector3(diameter, size, diameter);
    }

    public void ChangeColor(float R, float G, float B, float A = 1f)
    {
        shownObject.GetComponent<Renderer>().material.color = new Color(R, G, B, A);
    }

    public void ChangeColor(float Val, int freq)
    {
        CurrentColor = valueToColor(Val,freq);
        shownObject.GetComponent<Renderer>().material.color = CurrentColor;
        lightObject.intensity = Val*Val*1.5f;
    }

    private Color valueToColor(float value, int freq) {
        if (value > 0.9f)
            value *= value;
        if (value > 1)
            value = 0.95f;
        Color output;
        float value2 = value * Random.Range(0,value*2);
        float value3 = value2 * Random.Range(0, value * 2);
        if (freq > 5000) // Violet
            output = new Color(value, value3, value);
        else if (freq > 2000) // Blue
            output = new Color(value3, value3, value);
        else if (value > 800) // Green
            output = new Color(value3/2, value, value3/2);
        else if (value > 200) // Yellow
            output = new Color(value, value, value3);
        else // Red
            output = new Color(value, value3,value3);
        return output;
    }
}
