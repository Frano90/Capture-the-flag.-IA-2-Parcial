using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    //Singleton
    public static Main instance;
    
    
    //Managers
    public GameController gameCotroller; 
    
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    
}
