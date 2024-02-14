using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    [SerializeField] SpriteRenderer rightWallSR, leftWallSR;
    [Header("Doors")]
    [SerializeField] DoorGroup uprightDoor;
    [SerializeField] DoorGroup upleftDoor;
    [SerializeField] DoorGroup downrightDoor;
    [SerializeField] DoorGroup downleftDoor;

    [Header("Shadow Casters")]
    [SerializeField] ShadowCaster2D roomTopCorner;
    [SerializeField] ShadowCaster2D roomLeftCorner, roomRightCorner, roomBottomCorner;

    DoorGroup[] doors;

    bool hasPlayer;
    bool beingPeek;

    // Start is called before the first frame update
    void Start()
    {
        doors = new DoorGroup[4] { uprightDoor , upleftDoor , downrightDoor , downleftDoor };
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPeeked();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCheck(collision);
    }

    void PlayerCheck(Collider2D collision)
    {
        collision.gameObject.CompareTag("Player");
    }

    void CheckIfPeeked()
    {
        beingPeek = false;

        foreach (DoorGroup door in doors)
        {
            if (door)
            {
                beingPeek = door.GetIsBeingPeek();
                if (beingPeek)
                {
                    break;
                }
            }
        }
    }

    void WallRule()
    {

    }
}
