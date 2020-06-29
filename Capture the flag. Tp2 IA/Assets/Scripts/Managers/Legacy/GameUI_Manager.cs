using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Manager : MonoBehaviour
{
    [SerializeField] private Button startButton;

 
        
    public Action<Tuple<int, int>> RefreshScore = delegate {  };
        


    [SerializeField] private Text redScore;
    [SerializeField] private Text blueScore;


    
    void Start()
    {
        startButton.onClick.AddListener(StartSimulation); 
        
        RefreshScore = (score) =>
        {
        redScore.text = $"Red Score: {score.Item1}";
        blueScore.text = $"Blue Score: {score.Item2}";

        }; 
    }

    void StartSimulation() => Main.instance.gameCotroller.StartSimulation();
}
