using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using B83.MathHelpers;

public class AudioScript : MonoBehaviour {
    public Material wall;

    public LightScript light1, light2, light3;
    public List<Light> SceneLights;
    public CameraScript cameraScript;

    const int SampleRate = 48000;
    const int BufferSize = 1024;

    int AudioDevice = 0;

    float InputGain = 2;

    //int FrameScape = 2;

	AudioClip p;

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
        p = Microphone.Start(Microphone.devices[AudioDevice], true, 1, SampleRate);
    }
    // Update is called once per frame

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
        p.GetData(whole, 0);
        ShowData();
        ChangeColor();
    }

    float[] whole = new float[SampleRate];
	int idx = 0;	
	Vector3 oldRotate = new Vector3(0,0,0);
    float rms = 0;

	void ShowData () {
		float[] data = new float[BufferSize];
        if (idx >= SampleRate - BufferSize || idx >= Microphone.GetPosition(Microphone.devices[AudioDevice]) - BufferSize)
        {
            idx = 0;
            return;
        }
		System.Array.Copy (whole, idx, data, 0, BufferSize);
		idx += BufferSize;
        Complex[] cx = FFT.Float2Complex(data);
		FFT.CalculateFFT (cx, false);

        float bassEnergy = 0f, midEnergy = 0f, highEnergy = 0f;
        int counter = 0, i = 0;
        for (; i < cx.Length / 100; i++, counter++) //0-200hz
        {
            bassEnergy += Mathf.Sqrt(cx[counter].fMagnitude * InputGain * 500f);
            rms = 0.999f * rms + 0.001f * cx[counter].fMagnitude;
        }
        bassEnergy = bassEnergy / counter;
        for (counter = 0; i < cx.Length / 10; i++, counter++) // 200-2000hz
        {
            midEnergy += Mathf.Sqrt(cx[counter].fMagnitude * InputGain * 5000f);
            rms = 0.999f * rms + 0.001f * cx[counter].fMagnitude;
        }
        midEnergy = midEnergy / counter;
        for (counter = 0; i < cx.Length; i++, counter++) // 2000 - 20000hz
        {
            highEnergy += Mathf.Sqrt(cx[counter].fMagnitude * InputGain * 20000f);
            rms = 0.999f * rms + 0.001f * cx[counter].fMagnitude;
        }
        highEnergy = highEnergy / counter;


        light1.ChangeColor(bassEnergy, 0, 200);
        light2.ChangeColor(midEnergy, 200, 2000);
        light3.ChangeColor(highEnergy, 2000, 20000);
	}

    public void ChangeColor()
    {
        float amplitudeMagnitude = Mathf.Sqrt(rms * 20f * InputGain)*20f;
        cameraScript.SetField(amplitudeMagnitude);
        foreach (Light l in SceneLights)
            l.intensity = amplitudeMagnitude;
    }
}
