using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gun : ScriptableObject
{
    [Header("---- Gun Stats ----")]
    public string gunName;
    public float shootRate;
    public int shootDamage;
    public int shootDistance;
    public int ammoCurrent;
    public int ammoMax;

    [Header("---- Visual/Audio ----")]
    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shootSound;
}
