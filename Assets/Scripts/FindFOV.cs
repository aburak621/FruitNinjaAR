using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindFOV : MonoBehaviour
{
    private Text textField = null;
    
    // Start is called before the first frame update
    void Start()
    {
        textField = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textField.text = Camera.main.fieldOfView.ToString();
    }
}
