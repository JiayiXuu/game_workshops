﻿using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountTxet();
        winText.text = "";
    }

    // FixedUpdate is called before performing any physics calculation
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountTxet();
        }
    }

    void SetCountTxet ()
    {
        countText.text = "Count: " + count.ToString();
        if  (count >= 12)
        {
            winText.text = "You Win!";
        }
    }
}
