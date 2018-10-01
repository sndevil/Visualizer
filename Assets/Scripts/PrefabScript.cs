using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour {

    public GameObject shownObject;
    private float speed = 0.05f;
    // Use this for initialization
    int thrust = 1;
	void Start () {
        //gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1f));
        //gameObject.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);
        Destroy(this.gameObject, 5f);
	}

    public Color CurrentColor;// = new Color(0.5f,0.5f,0.5f,1);
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = gameObject.transform.position - new Vector3(0, 0, speed);
        //CurrentColor = shownObject.GetComponent<Renderer>().material.GetColor("_Color");
        /*CurrentColor.a -= 0.05f;
        shownObject.GetComponent<Renderer>().material.color = CurrentColor;
        if (CurrentColor.a <= 0f){
            Destroy(this.gameObject);
            //Debug.Log("deleting");
        }*/
    }

    public void ChangeSize(float size) {
        size = Mathf.Abs(size);
        shownObject.transform.localScale = new Vector3(1 ,size, 1);
    }

    public void ChangeColor(float R, float G, float B, float A = 1f)
    {
        shownObject.GetComponent<Renderer>().material.color = new Color(R, G, B, A);
    }

    public void ChangeColor(float Val, int freq)
    {
        /*float scale = Val * Val;
        Val *= 10f;
        int val = (int)(Val * 256 * 256 * 256);
        int B = (int)((val % 256) * scale);
        int G = (int)((val / 256) % 256 * scale);
        int R = (int)(val / 256 * scale);*/
        CurrentColor = valueToColor(Val,freq);//new Color(1f - (256f / R), 1f - (256f / G), 1f - (256f / B));
        shownObject.GetComponent<Renderer>().material.color = CurrentColor;

    }

    private Color valueToColor(float value, int freq) {
        Color output;
        float value2 = value * value;
        float value3 = value * value * value;
        if (freq > 5000) // Violet
            output = new Color(value, value3, value);
        else if (freq > 2000) // Blue
            output = new Color(value2, value3, value);
        else if (value > 600) // Green
            output = new Color(value2, value, value2);
        else if (value > 200) // Yellow
            output = new Color(value2, value2, value3);
        else // Red
            output = new Color(value, value3,value3);
        return output;
    }
}
