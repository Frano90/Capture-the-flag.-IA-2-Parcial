using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private StateMachine _sm;

    private void Start()
    {
        _sm = new StateMachine();
        
        
    }
}
