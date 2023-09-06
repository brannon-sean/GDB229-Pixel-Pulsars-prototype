using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer Model;
    [SerializeField] int HP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(flashdamage());
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }

    IEnumerator flashdamage()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Model.material.color = Color.white;
    }
}
