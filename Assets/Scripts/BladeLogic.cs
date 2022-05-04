using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeLogic : MonoBehaviour
{
    [SerializeField] private GameObject trail = null;

    public Vector3 velocity = Vector3.zero;
    private Vector3 previousLocation = Vector3.zero;
    private Rigidbody rb = null;
    private Transform speedPoint = null;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedPoint = gameObject.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.033f)
        {
            Vector3 currentLocation = speedPoint.position;
            velocity = (currentLocation - previousLocation) * Time.deltaTime;
            previousLocation = currentLocation;
            timer -= 0.033f;
        }
    }
}