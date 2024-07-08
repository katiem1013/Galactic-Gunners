using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    public bool leaveDoor;
    static public bool objectiveCompleted;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindInActiveObjectByTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (leaveDoor && gameManager.levelIsLeaveable)
        {
            SceneManager.LoadScene("Scene1");

            if (!objectiveCompleted)
                GameManager.completedObjectives++; // adds to the amount of completed objectives

            objectiveCompleted = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        leaveDoor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        leaveDoor = false;
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
