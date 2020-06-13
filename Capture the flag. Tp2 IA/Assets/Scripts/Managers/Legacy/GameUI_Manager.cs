using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Manager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    
    void Start()
    {
        startButton.onClick.AddListener(StartSimulation);
    }

    void StartSimulation() => Main.instance.gameCotroller.StartSimulation();
}
