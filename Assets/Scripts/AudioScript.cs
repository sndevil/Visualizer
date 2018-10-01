using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.MathHelpers;

public class AudioScript : MonoBehaviour {
    const int SampleRate = 48000;
    const int BufferSize = 1024;

    public Camera c;
	AudioClip p, previous;

    public GameObject prefab;

    private List<GameObject> prefabsList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        Physics.gravity.Set(0, -9.8f, 0);
        Debug.Log (Microphone.devices[1]);
        p = Microphone.Start(Microphone.devices[1], false, 1, SampleRate);
    }
    // Update is called once per frame

    int framescape = 0;
	void Update () {
        //Debug.Log(Microphone.GetPosition(Microphone.devices[1]));
        //if (show)
        if (framescape++ > 10)
        {
            p.GetData(whole, 0);
            ShowData();
        }
        if (!Microphone.IsRecording(Microphone.devices[1]))
        {
            p.GetData(whole, 0);
            idx = 0;
            p = Microphone.Start(Microphone.devices[1], false, 1, SampleRate);
        }
    }

	float[] whole = new float[SampleRate];
	int idx = 0;	
	Vector3 oldRotate = new Vector3(0,0,0);
    float freqStep = SampleRate / BufferSize;

	void ShowData () {
		float[] data = new float[BufferSize];
		if (idx >= SampleRate - BufferSize || idx >= Microphone.GetPosition(Microphone.devices[1]) - BufferSize)
			return;
		System.Array.Copy (whole, idx, data, 0, BufferSize);
		idx += BufferSize;
        //prefabsList[0].GetComponent<PrefabScript>().ChangeSize(data[20]*20);
        //prefabsList[0].GetComponent<PrefabScript>().ChangeColor(data[20]*20, data[20]*20, data[20]*20);
        //Debug.Log (data[0].ToString());
        //Debug.Log (p.samples.ToString ());
        //FFT.CalculateFFT
        Complex[] cx = FFT.Float2Complex(data);
		FFT.CalculateFFT (cx, false);
        float linestart = -3.5f;
        float linestep = 0.03f;
        float max = 0;
        float fstep = 1f;
        float fstart = 0;
        for (int i = 0; i < cx.Length / 2; i=(int)fstart) {
            fstart += fstep;
            fstep *= 1.05f;
            var value = cx[i].fMagnitude*20f;//Mathf.Sqrt(cx[i].fMagnitude * 200);
			if (value > max)
				max = value;
            GameObject tempObject = Instantiate(prefab, new Vector3(linestart, 0), new Quaternion(0, 90, 0, 0));
            PrefabScript tempScript = tempObject.GetComponent<PrefabScript>();
            tempScript.shownObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            tempScript.ChangeSize(value*20);
            tempScript.ChangeColor(value,(int)(i*freqStep));
            tempObject.SetActive(true);

            linestart += linestep;
            //linestep *= 0.997f;

        }
		//max = max * max * max / 50f;
		//var currot = c.transform.rotation;
		//Debug.Log ("curz:" + currot.z.ToString ());
		//var correction = new Vector3 (0.7f - currot.x, -currot.y, -currot.z);
		//correction /= 100f;
		//Vector3 newRotate = new Vector3 ((Random.value * Random.value - 0.5f) * max, (Random.value * Random.value - 0.5f) * max, (Random.value * Random.value - 0.5f) * max);
		//newRotate = (newRotate + oldRotate) / 2f;
		//newRotate = (newRotate + 2 * correction) / 3f;
		//c.transform.Rotate (newRotate);
		//oldRotate = newRotate;
	}
}
