using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public Queries query;

    public void StartRaycast(bool cooldown)
    {
        query.RayFromCamera(cooldown);
    }
}
