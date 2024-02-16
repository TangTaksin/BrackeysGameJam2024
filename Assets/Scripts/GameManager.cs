using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void GameEvent();
    public static GameEvent OnPauseEvent;
    public static GameEvent OnResumeEvent;

    private void Start()
    {
        
    }

    public static void RequestPause()
    {
        OnPauseEvent?.Invoke();
    }

    public static void RequestResume()
    {
        OnResumeEvent?.Invoke();
    }
}
