using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class character : ScriptableObject
{
    [Header("---- Character Stats ----")]
    public string characterName;
    public int healthPoints;
    public int playerSpeed;
    public int jumpsMax;

    [Header("---- Gun Assignment ----")]
    public gun gunPrefab;
}
