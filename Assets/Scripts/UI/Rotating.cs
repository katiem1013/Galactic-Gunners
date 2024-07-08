using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    public Vector3 spinRotation;
    public float time;

    private void Update()
    {
        transform.Rotate(spinRotation * Time.deltaTime);

        time += Time.deltaTime;

        if (time > 4)
        {
            time = 0;
            spinRotation += new Vector3(1, 2, 1);
        }
    }
}
