using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyCons : MonoBehaviour
{
    // joycon variables
    private List<Joycon> joycons;
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;
    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Joycon j = joycons[jc_ind];
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            stick = j.GetStick();

            // finds the rotation of the joycons
            orientation = j.GetVector();
            orientation = new Quaternion(orientation.x, orientation.z, orientation.y, orientation.w);
            gameObject.transform.localRotation = Quaternion.Inverse(orientation);
            gameObject.transform.rotation = orientation;
        }
    }
}
