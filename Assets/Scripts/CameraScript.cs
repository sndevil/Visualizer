using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public List<Camera> Cameras;

    private float currentField = 72f, targetField = 72f, startingField = 72f;
    public float speed = 0.2f;

    public void FixedUpdate()
    {
        foreach (Camera c in Cameras)
            c.fieldOfView = currentField;

        if (currentField < targetField)
            currentField += speed;
        else if (currentField > targetField)
            currentField -= speed;

    }

    public void SetField(float target)
    {
        targetField = startingField / (target * target);
    }
}
