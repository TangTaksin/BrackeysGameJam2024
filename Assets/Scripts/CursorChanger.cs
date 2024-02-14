using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D customCursor; // Drag your custom cursor texture to this field in the Unity editor.

    void Start()
    {
        SetCustomCursor();
    }

    public void SetCustomCursor()
    {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseEnter()
    {
        SetCustomCursor();
    }

    public void OnMouseExit()
    {
        ResetCursor();
    }


}
