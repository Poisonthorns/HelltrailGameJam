﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
	public LayerMask enemyLayer;
	public Transform attackPoint;
    public float additionalDamage;
	//Access variables like this: weapon.GetComponent<BaseWeapon>().attackDamage
	private GameObject weapon;
	int weaponIndex = 0;
	public GameObject[] allWeapons;

	private float timeSinceLastAttack;
	private AudioSource playerAudio;
	private WeaponWheel weaponSlots;

	[SerializeField]
	private AudioClip weaponSwap;

	//booleans here to know what weapon is currently equipped for animations along with animator
	public bool lightAttack = true;
	public bool heavyAttack = false;
	public bool rangeAttack = false;
	public Animator anim;

	public GameObject upgradedStats;

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
			
			playerAudio.PlayOneShot(weaponSwap);

			if (weaponIndex >= allWeapons.Length)
			{
				weaponIndex = 0;
				weaponSlots.selectNext(allWeapons.Length - 1, 0);// light
											 //for light attack animation
				lightAttack = true;
				heavyAttack = false;
				rangeAttack = false;
				weapon = allWeapons[weaponIndex];
			}
			else
			{
				weapon = allWeapons[weaponIndex];
				// This is hardcoded so I will fix this: swaps weapon icon on HUD
				if (weapon.GetComponent<BaseWeapon>().attackDamage == 20)
				{
					weaponSlots.selectNext(0, 1);// heavy
												 //for heavy attack animation
					lightAttack = false;
					heavyAttack = true;
					rangeAttack = false;
				}
				else if (weapon.GetComponent<BaseWeapon>().attackDamage == 10)
				{
					weaponSlots.selectNext(1, 0);// light
												 //for light attack animation
					lightAttack = true;
					heavyAttack = false;
					rangeAttack = false;
				}
				else if (weapon.GetComponent<BaseWeapon>().attackDamage == 5)
				{
					weaponSlots.selectNext(1, 2);// range
												 //for range attack animation
					lightAttack = false;
					heavyAttack = false;
					rangeAttack = true;
				}
			}
			Debug.Log("Swap weapon");
		}
		if(weapon.GetComponent<BaseWeapon>().rangedAttack)
		{

		}
		else
		{
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
	}

	void Attack()
	{
		if (Time.time > timeSinceLastAttack)
		{
			
			Debug.Log("PlayerCombatController Attack");
			//If statements for attack aniamations
			//Animation for light attack here
			if (lightAttack)
			{
				anim.Play("Light Attack");
			}

			//Animation for heavy attack
			else if (heavyAttack)
			{
				anim.Play("Heavy Attack");
			}
			Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint.position, weapon.GetComponent<BaseWeapon>().attackBox, 0f, enemyLayer, -100f, 100f);
			foreach (Collider2D enemy in enemies)
			{
				UnityEngine.Debug.Log("We hit " + enemy.name);
				if (enemy.gameObject != null)
				{
					if (enemy.gameObject.tag == "Enemy")
					{
						playerAudio.PlayOneShot(weapon.GetComponent<BaseWeapon>().attackSound);
						enemy.GetComponent<EnemyHealthManager>().LoseHealth(weapon.GetComponent<BaseWeapon>().attackDamage + additionalDamage + upgradedStats.GetComponent<Stats>().upgradedAttackRate);
					}
				}
			}
		
			timeSinceLastAttack = Time.time + weapon.GetComponent<BaseWeapon>().attackRate - upgradedStats.GetComponent<Stats>().upgradedAttackRate;
		}
	}
}
