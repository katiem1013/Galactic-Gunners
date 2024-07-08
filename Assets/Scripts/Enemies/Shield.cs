using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldHealth <= 0f)
            Invoke("Die", 0.1f);
    }

    public void TakeDamage(float amount)
    {
        shieldHealth -= amount;
    }

    IEnumerator Die()
    {
        Destroy(gameObject);
        return null;
    }

}
