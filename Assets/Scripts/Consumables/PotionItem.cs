﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : MonoBehaviour
{
    public int id;
    Transform player;
    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        print("item detected");
        Debug.Log("Collided with " + col.gameObject.name);
        if (col.gameObject.name.Equals("Sprite"))
        {
            Debug.Log("made it past if");
            player = GameObject.Find("Player").transform;
            bool obtained = player.GetComponent<Inventory>().addItem(id);
            if(obtained)
            {
                Debug.Log("obtained true");
                Destroy(gameObject);
            }
        }
    }
}
