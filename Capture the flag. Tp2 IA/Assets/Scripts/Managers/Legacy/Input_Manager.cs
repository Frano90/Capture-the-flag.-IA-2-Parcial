using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    Event_Manager _manager;
    bool _readyToAttack;

    void Start()
    {
        _readyToAttack = true;
        _manager = FindObjectOfType<Event_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _readyToAttack)
        {
            _readyToAttack = false;
            _manager.StartRaycast(false);
            StartCoroutine(MyTimer());
        }
    }

    IEnumerator MyTimer()
    {
        yield return new WaitForSeconds(1);
        _readyToAttack = true;
    }
}
