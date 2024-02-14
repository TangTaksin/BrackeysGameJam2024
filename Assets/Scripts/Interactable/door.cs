using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : Interactable
{
    bool isPeeked;

    public delegate void doorEvent();
    public static doorEvent OnPeeked;
    public static doorEvent OnMoveaway;

    public void SetIsPeeked(bool val)
    {
        isPeeked = val;
    }

    public override void Interact()
    {
        // if it hasn't been peeked yet.
        if (!isPeeked)
        {
            // start door peeking animation
            OnPeeked?.Invoke();
            isPeeked = true;
        }

        //show enter or not prompt
        base.Interact();
    }

    // Enter next room if yes
    protected override void OnYes()
    {

    }

    // don't, if no
    protected override void OnNo()
    {

    }
}
