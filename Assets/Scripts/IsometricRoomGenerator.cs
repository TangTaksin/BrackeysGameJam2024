using UnityEngine;
using System.Collections.Generic;

public class IsometricRoomGenerator : MonoBehaviour
{
    public GameObject floorTilePrefab;
    public GameObject wallTilePrefab;
    public int roomWidth = 5;
    public int roomHeight = 5;

    void Start()
    {
        GenerateRoom();
    }

    void GenerateRoom()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                Vector3 tilePosition = new Vector3(x - y, (x + y) / 2f, 0);
                GameObject tilePrefab = (x == 0 || x == roomWidth - 1 || y == 0 || y == roomHeight - 1) ? wallTilePrefab : floorTilePrefab;
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.parent = transform;
            }
        }
    }
}
