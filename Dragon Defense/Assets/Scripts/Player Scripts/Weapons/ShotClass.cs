﻿using UnityEngine;
using System.Collections;

public class ShotClass : MonoBehaviour {

	public string shotName; //Name of the weapon
	public float damage; //Damage the weapon deals
	public float timeAlive;
	public bool canRoll; //Can it roll on the ground? (not sure what this will be fure quite yet)
	public bool canCollideWithShots; //Can it collide with other shots?
	public bool lobShot; //Will it lob, or be a straight shot?
	public int amountToFire = 1; //Amount of shots to fire (increased with upgrade system etc)
	public Vector2 velocity;
	public int weaponColorNumber;

	protected bool paused;
	protected bool wasPaused;

	public ParticleSystem trailParticles;
	public ParticleSystem burstParticles;

	public delegate void DamageEvent(float damage, GameObject col, int weaponNumber, Vector3 shotPosition);
	public static event DamageEvent OnDamage;


	#region Event Subscriptions
	void OnEnable(){
		GameStateManager.OnPause += OnPause;
	}
	void OnDisable(){
		GameStateManager.OnPause -= OnPause;
	}
	void OnDestroy(){
		GameStateManager.OnPause -= OnPause;
	}
	#endregion

	void OnTriggerEnter2D(Collider2D col){
		burstParticles.Play();
		if(!canRoll)
			GetComponent<Collider2D>().enabled = false;
		GameObject coll = col.gameObject;

        if (OnDamage != null) {
			OnDamage(damage, coll, weaponColorNumber, transform.position);
        }

        if(GetComponentInChildren<IceBolt>() != null && coll.GetComponentInParent<EnemyClass>() != null) //this is shit
        {
            coll.GetComponentInParent<EnemyClass>().moveSpeed /= 2;
        }
		MoveShot();
	}

	public void MoveShot(){
		GetComponent<SpriteRenderer>().enabled = false;
		trailParticles.Stop();
		timeAlive = timeAlive/2;
	}

	public void OnPause(){
		paused = !paused;
		wasPaused = true;
	}

	protected void CheckTime(){
		if(timeAlive > 0){
			timeAlive -= Time.deltaTime;
		} else if(timeAlive <= 0){ //If the fired shot is alive for longer than timeAlive, we destroy it
			Destroy(gameObject);
		}
	}
}