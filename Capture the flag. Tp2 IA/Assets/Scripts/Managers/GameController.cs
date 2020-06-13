using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Flag flag;
    public bool IsGameOn { get; private set; }
    public event Action OnStartSimulation = delegate {  };

    private void Start()
    {
        OnStartSimulation += ResetWorld;
    }
    
    void ResetWorld()
    {
        Debug.Log("SALIMOOOO");
        IsGameOn = true;
    }
    
    public void StartSimulation() => OnStartSimulation?.Invoke();
}
