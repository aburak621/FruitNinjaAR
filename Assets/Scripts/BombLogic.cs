using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLogic : MonoBehaviour
{
    private GameObject gameWorld;
    private Rigidbody rb = null;
    private float fruitSpeed = 0.2f;
    private float rotationSpeed = 3.5f;
    private bool isExploded = false;
    private GameObject audioManagerObject;
    private AudioManager audioManager;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Cache variables
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManagerObject = GameObject.Find("AudioManager");
        audioManager = audioManagerObject.GetComponent<AudioManager>();
        gameWorld = GameObject.Find("GameWorld");
        
        // Starting velocity and rotation
        rb = gameObject.GetComponent<Rigidbody>();
        Vector3 fruitForce = new Vector3(Random.Range(-fruitSpeed, fruitSpeed), Random.Range(2.5f, 3.3f) / 1.5f,
            Random.Range(-fruitSpeed, fruitSpeed));
        rb.velocity = fruitForce;
        rb.angularVelocity = fruitForce;
        rb.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * rotationSpeed);
        
        audioManager.PlayBombThrowSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Blade") &&
            other.gameObject.GetComponent<BladeLogic>().velocity.magnitude > 0.00003f && !isExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        isExploded = true;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        
        // Explode FX
        gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play(true);
        audioManager.PlayExplosionSound();
        gameObject.GetComponent<AudioSource>().Stop();
        
        // End game
        gameManager.GetComponent<GameManager>().GameOver();
        gameObject.transform.Find("FuseFX").gameObject.SetActive(false);
        Destroy(gameObject, 6f);
    }
}