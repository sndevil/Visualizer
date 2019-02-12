using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    public Material LightMaterial;
    public List<LightArrayScript> RealLights;

    public float h = 0, s = 0.5f, v = 0;

    float hmax = 1f, hmin = 0f, dh = 0.0001f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Color c = Color.HSVToRGB(h, s, v);
       LightMaterial.SetColor("_EmissionColor", c);
        foreach (LightArrayScript t in RealLights)
        {
            t.setColor(c);
            t.setIntensity(v);
        }
        //LightMaterial.color = Color.HSVToRGB(h,s,v);

        if (h > hmax || h < hmin)
            dh *= -1;
        h += dh;
        //Debug.Log("Min:" + hmax.ToString() + "  ," + h.ToString());

	}

    public void ChangeColor(float amplitude, float freqstart, float freqend)
    {
        //print(freqstart);
        //print(amplitude);
        //amplitude *= 500f;
        hmax = Mathf.Sqrt(Mathf.Sqrt(freqend / 20000f)) + amplitude/100f;
        hmin = Mathf.Sqrt(Mathf.Sqrt(freqstart / 20000f)) - amplitude/100f;
        h = hmin;

        v = amplitude;
        s = 1f - amplitude;
    }
}
