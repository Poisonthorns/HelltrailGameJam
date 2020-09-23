﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
	public LayerMask enemyLayer;
	public Transform attackPoint;

	//Access variables like this: weapon.GetComponent<BaseWeapon>().attackDamage
	private GameObject weapon;
	int weaponIndex = 0;
	public GameObject[] allWeapons;

	private float timeSinceLastAttack;
	private AudioSource playerAudio;
	private WeaponWheel weaponSlots;

	[SerializeField]
	private AudioClip weaponSwap;


	// Start is called before the first frame update
	void Start()
	{
		weapon = allWeapons[weaponIndex];
		playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
		weaponSlots = GameObject.Find("Weapon Wheel").GetComponent<WeaponWheel>();
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
	}

	private void GetInput()
	{
		//Swap Weapon Input
		if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift))
		{
			weaponIndex += 1;
			if (weaponIndex >= allWeapons.Length)
				weaponIndex = 0;
			weapon = allWeapons[weaponIndex];
			playerAudio.PlayOneShot(weaponSwap);

			// This is hardcoded so I will fix this: swaps weapon icon on HUD
			if (weapon.GetComponent<BaseWeapon>().attackDamage == 20)
			{
				weaponSlots.selectNext(0, 1);// heavy
			} else if (weapon.GetComponent<BaseWeapon>().attackDamage == 10)
            {
				weaponSlots.selectNext(1, 0);// light
			}
				Debug.Log("Swap weapon");
		}

		//Attack Input
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			attackPoint.localPosition = new Vector3(0.0f, 1.5f, 0f);
			Debug.Log(weapon.GetComponent<BaseWeapon>().attackDamage);
			Attack();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			attackPoint.localPosition = new Vector3(0.0f, -1.5f, 0f);
			Attack();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			attackPoint.localPosition = new Vector3(-1.0f, 0.5f, 0f);
			Attack();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			attackPoint.localPosition = new Vector3(1.0f, 0.5f, 0f);
			Attack();
		}
	}

	void Attack()
	{
		playerAudio.PlayOneShot(weapon.GetComponent<BaseWeapon>().attackSound);
		if (Time.time > timeSinceLastAttack)
		{
			Debug.Log("PlayerCombatController Attack");
			Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint.position, weapon.GetComponent<BaseWeapon>().attackBox, 0f, enemyLayer, -100f, 100f);
			foreach (Collider2D enemy in enemies)
			{
				UnityEngine.Debug.Log("We hit " + enemy.name);
				if(enemy.gameObject.tag == "Enemy")
					enemy.GetComponent<EnemyHealthManager>().LoseHealth(weapon.GetComponent<BaseWeapon>().attackDamage);
			}
		
			timeSinceLastAttack = Time.time + weapon.GetComponent<BaseWeapon>().attackRate;
		}
	}
}