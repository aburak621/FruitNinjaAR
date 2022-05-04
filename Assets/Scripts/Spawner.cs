using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject[] fruits;
    [SerializeField] private float spawnWidth = 0.3f;
    [SerializeField] private float spawnHeight = 0.3f;

    private GameObject gameWorld;
    private float timer = 0f;
    private float currentRunTime = 0f;
    private float timeBetweenSpawns = 0f;
    private float timeBetweenTemplates = 0f;
    private bool shouldSpawn = false;
    private int burstSpawnCount = 1;
    private int totalSpawnAmount = 1;
    private int bombCount = 1;
    private bool containsBomb = false;
    private bool[] bombOrder;
    private int currentIndexInBombOrder = 0;
    private bool isBurstSpawn = false;
    private bool shouldChooseTemplate = true;

    // Start is called before the first frame update
    void Start()
    {
        gameWorld = GameObject.Find("GameWorld");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        currentRunTime += Time.deltaTime;
        if (shouldChooseTemplate)
        {
            ChooseSpawnTemplate();
            timer -= timeBetweenTemplates;
        }

        if (timer >= timeBetweenSpawns && shouldSpawn)
        {
            if (containsBomb)
            {
                if (bombOrder[currentIndexInBombOrder])
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y,
                        Random.Range(-spawnHeight, spawnHeight));
                    GameObject currentThrowable = Instantiate(bombPrefab, spawnPosition,
                        Quaternion.identity,
                        gameWorld.transform);
                    Destroy(currentThrowable, 3f);
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y,
                        Random.Range(-spawnHeight, spawnHeight));
                    GameObject currentThrowable = Instantiate(fruits[Random.Range(0, fruits.Length)], spawnPosition,
                        Quaternion.identity,
                        gameWorld.transform);
                    Destroy(currentThrowable, 3f);
                }

                currentIndexInBombOrder++;
            }
            else
            {
                for (int i = 0; i < burstSpawnCount; i++)
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y,
                        Random.Range(-spawnHeight, spawnHeight));
                    GameObject currentThrowable = Instantiate(fruits[Random.Range(0, fruits.Length)], spawnPosition,
                        Quaternion.identity,
                        gameWorld.transform);
                    Destroy(currentThrowable, 3f);
                }
            }

            timer -= timeBetweenSpawns;
            totalSpawnAmount--;
            if (totalSpawnAmount <= 0)
            {
                shouldChooseTemplate = true;
            }
        }
    }

    void ChooseSpawnTemplate()
    {
        burstSpawnCount = 1;
        isBurstSpawn = Random.Range(0f, 1f) <= 0.4f;
        totalSpawnAmount = 1;
        if (isBurstSpawn)
        {
            int extraBurstFruits = Mathf.Clamp((int) (currentRunTime / 60), 0, 3);
            burstSpawnCount = Random.Range(3 + extraBurstFruits, 5 + extraBurstFruits);
            containsBomb = false;
        }
        else
        {
            timeBetweenSpawns = Mathf.Clamp(Random.Range(0.5f, 0.75f) - currentRunTime / 450, 0.111f, 1f);
            int extraFruits = Mathf.Clamp((int) (currentRunTime / 30), 0, 5);
            totalSpawnAmount = Random.Range(1 + extraFruits, 5 + extraFruits);

            containsBomb = Random.Range(0f, 1f) <= 0.33f;
            currentIndexInBombOrder = 0;
            bombCount = Random.Range(1, 2 + Mathf.Clamp((int) (currentRunTime / 60), 0, 1));
            bombOrder = new bool[totalSpawnAmount];
            for (int i = 0; i < bombCount;)
            {
                int randomIndex = Random.Range(0, totalSpawnAmount);
                if (bombOrder[randomIndex] == false)
                {
                    bombOrder[randomIndex] = true;
                    i++;
                }
            }
        }

        timer = timeBetweenSpawns;
        timeBetweenTemplates = Mathf.Clamp(3.5f - currentRunTime / 40, 2.2f, 7f);
        shouldChooseTemplate = false;
    }

    public void StartSpawning()
    {
        shouldSpawn = true;
        shouldChooseTemplate = true;
        currentRunTime = 0f;
    }

    public void StopSpawning()
    {
        shouldSpawn = false;
    }
}