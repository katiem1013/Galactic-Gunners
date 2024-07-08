using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    public float attackSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // shoots the bullet out and destroys after 1 second
        transform.Translate(Vector3.forward * Time.deltaTime * attackSpeed);
        Destroy(gameObject, 1);
    }
}
