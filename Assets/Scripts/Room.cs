using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    [SerializeField] SpriteRenderer rightWallSR, leftWallSR, roomSR;
    [SerializeField] bool hideOnAwake;

    [SerializeField] DoorGroup doors;

    bool beingPeek;

    // Start is called before the first frame update
    void Start()
    {
        doors.onPeekEvent += OnPeek;

        if (hideOnAwake)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnPeek(bool value)
    {
        beingPeek = value;
        gameObject.SetActive(beingPeek);
    }
}
