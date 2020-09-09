﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Transform attackPoint;
    public Animator anim;

    int currentAttack;
    int attackDamage;
    Vector2 attackBox;

    Vector2 lightAttackBox = new Vector2(0.5f, 0.5f);
    Vector2 heavyAttackBox = new Vector2(1f, 2f);

    [SerializeField]
    int lightAttackDamage = 10;
    [SerializeField]
    int heavyAttackDamage = 20;

    [SerializeField]
    float lightAttackRange = 0.2f;
    [SerializeField]
    float heavyAttackRange = 1.5f;

    [SerializeField]
    LayerMask enemyLayer;
    

    void Start()
	{
        attackDamage = lightAttackDamage;
        currentAttack = 0;
        attackBox = lightAttackBox;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
	{
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwapWeapon();
        }
    }

    void Attack()
	{
        Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint.position, attackBox, 0f, enemyLayer, -100f, 100f);
        if (attackDamage == heavyAttackDamage)
        {
            anim.Play("Heavy_Attack");
        }
        else 
        {
            anim.Play("Light_Attack");
        }
        foreach (Collider2D enemy in enemies)
		{
            UnityEngine.Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
		}
    }

    void SwapWeapon()
	{
        switch(currentAttack)
		{
            case 0:
                currentAttack = 1;
                attackDamage = heavyAttackDamage;
                attackBox = heavyAttackBox;
                //Weapon wheel animation here - switch from light attack to heavy
                UnityEngine.Debug.Log("Switched to the heavy weapon");
                break;
            case 1:
                currentAttack = 0;
                attackDamage = lightAttackDamage;
                attackBox = lightAttackBox;
                //Weapon wheel animation here - switch from heavy attack to light
                UnityEngine.Debug.Log("Switched to the light weapon");
                break;
            default:
                UnityEngine.Debug.Log("Something went wrong in SwapWeapon");
                break;
		}
	}
}
