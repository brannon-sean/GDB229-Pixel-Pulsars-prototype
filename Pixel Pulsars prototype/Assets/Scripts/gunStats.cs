using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    // Gun Stats
    public float shootRate;
    public int shootDamage;
    public int shootDistance;
    public int currentAmmo;
    public int maxAmmo;

    // Gun Model 
    public GameObject gunModel;
    public ParticleSystem hitEff;
    public AudioClip shotSound;


}