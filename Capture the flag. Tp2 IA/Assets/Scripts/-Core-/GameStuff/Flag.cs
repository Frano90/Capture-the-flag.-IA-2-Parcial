using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Vector3 initPos;
    private void Start()
    {
        initPos = transform.position;
    }

    public void ResetPos()
    {
        transform.position = initPos;
    }
}
