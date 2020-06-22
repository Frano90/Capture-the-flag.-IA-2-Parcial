using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevTools.Enums;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Flag flag;
    public List<Entity> entidades = new List<Entity>();
    public bool isGameOn = false;
    public Queries grid;

    [Header("Initial position settings")]
    public Transform blueExitPos;
    public Transform redExitPos;
    public Transform redBasePos;
    public Transform blueBasePos;

    public Entity flagHolder;
    public bool isFlagGrabbed = false;
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
                e.ResetEntityWithPos();
            }    
        }
        
    }

    public void OnFlagToBase()
    {
        foreach (Entity e in entidades)
        {
            e.ResetEntitySM();
        }  
    }
    
    public void StartSimulation() => OnStartSimulation?.Invoke();
}
