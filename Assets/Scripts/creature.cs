using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creature : MonoBehaviour
{
    //has health
    [SerializeField] int health_max;
    int health_cur;

    //has varied move speed
    [SerializeField] protected float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        health_cur = health_max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // health can be subtract (or heal).
    public void DamageHealth(int amount)
    {
        health_cur -= amount;

        // if health less than 0
        if (health_cur <= 0)
        {
            health_cur = 0;
            // ur ded lmao.
        }
        
        // clamp health at max.
        if (health_cur > health_max)
        {
            health_cur = health_max;
        }
    }

}
