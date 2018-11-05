using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.MathHelpers;

public class AudioScript : MonoBehaviour {
    public GameObject[] Cameras;
    public int CurrentCamera = 0;

    const int SampleRate = 48000;
    const int BufferSize = 1024;

    const int AudioDevice = 1;

    float InputGain = 2;

    public Camera c;
	AudioClip p, previous;

    public GameObject prefab;

    private List<GameObject> prefabsList = new List<GameObject>();

    public int CameraCountdown = 300;

    // Use this for initialization
    void Start () {
        foreach (string t in Microphone.devices)
            Debug.Log(t);
        Physics.gravity.Set(0, -9.8f, 0);
        Debug.Log ("Connected To: " + Microphone.devices[AudioDevice]);
        p = Microphone.Start(Microphone.devices[AudioDevice], false, 1, SampleRate);
    }
    // Update is called once per frame

    int framescape = 0;
	void Update () {

    }

    private void FixedUpdate()
    {
        if (framescape++ > 30)
        {
            p.GetData(whole, 0);
            ShowData();
        }
        if (CameraCountdown-- < 0)
        {
            CameraCountdown = (int)Random.Range(60, 500);
            ChangeCamera();
        }
        if (!Microphone.IsRecording(Microphone.devices[AudioDevice]))
        {
            p.GetData(whole, 0);
            idx = 0;
            p = Microphone.Start(Microphone.devices[AudioDevice], false, 1, SampleRate);
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
        Complex[] cx = FFT.Float2Complex(data);
		FFT.CalculateFFT (cx, false);
        float linestart = -3.5f;
        float linestep = 0.1f;
        float max = 0;
        float fstep = 1f;
        float fstart = 0;
        for (int i = 0; i < cx.Length; i=(int)fstart) {
            fstart += fstep;
            fstep *= 1.9f;
            var value = cx[i].fMagnitude*20f * InputGain;
			if (value > max)
				max = value;
            if (value > 0.01f)
            {
                float rotationValue = value;
                float x = linestart;
                float z = value * linestart / 2;
                float sinRotation = Mathf.Sin(rotationValue);
                float cosRotation = Mathf.Cos(rotationValue);
                float xrotated = cosRotation * x + sinRotation * z;
                float zrotated = cosRotation * z - sinRotation * x;


                Vector3 rotated = new Vector3(xrotated, 0, zrotated);

                GameObject tempObject = Instantiate(prefab, rotated, new Quaternion(0, 90, 0, 0));
                PrefabScript tempScript = tempObject.GetComponent<PrefabScript>();
                tempScript.shownObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                tempScript.ChangeSize(value * 30);
                tempScript.ChangeColor(value, (int)(i * freqStep));
                tempObject.SetActive(true);
            }
            linestart += linestep;
            //linestep *= 0.997f;

        }
	}

    public void ChangeCamera()
    {
        int next = CurrentCamera;
        while (next == CurrentCamera)
            next = Random.Range(0, Cameras.Length);
        for (int i = 0; i < Cameras.Length; i++)
            Cameras[i].SetActive(i == next);
        CurrentCamera = next;
    }
}
