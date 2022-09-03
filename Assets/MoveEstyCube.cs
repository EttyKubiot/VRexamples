using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEstyCube : MonoBehaviour
{
    public Transform movementSource;
    private void Update()
    {
        transform.localPosition =  movementSource.localPosition;
    }
}
