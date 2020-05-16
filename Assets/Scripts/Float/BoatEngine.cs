using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEngine : MonoBehaviour
{
    public Rigidbody boatRB;
    [Tooltip("the boat maximum power")]
    public float maxPower;
    [Tooltip("How fast should the engine accelerate")]
    public float powerFactor;
    [Tooltip("current engine power, for debugging")]
    public float currentPower;
    [Tooltip("speed of rotation")]
    public float angularSpeed = 1f;
    [Tooltip("max speed of rotation")] [Range(0.2f, 0.5f)]
    public float maxRotationSpeed = 0.5f;



    private float boatRotation_YAngle = 0f;

    public Transform boatTransform;
    private BoatController boatController;

    private void Start()
    {
        if (boatRB == null)
            boatRB = GetComponent<Rigidbody>();
        boatController = GetComponent<BoatController>();
        if (boatTransform == null)
            boatTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        UserInput();
    }

    private void FixedUpdate()
    {
        UpdateBoatPower();
    }

    /// <summary>
    /// handle user input
    /// </summary>
    void UserInput()
    {
        if (BoatControlTrigger.ifEnterTrigger == true) // disable character move and enable boat move
        {
            Debug.Log("enable boat move");
            // forward
            if (Input.GetKey(KeyCode.I))
            {
                if (boatController.getCurrentSpeed() < 50f && currentPower < maxPower)
                {
                    currentPower += 1f * powerFactor;
                }
            }
            // stop
            else
                currentPower = 0f;

        // Steer left
        if (Input.GetKey(KeyCode.J))
        {
            boatRotation_YAngle -= boatRotation_YAngle > -maxRotationSpeed ? 0.001f : 0f;
            boatRB.angularVelocity = boatTransform.forward * angularSpeed * boatRotation_YAngle;

        }
        // Steer right
        else if (Input.GetKey(KeyCode.L))
        {
            boatRotation_YAngle += boatRotation_YAngle < maxRotationSpeed ? 0.001f : 0f;
            boatRB.angularVelocity = boatTransform.forward * angularSpeed * boatRotation_YAngle;
        }
        else
        {
            boatRotation_YAngle = 0;

        }

        }
    }

    void UpdateBoatPower()
    {
        //if (BoatControlTrigger.ifEnterTrigger == true) // disable character move and enable boat move
        //{

        //float waveYPos = WaveManager.instance.GetWaveHeight(transform.position.x);
        //boatRB.AddForceAtPosition(forceToAdd, boatTransform.position, ForceMode.Acceleration);


        //}
        // head forward
        if (BoatControlTrigger.ifEnterTrigger == true) // disable character move and enable boat move
        {
            Vector3 forceToAdd = boatTransform.right * currentPower;
            boatRB.AddForce(forceToAdd, ForceMode.Force);
        }

    }
}
