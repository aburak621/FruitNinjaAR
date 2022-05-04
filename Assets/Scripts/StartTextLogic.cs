using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTextLogic : MonoBehaviour
{
    private RectTransform Text;
    private RectTransform Text2;
    
    // Start is called before the first frame update
    void Start()
    {
        Text = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        Text2 = transform.GetChild(1).gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        
        Vector3 textRotation = new Vector3(0f, 0f, -30 * Time.deltaTime);
        Text.eulerAngles += textRotation;
        Text2.eulerAngles += textRotation;
    }
}
