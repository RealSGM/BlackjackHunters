using UnityEngine;

public class DevilWheel : MonoBehaviour
{
    [SerializeField] GameObject wheel;
    [SerializeField] Rigidbody2D rb;

    readonly int minSpeed = 400;
    readonly int maxSpeed = 1000;
    readonly int wheelSectorCount = 8;
    readonly float duration = 5f;

    private int spinSpeed;
    private float startTime;
    private bool spinning = false;

    //--------------------------------------------------------------------------
    private void Start()
    {
        // Hide wheel
        // Show stationary with devil being approached
        // Dialogue
        // Spin wheel
    }

    //--------------------------------------------------------------------------
    public void SpinWheel()
    {
        // Store time when spin starts ans then start the spin
        startTime = Time.time;
        spinSpeed = Random.Range(minSpeed, maxSpeed);
        spinning = true;
    }

    //--------------------------------------------------------------------------
    private void Update()
    {
        if (spinning)
        {
            // While spinning, slowly decrease the value of the angular velocity over time
            float t = (Time.time - startTime) / duration;
            rb.angularVelocity = Mathf.SmoothStep(spinSpeed, 0f, t);

            if (rb.angularVelocity == 0f)
            {
                // Once the wheel has stopped moving, detect the prize
                spinning = false;
                DetectPrize();
            }
        }
    }

    //--------------------------------------------------------------------------
    private void DetectPrize()
    {
        // Retrieve rotation as Vector3 of degrees
        Vector3 rotation = wheel.transform.rotation.eulerAngles;
        float Z = rotation.z;

        // Convert from -180 to 180 -> 0 to 360
        if (Z < 0) Z += 360f;

        // Calculate which sector it is in.
        int sector = Mathf.FloorToInt(Z * wheelSectorCount / 360);

        switch (sector)
        {
            // Match the sector to the output code
            case 0: case 4:
                Debug.Log("Red");
                break;
            case 1: case 5:
                Debug.Log("Green");
                break;
            case 2: case 6:
                Debug.Log("Blue");
                break;
            case 3: case 7:
                Debug.Log("Yellow");    
                break;
        }

        // Instantiate exit button
        Instantiate(GameManager.Game.exitEncounterPrefab, GameObject.Find("Canvas").transform);
    }
}