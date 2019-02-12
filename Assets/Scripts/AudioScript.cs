using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using B83.MathHelpers;

public class AudioScript : MonoBehaviour {
    public GameObject[] Cameras;
    public int CurrentCamera = 0;
    public Material wall;

    public LightScript light1, light2, light3;

    const int SampleRate = 48000;
    const int BufferSize = 1024;

    int AudioDevice = 0;

    float InputGain = 2;

    public Camera c;
	AudioClip p, previous;

    public GameObject prefab;

    private List<GameObject> prefabsList = new List<GameObject>();

    public int CameraCountdown = 300;
    float h = 0f;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        foreach (string t in Microphone.devices)
            Debug.Log(t);
        Physics.gravity.Set(0, -9.8f, 0);
        StartRecording();
    }

    void StartRecording()
    {
        Debug.Log("Connected To: " + Microphone.devices[AudioDevice]);
        p = Microphone.Start(Microphone.devices[AudioDevice], false, 1, SampleRate);
    }
    // Update is called once per frame

    int framescape = 0;
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            InputGain += 0.2f;
            print(InputGain);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            InputGain -= 0.2f;
            print(InputGain);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AudioDevice++;
            StartRecording();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioDevice--;
            StartRecording();
        }
    }

    private void FixedUpdate()
    {

        //ChangeColor();
        if (framescape++ > 30)
        {
            p.GetData(whole, 0);
            ShowData();
        }
       if (CameraCountdown-- < 0)
        {
            CameraCountdown = 30;
            //ChangeCamera();
            //ChangeColor();
        }
        if (!Microphone.IsRecording(Microphone.devices[AudioDevice]))
        {
            p.GetData(whole, 0);
            p.UnloadAudioData();
            p = null;
            idx = 0;
            p = Microphone.Start(Microphone.devices[AudioDevice], false, 1, SampleRate);
        }
    }

    float[] whole = new float[SampleRate];
	int idx = 0;	
	Vector3 oldRotate = new Vector3(0,0,0);
    float freqStep = SampleRate / BufferSize;
    float max = 0;
    float rms = 0;
    float Amplitude = 0;
    int AmplitudeCounter = 30;

	void ShowData () {
		float[] data = new float[BufferSize];
		if (idx >= SampleRate - BufferSize || idx >= Microphone.GetPosition(Microphone.devices[AudioDevice]) - BufferSize)
			return;
		System.Array.Copy (whole, idx, data, 0, BufferSize);
		idx += BufferSize;
        Complex[] cx = FFT.Float2Complex(data);
		FFT.CalculateFFT (cx, false);
        float linestart = -3.5f;
        float linestep = 0.1f;
        float max = 0;

        float bassEnergy = 0f, midEnergy = 0f, highEnergy = 0f;
        int counter = 0, i = 0;
        for (; i < cx.Length / 100; i++ , counter++) //0-200hz
            bassEnergy += Mathf.Sqrt(cx[counter].fMagnitude * 500f);
        bassEnergy = bassEnergy / counter;
        for (counter = 0; i < cx.Length / 10; i++, counter++) // 200-2000hz
            midEnergy += Mathf.Sqrt(cx[counter].fMagnitude * 1000f);
        midEnergy = midEnergy / counter;
        for (counter = 0; i < cx.Length; i++, counter++) // 2000 - 20000hz
            highEnergy += Mathf.Sqrt(cx[counter].fMagnitude * 5000f);
        highEnergy = highEnergy / counter;


        light1.ChangeColor(bassEnergy, 0, 200);
        light2.ChangeColor(midEnergy, 200, 2000);
        light3.ChangeColor(highEnergy, 2000, 20000);


        float fstep = 1f;
        float fstart = 0;
        for (i = 0; i < cx.Length; i=(int)fstart) {
            fstart += fstep;
            fstep *= 1.9f;
            var realvalue = cx[i].fMagnitude;
            var value = cx[i].fMagnitude*20f * InputGain;
			if (value > max)
				max = value;
            Amplitude = realvalue;
            rms = 0.999f * rms + 0.001f * realvalue;
            if (realvalue > max)
            {
                max = realvalue;
                AmplitudeCounter = 50;
            }
            else if (AmplitudeCounter-- <= 0)
            {
                max = realvalue;
                AmplitudeCounter = 50;
            }
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

    float hstep = 0.001f;

    public void ChangeColor()
    {
        float amplitudeMagnitude = Mathf.Sqrt(rms * 20f * InputGain) / 50f;
        //print(amplitudeMagnitude);
        //float r = Random.Range(0f, 1f) * amplitudeMagnitude;
        //float g = Random.Range(0f, 1f) * amplitudeMagnitude;
        //float b = Random.Range(0f, 1f)* amplitudeMagnitude;
        //print(r);
        //print(g);
        //print(b);
        //Color.HSVToRGB(0, 1, amplitudeMagnitude);
        //Color toChange = new Color(r, g, b);
        wall.color = Color.HSVToRGB(rms ,amplitudeMagnitude, h);
        if (h > 1f || h < 0f)
            hstep *= -1;
        h += hstep;
    }
}
