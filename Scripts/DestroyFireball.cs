using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class DestroyFireball : MonoBehaviour {

	public float damage=2.0f;
	public AudioClip hitSound;
	AudioSource AS;

	void Start(){

		AS = GetComponent<AudioSource> ();

	}

	void OnTriggerStay (Collider col) {
	
		Debug.Log ("Inside Collision!");

		if (col.CompareTag("Player")) {

			Destroy(this.gameObject);
			
			//col.gameObject.SendMessage("Hurt", damage);

		} else if (col.CompareTag("Wall")) { 
			
			Debug.Log ("Hit Wall !!!");

			AS.PlayOneShot (hitSound);

			Destroy(this.gameObject);

		} else {
			
			Debug.Log ("Fireball NOT destroyed!!!");

		}

	}
}
