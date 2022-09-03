using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwaner : MonoBehaviour
{
    public List<GameObject> objects;

    public void Spwan(string objectName)
    {
        foreach (var item in objects)
        {
            item.SetActive(objectName == item.name);
        }
    }
}
