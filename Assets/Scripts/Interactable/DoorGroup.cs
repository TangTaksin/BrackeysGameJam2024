using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGroup : MonoBehaviour
{
    [SerializeField] door[] doors;
    [SerializeField] bool canPeekManyDoor;

    bool isBeingPeek;

    public delegate void DoorGroupDelegate(bool value);
    public DoorGroupDelegate onPeekEvent;
    
    private void Start()
    {
        //add listener to each doors
        foreach (var d in doors)
        {
            d.OnPeeked += OnPeek;
            d.OnMoveaway += OnMoveaway;
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


    void OnPeek(door doorData)
    {
        if (!canPeekManyDoor)
        {
            foreach (var door in doors)
            {
                if (door != doorData)
                    door.SetIsLocked(true);
            }
        }

        isBeingPeek = true;
        onPeekEvent?.Invoke(isBeingPeek);
    }

    void OnMoveaway(door doorData)
    {
        isBeingPeek = false;
        onPeekEvent?.Invoke(isBeingPeek);
    }
}
