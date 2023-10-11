using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform Player, WeaponCam;
    public float MoveSpeed = 2.0f;
    public float MaxDist = 20.0f;
    public float MinDist = 10.0f;
    public AudioClip zombie_roar;
    public AudioClip zombie_hurt;
    public AudioClip zombie_die;
    public AudioClip FireballFly;

    private float Dist;
    private float hitDist = 1000.0f;
    private Animator anim;
    private bool inSight = false;
    private bool roared = false;
    private AudioSource AS;
    private NavMeshAgent NM;
    public float health = 100.0f;
    private bool dead = false;

    public GameObject fb_clone;
    public Object fb;
    public float PushForce = 1000.0f;
    public bool Busy = false;
    public float WaitTime = 2.0f;

    public CharacterController charCtrl;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();
        NM = GetComponent<NavMeshAgent>();
        NM.enabled = false;
        health = 100.0f;

        //fb = GameObject.Find("FireballPrefab");
        Player = GameObject.Find("Player").transform;
        WeaponCam = GameObject.Find("WeaponCam").transform;

    }

    void Update()
    {

        Dist = Vector3.Distance(transform.position, Player.position); //calculate the distance between the player and the zombie

        Debug.Log("Player Dist : " + Dist);

        Ray eyeSight = new (transform.position, Player.position);

        if (Physics.Raycast(eyeSight, out RaycastHit hit, hitDist))        
        {
            if (hit.collider.CompareTag("Player"))   //Player seen by zombie
            {

                Debug.Log("insight is TRUE!!");
                inSight = true;
                
            } else if (hit.collider.CompareTag("Wall"))
            {

                Debug.Log("insight is FALSE!!");
                inSight = false;

            }
        }

        if (inSight)    //Player seen by zombie
        {


            if ((Dist > MinDist) && (Dist <= MaxDist))
            {                

                if (!roared)
                {
                    AS.PlayOneShot(zombie_roar);
                    roared = true;
                }

                if (!Busy)
                {

                    transform.LookAt(Player);
                    StartCoroutine(Eject());

                }

                //change from idle to walking
                NM.enabled = true;
                NM.SetDestination(Player.position);
                anim.SetTrigger("Walk");

            }

            if (Dist > MaxDist)
            {
                //change to idle mode
                anim.SetTrigger("Idle");
                roared = false;

                NM.enabled = false;     // Stop tracing the player
            }

            if (Dist <= MinDist)
            {
                //change to attack mode             
                NM.enabled = false;
                
                Vector3 temPos = Player.position;
                temPos.y = transform.position.y;
                transform.LookAt(temPos);                
                anim.SetTrigger("Attack");

            }
        }
        else  //Player not seen by zombie
        {
            //change to idle
            anim.SetTrigger("Idle");
            NM.enabled = false;
        }

    }

    IEnumerator Eject()
    {
        Busy = true;

        fb_clone = Instantiate(fb, GameObject.Find("FBSpawnPoint").transform.position, GameObject.Find("FBSpawnPoint").transform.rotation) as GameObject;

        fb_clone.transform.LookAt(WeaponCam.position);

        fb_clone.GetComponent<Rigidbody>().AddForce(fb_clone.transform.forward * PushForce);

        AS.PlayOneShot(FireballFly, 0.5f);

        yield return new WaitForSeconds(WaitTime);

        Busy = false;
    }



    internal void Damage(float dmg)
    {
        if (health > 0)
        {
            AS.PlayOneShot(zombie_hurt);
            anim.SetTrigger("Hurt");
            health -= dmg;

            transform.LookAt(Player);

            if (health <=0)
            {

                dead = true;
                AS.PlayOneShot(zombie_die);
                anim.SetTrigger("Die");
                NM.enabled = false;
                this.enabled = false;

                Destroy(fb);

            }

        }
        
        
    }


}