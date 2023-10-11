using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFireBall : MonoBehaviour
{
    public GameObject fb, fb_clone;
    public float PushForce = 100.0f;
    public bool Busy = false;
    public float WaitTime = 1.0f;

    private Transform Player, WeaponCam;

    // Start is called before the first frame update
    void Start()
    {
        fb = GameObject.Find("Fireball");

        Player = GameObject.Find("Player").transform;

        WeaponCam = GameObject.Find("WeaponCam").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //fb_clone = Instantiate(fb, GameObject.Find("FBSpawnPoint").transform.position, GameObject.Find("FBSpawnPoint").transform.rotation) as GameObject;

        if (!Busy)
        {
            StartCoroutine("Eject");
        }
    }

    IEnumerator Eject()
    {
        Busy = true;

        fb_clone = Instantiate(fb, GameObject.Find("FBSpawnPoint").transform.position, GameObject.Find("FBSpawnPoint").transform.rotation) as GameObject;
        
        fb_clone.transform.LookAt(WeaponCam.position);
        //fb_clone.transform.LookAt(Player.position);

        //fb_clone.GetComponent<Rigidbody>().AddForce(0, 0, -PushForce);
        fb_clone.GetComponent<Rigidbody>().AddForce(fb_clone.transform.forward * PushForce);

        yield return new WaitForSeconds(WaitTime);

        Busy = false;
    }


}
