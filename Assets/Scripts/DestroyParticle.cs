﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Destroy after 2 seconds
        Destroy(gameObject, 4f);
    }
}
