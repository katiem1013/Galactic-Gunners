using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
