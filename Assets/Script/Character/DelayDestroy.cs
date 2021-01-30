﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float DelayTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DelayTime -= Time.deltaTime;

        if(DelayTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
