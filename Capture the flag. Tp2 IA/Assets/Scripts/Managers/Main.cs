using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    //Singleton
    private static Main instance;
    
    
    //Managers
     
    
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    
}
