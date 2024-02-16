using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    [SerializeField] SpriteRenderer rightWallSR, leftWallSR, roomSR;
    [Header("Shadow Casters")]
    [SerializeField] ShadowCaster2D roomTopCorner;
    [SerializeField] ShadowCaster2D roomLeftCorner, roomRightCorner, roomBottomCorner;

    DoorGroup[] doors;

    bool hasPlayer;
    bool beingPeek;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCheck(collision);
    }

    void PlayerCheck(Collider2D collision)
    {
        collision.gameObject.CompareTag("Player");
    }

    void SetRoomVisibility(bool value)
    {
        if (value)
        {
            roomSR.color = Color.white;
            rightWallSR.color = Color.white;
            leftWallSR.color = Color.white;
        }
        else
        {
            roomSR.color = Color.clear;
            rightWallSR.color = Color.clear;
            leftWallSR.color = Color.clear;
        }
    }
}
