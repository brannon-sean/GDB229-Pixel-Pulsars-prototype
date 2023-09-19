using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gun : ScriptableObject
{
    // Gun Stats
    public float shootRate;
    public int shootDamage;
    public int shootDistance;
    public int currentAmmo;
    public int maxAmmo;

    // Gun Model 
    public GameObject model;
    public ParticleSystem hitEff;
    public AudioClip shotSound;


}
