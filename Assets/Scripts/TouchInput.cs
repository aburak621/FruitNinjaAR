using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private GameObject bladePrefab = null;
    [SerializeField] private int maxNumOfBlades = 4;
    [SerializeField] private float maxXRot = 15f;
    [SerializeField] private float maxYRot = -26f;

    private GameManager gameManager;
    private List<GameObject> blades;
    private Camera _camera;
    private bool isFOVSet = false;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        blades = new List<GameObject>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        for (int i = 0; i < maxNumOfBlades; i++)
        {
            blades.Add(Instantiate(bladePrefab, _camera.transform.position, _camera.transform.rotation));
            blades[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFOVSet)
        {
            if ((int) _camera.fieldOfView != 60)
            {
                float FOVAdjustment = _camera.fieldOfView / 41.4921f;
                maxXRot *= FOVAdjustment;
                maxYRot *= FOVAdjustment;
                isFOVSet = true;
            }
        }

        if (Input.touchCount > 0 && !gameManager.isPaused)
        {
            for (int i = maxNumOfBlades; i > Input.touchCount; i--)
            {
                blades[i - 1].SetActive(false);
            }

            for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
            {
                if (touchIndex >= maxNumOfBlades)
                {
                    break;
                }

                TouchPhase touchPhase = Input.GetTouch(touchIndex).phase;
                Vector3 screenPosition = Input.GetTouch(touchIndex).position;
                screenPosition.z = 0.0f;
                Vector3 bladePosition = _camera.ScreenToWorldPoint(screenPosition);

                Quaternion bladeRotation = Quaternion.Euler(
                    MapValue(0, 1, -maxYRot, maxYRot, screenPosition.y / Screen.height),
                    MapValue(0, 1, -maxXRot, maxXRot, screenPosition.x / Screen.width), 0);

                blades[touchIndex].transform
                    .SetPositionAndRotation(bladePosition, _camera.transform.rotation * bladeRotation);
                blades[touchIndex].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < maxNumOfBlades; i++)
            {
                blades[i].SetActive(false);
            }
        }
    }

    public float MapValue(float a0, float a1, float b0, float b1, float a)
    {
        return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
    }
}