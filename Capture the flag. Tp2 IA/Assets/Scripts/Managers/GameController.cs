using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Flag flag;
    public List<Entity> entidades = new List<Entity>();
    public bool isGameOn = false;

    public Transform blueExitPos;
    public Transform redExitPos;
    public event Action OnStartSimulation = delegate {  };

    private void Start()
    {
        InitWorld();
        OnStartSimulation += ResetWorld;
    }

    void InitWorld()
    {
        entidades = FindObjectsOfType<Entity>().ToList();
    }


    void ResetWorld()
    {
        Debug.Log("SALIMOOOO");
        if(!isGameOn)
            isGameOn = !isGameOn;
        else
        {
            foreach (Entity e in entidades)
            {
                e.ResetEntity();
            }    
        }
        
    }
    
    public void StartSimulation() => OnStartSimulation?.Invoke();
}
