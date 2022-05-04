using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitLogic : MonoBehaviour
{
    [SerializeField] private GameObject fruitTop = null;
    [SerializeField] private GameObject fruitBottom = null;
    [SerializeField] private GameObject juiceFX = null;
    [SerializeField] private Color juiceColor = Color.red;

    // private float timer = 0f;
    private GameObject gameWorld;
    private Rigidbody rb = null;
    private float fruitSpeed = 0.2f;
    private float rotationSpeed = 3.5f;
    private Gradient juiceGradient = new Gradient();
    private GradientAlphaKey alphaKey1 = new GradientAlphaKey(255f, 0.215f);
    private GradientAlphaKey alphaKey2 = new GradientAlphaKey(0f, 1f);
    private GameObject audioManagerObject;
    private AudioManager audioManager;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Cache variables
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameWorld = GameObject.Find("GameWorld");
        rb = gameObject.GetComponent<Rigidbody>();
        audioManagerObject = GameObject.Find("AudioManager");
        audioManager = audioManagerObject.GetComponent<AudioManager>();
        
        // Set random velocity and rotation
        Vector3 fruitForce = new Vector3(Random.Range(-fruitSpeed, fruitSpeed), Random.Range(2.5f, 3.3f) / 1.5f,
            Random.Range(-fruitSpeed, fruitSpeed));
        rb.velocity = fruitForce;
        rb.angularVelocity = fruitForce;
        rb.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * rotationSpeed);

        // Color gradient
        GradientColorKey colorKey1 = new GradientColorKey(juiceColor, 0f);
        GradientColorKey colorKey2 = new GradientColorKey(juiceColor, 0f);
        juiceGradient.SetKeys(new[] {colorKey1, colorKey2}, new[] {alphaKey1, alphaKey2});
        
        // Play throw sound
        audioManager.PlayFruitThrowSound();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy if below the map
        if (transform.position.y <= -1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Slice if conditions satisfy
        if (other.gameObject.CompareTag("Blade") &&
            other.gameObject.GetComponent<BladeLogic>().velocity.magnitude > 0.00003f)
        {
            GameObject otherObject = other.gameObject;
            Vector3 sliceDirection = otherObject.GetComponent<BladeLogic>().velocity;
            Vector3 cameraNormal = otherObject.transform.eulerAngles;
            DestroyFruit(sliceDirection, cameraNormal);
        }
    }

    private void DestroyFruit(Vector3 sliceDirection, Vector3 cameraNormal)
    {
        gameObject.SetActive(false);

        // Calculate velocity and rotation
        Vector3 lastVelocity = rb.velocity;
        Vector3 rotationFromSlice = Quaternion.FromToRotation(Vector3.right, sliceDirection).eulerAngles;
        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationFromSlice.x,
            Quaternion.FromToRotation(Vector3.back, Camera.main.transform.position).eulerAngles.y,
            rotationFromSlice.z));
        // Instantiate slices
        GameObject sliceFront =
            Instantiate(fruitTop, transform.position,
                finalRotation, gameWorld.transform);
        GameObject sliceBack = Instantiate(fruitBottom, transform.position,
            finalRotation,
            gameWorld.transform);
        Vector3 sliceSpeed = Vector3.ClampMagnitude(sliceDirection * 5000f, 1.2f);
        
        // Set velocity of sliced parts and destroy them
        Vector3 sliceVelocity = lastVelocity * 0.5f + sliceSpeed;
        sliceFront.GetComponent<Rigidbody>().velocity = sliceVelocity;
        sliceBack.GetComponent<Rigidbody>().velocity = sliceVelocity;
        
        Transform cameraTransform = Camera.main.gameObject.transform;
        Quaternion torqueDirection = Quaternion.Euler(sliceVelocity.x, sliceVelocity.y, sliceVelocity.z);
        torqueDirection *= Quaternion.Inverse(cameraTransform.rotation);
        int slicedTorqueDirection = 1;
        if (torqueDirection.eulerAngles.z < 0)
        {
            slicedTorqueDirection = -1;
        }
        
        sliceFront.GetComponent<Rigidbody>().AddTorque(-cameraNormal * slicedTorqueDirection * sliceDirection.magnitude * 300f);
        sliceBack.GetComponent<Rigidbody>().AddTorque(cameraNormal * slicedTorqueDirection * sliceDirection.magnitude * 300f);

        Destroy(sliceFront, 1.5f);
        Destroy(sliceBack, 1.5f);

        // Spawn visual fx
        GameObject currentJuice = Instantiate(juiceFX, transform.position, transform.rotation);
        ParticleSystem ps = currentJuice.GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.color = juiceGradient;
        Destroy(currentJuice, 0.75f);
        
        // Play sound
        audioManager.PlaySliceSound();
        
        // Increment Score
        gameManager.IncreaseScore();
        
        Destroy(gameObject);
    }
}