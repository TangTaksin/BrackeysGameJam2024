using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects", order = 1)]
public class PlayerInvSO : ScriptableObject
{
    public int bullet;
    public int magazine;
}
