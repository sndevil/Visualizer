using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public List<Camera> Cameras;

    private float currentField = 180f, targetField = 180f, startingField = 180f;
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
        targetField = startingField / target;
        if (targetField > 200) targetField = 200;
       //print(target.ToString() + " ; " + targetField.ToString());
    }
}
