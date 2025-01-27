﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class Portal : MonoBehaviour
{
    public float z_tran;
    public Material[] materials;
    // Start is called before the first frame update

    void Start()
    {
        foreach (var mat in materials)
        {
            mat.SetInt("stest", (int)CompareFunction.Equal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag != "MainCamera")
        {
            return;
        }

        //outside
        if (transform.position.z >= (collider.transform.position.z + z_tran))
        {
            foreach (var mat in materials)
            {
                mat.SetInt("stest", (int)CompareFunction.Equal);
            }
        }else
        {
            foreach (var mat in materials)
            {
                mat.SetInt("stest", (int)CompareFunction.NotEqual);
            }
        }
    }
}
