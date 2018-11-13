﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBehavior : MonoBehaviour {

	private Rigidbody2D bot;
	private GameObject player;
	private float hitPoints = 100.0f;
	
	public float speed = 4.5f;

	void Start () {
		
		bot = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update(){

		if(hitPoints == 0.0f){
			Destroy(gameObject);
		}
	}

	void FixedUpdate () {
		if(player != null)
			moveBot();
	}
 
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Bullet"){
			hitPoints -= 50.0f;
		}
	}

	void moveBot(){

		Vector2 playerDir = player.transform.position - bot.transform.position;
		float rotateBy = Vector2.SignedAngle(Vector2.up, playerDir);
		bot.transform.eulerAngles = new Vector3(0, 0, rotateBy);

		Vector2 direction = getAStarDir();
		bot.MovePosition(bot.position + direction.normalized * Time.fixedDeltaTime*speed);
	}

	Vector2 getAStarDir(){
		
		Vector2 playerDir = player.transform.position - bot.transform.position;
		Vector2 avoidanceDir = new Vector2(0, 0);
		Collider2D[] colliders = Physics2D.OverlapCircleAll(bot.transform.position, playerDir.magnitude);
		float xSum = 0.0f;
		float ySum = 0.0f;
		float wA = -1.0f;
		float wP = 2.0f;
		for(int i=0; i < colliders.Length; i++){
			Rigidbody2D thing = colliders[i].attachedRigidbody;
			if(thing != null && thing.tag != "Player" && thing.tag != "Bullet"){
				//xSum += thing.transform.position.x - bot.transform.position.x;
				//ySum += thing.transform.position.y - bot.transform.position.y;
				avoidanceDir += thing.position - bot.position;
			}
		}
		/*
		if(playerDir.magnitude <= 1.0f){
			wA = 0.0f;
			wP = 2.0f;
		}
		*/
		Vector2 direction = (wA*avoidanceDir.normalized + wP*playerDir.normalized);
		return direction;
	}
}