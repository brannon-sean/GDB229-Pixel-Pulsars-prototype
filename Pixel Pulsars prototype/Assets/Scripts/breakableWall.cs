using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableWall : MonoBehaviour, IDamage
{
    [SerializeField] int healthPoints;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject brokenWall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        healthPoints -= amount;
        if(healthPoints <= 0) {
            wall.SetActive(false);
            brokenWall.SetActive(true);
            StartCoroutine(clearBrokenWall());
        }
    }

    IEnumerator clearBrokenWall()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
