using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGroup : MonoBehaviour
{
    [SerializeField] door[] doors;
    [SerializeField] bool canPeekManyDoor;

    bool isBeingPeek;
    
    private void Start()
    {
        //add listener to each doors
        foreach (var door in doors)
        {
            door.OnPeeked += OnPeek;
            door.OnMoveaway += OnMoveaway;
        }
    }

    public bool GetIsBeingPeek()
    {
        return isBeingPeek;
    }

    public void SetVisibility(bool value)
    {
        foreach(var door in doors)
        {
            door.SetVisibility(value);
        }
    }

    void OnPeek()
    {
        if (!canPeekManyDoor)
        {
            foreach (var door in doors)
            {
                door.SetIsPeeked(true);
            }
        }

        isBeingPeek = true;
    }

    void OnMoveaway()
    {
        isBeingPeek = false;
    }
}
