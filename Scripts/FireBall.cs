using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float lifeTimer;
    private float lifeDuration = 1.0f; 

    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {

        lifeTimer -= Time.deltaTime;

        if  (lifeTimer <= 0)
        {

            //Destroy(this.gameObject);

        }
 
    }


}
