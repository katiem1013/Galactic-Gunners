using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameManager gameManager;
   
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindInActiveObjectByTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.levelIsLeaveable = true;
            gameObject.SetActive(false);
        }
    }

    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
