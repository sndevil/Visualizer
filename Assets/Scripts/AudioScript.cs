using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.MathHelpers;

public class AudioScript : MonoBehaviour {

	public Camera c;
	AudioClip p, previous;

    public GameObject prefab;

    private List<GameObject> prefabsList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        float linestart = -3.5f;
        float linestep = 0.03f;
        for (int i = 0; i < 512; i++)
        {
            prefabsList.Add(Instantiate(prefab,new Vector3(linestart,0),new Quaternion(0,0,0,0)));
            prefabsList[i].SetActive(true);
            linestart += linestep;
            linestep *= 0.997f;
        }
        Debug.Log (Microphone.devices[1]);
        p = Microphone.Start(Microphone.devices[1], false, 1, 48000);
    }
	// Update is called once per frame
	void Update () {
        //Debug.Log(Microphone.GetPosition(Microphone.devices[1]));
        //if (show)
        p.GetData(whole, 0);
		ShowData ();
        if (!Microphone.IsRecording(Microphone.devices[1]))
        {
            p.GetData(whole, 0);
            idx = 0;
            p = Microphone.Start(Microphone.devices[1], false, 1, 48000);
        }
    }

	float[] whole = new float[48000];
	int idx = 0;	
	Vector3 oldRotate = new Vector3(0,0,0);

	void ShowData () {
		float[] data = new float[1024];
		if (idx >= 47000 || idx >= Microphone.GetPosition(Microphone.devices[1]) - 1024)
			return;
		System.Array.Copy (whole, idx, data, 0, 1024);
		idx += 1024;
        //prefabsList[0].GetComponent<PrefabScript>().ChangeSize(data[20]*20);
        //prefabsList[0].GetComponent<PrefabScript>().ChangeColor(data[20]*20, data[20]*20, data[20]*20);
        //Debug.Log (data[0].ToString());
        //Debug.Log (p.samples.ToString ());
        //FFT.CalculateFFT
        Complex[] cx = FFT.Float2Complex(data);
		FFT.CalculateFFT (cx, false);

		float max = 0;
		for (int i = 0; i < cx.Length / 2; i++) {
			var value = Mathf.Sqrt(cx[i].fMagnitude * 200);
			if (value > max)
				max = value;
			var R = value;
			var G = value;
			var B = value;

			Color t = new Color(R,G,B);
            prefabsList[i].GetComponent<PrefabScript>().ChangeSize(value);
            prefabsList[i].GetComponent<PrefabScript>().ChangeColor(value);

            //Debug.DrawLine (new Vector3(linestart,-linestart,linestart),new Vector3(linestart,-linestart,-linestart),t,1f);
            //Debug.DrawLine (new Vector3 (linestart, -linestart, -linestart), new Vector3 (-linestart, -linestart, -linestart), t, 1f);
            //Debug.DrawLine (new Vector3 (-linestart, -linestart, -linestart), new Vector3 (-linestart, -linestart, linestart), t, 1f);
            //Debug.DrawLine (new Vector3 (-linestart, -linestart, linestart), new Vector3 (linestart, -linestart, linestart), t, 1f);
		}
		max = max * max * max / 50f;
		var currot = c.transform.rotation;
		//Debug.Log ("curz:" + currot.z.ToString ());
		var correction = new Vector3 (0.7f - currot.x, -currot.y, -currot.z);
		correction /= 100f;
		Vector3 newRotate = new Vector3 ((Random.value * Random.value - 0.5f) * max, (Random.value * Random.value - 0.5f) * max, (Random.value * Random.value - 0.5f) * max);
		newRotate = (newRotate + oldRotate) / 2f;
		newRotate = (newRotate + 2 * correction) / 3f;
		//c.transform.Rotate (newRotate);
		oldRotate = newRotate;
	}
}
