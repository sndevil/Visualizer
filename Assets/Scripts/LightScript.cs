using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    public Material LightMaterial;

    public float h = 0, s = 0.5f, v = 0;

    public void Update()
    {

        Color c = Color.HSVToRGB(h, s, v);
        LightMaterial.SetColor("_EmissionColor", c);
    }

    public void ChangeColor(float amplitude, float freqstart, float freqend)
    {
        h = Mathf.Sqrt(Mathf.Sqrt(freqstart / 20000f)) - amplitude / 100f;

        v = amplitude;
        s = 1f - amplitude;
    }
}
