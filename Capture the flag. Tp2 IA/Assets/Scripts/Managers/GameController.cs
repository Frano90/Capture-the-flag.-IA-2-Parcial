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

    private int redScore;
    private int blueScore;
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

    public void OnFlagToBase(Entity ent)
    {
        if (ent._teamSide == Enums.TeamSide.Blue)
        {
            blueScore++;
            
        }
        else
        {
            redScore++;
        }
        
        foreach (Entity e in entidades)
        {
            e.ResetEntitySM();
        }

        Main.instance.uiManager.RefreshScore(Tuple.Create(redScore, blueScore));
    }
    
    public void StartSimulation()
    {
        ResetScore();
        OnStartSimulation?.Invoke();
    }
    
    void ResetScore ()
    {
        blueScore = 0;
        redScore = 0;
        
        Main.instance.uiManager.RefreshScore(Tuple.Create(redScore, blueScore));
    }
}
