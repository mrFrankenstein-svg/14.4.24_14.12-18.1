using System;
using Unity.VisualScripting;
using UnityEngine;
using static SetingsValuses;

/*
    This file has a commented version with details about how each line works. 
    The commented version contains code that is easier and simpler to read. This file is minified.
*/


/// <summary>
/// Main script for third-person movement of the character in the game.
/// Make sure that the object that will receive this script (the player) 
/// has the Player tag and the Character Controller component.
/// </summary>
public class ThirdPersonController : MonoBehaviour, IScriptHubFixUpdateFunction, IScriptHubUpdateFunction
{

    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 3.5f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;
    [Space]
    [Space]
    [Tooltip("This is not a setting option.The parameter just shows Vector3 points for the camera and does not set anything.")]
    [SerializeField] Vector3 cameraPsition;
    [SerializeField] GameObject cameraPsitionGameobject;
    [SerializeField] float cameraExtendingMultiplier = 100;

    // Player states
    bool isSprinting = false;
    bool previosIsSprinting = false;
    bool isCrouching = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputCrouch;
    bool inputSprint;

    Animator animator;
    CharacterController cc;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        StartFunction();

        // Message informing the user that they forgot to add an animator
        if (animator == null)
            Debug.LogWarning("Hey buddy, you don't have the Animator component in your player. Without it, the animations won't work.");

        cameraPsitionGameobject = gameObject.transform.Find("CameraPointCube").gameObject;
    }


    // Update is only being used here to identify keys and trigger animations
    public void ScriptHubUpdate()
    {
        // Input checkers
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputSprint = Input.GetAxis("Fire3") == 1f;
        // Unfortunately GetAxis does not work with GetKeyDown, so inputs must be taken individually
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        // Check if you pressed the crouch input key and change the player's state
        if (inputCrouch)
            isCrouching = !isCrouching;

        // Run and Crouch animation
        // If dont have animator component, this block wont run
        if (cc.isGrounded && animator != null)
        {
            // Run
            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed);

            // Sprint
            isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
            if (isSprinting && !previosIsSprinting)
            {
                animator.SetFloat("SprintMultiplayer", 1.25f);
                previosIsSprinting = isSprinting;
            }
            if (!isSprinting && previosIsSprinting)
            {
                animator.SetFloat("SprintMultiplayer", 1.0f);
                previosIsSprinting = isSprinting;
            }
        }

    }


    // With the inputs and animations defined, FixedUpdate is responsible for applying movements and actions to the player
    public void ScriptHubFixUpdate()
    {
        if (inputHorizontal != 0 || inputVertical != 0)
        {
            // Sprinting velocity boost or crounching desacelerate
            float velocityAdittion = 0;
            if (isSprinting)
                velocityAdittion = sprintAdittion;
            if (isCrouching)
                velocityAdittion = -(velocity * 0.50f); // -50% velocity

            // Direction movement
            float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
            float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
            // Add gravity to Y axis
            float directionY = 0 - gravity * Time.deltaTime;


            // --- Character rotation --- 

            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            // Relate the front with the Z direction (depth) and right with X (lateral movement)
            forward = forward * directionZ;
            right = right * directionX;

            if (directionX != 0 || directionZ != 0)
            {
                float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
            }

            // --- End rotation ---


            Vector3 verticalDirection = Vector3.up * directionY;
            Vector3 horizontalDirection = forward + right;

            Vector3 moviment = verticalDirection + horizontalDirection;
            cc.Move(moviment);
            UploadingCameraPsition(moviment);
        }
    }
    void UploadingCameraPsition(Vector3 moviment)
    {
        float x = Math.Abs(moviment.x);
        float z = Math.Abs(moviment.z);
        float Z;
        float Y;

        if (z >= x)
        {

            if (moviment.z < 0)
                Z = z * MoveCameraWhileMovingDown;
            else
                Z = z * MoveCameraWhileMovingUp;

        }
        else
            Z = x;

        Y = Z * 0.4f;

        cameraPsition = new Vector3(0, Y * cameraExtendingMultiplier, Z * cameraExtendingMultiplier);
        cameraPsitionGameobject.transform.localPosition = cameraPsition;
    }

    public void StartFunction()
    {
        ScriptHub.AddToScriptsList(this);
        //hub.AddToScriptsList(this, ScriptHubUpdateFunction.FunctionOneSecondUpdate);
    }

    public void CameraExtendingMultiplierCalculating(int numberOfSteps)
    {
        int tackts;
        float number;
        if (numberOfSteps < 0)
        {
            tackts = numberOfSteps * -1;
            number = -1;
        }
        else
        {
            tackts = numberOfSteps;
            number = 1;
        }

        for (int i = 0; i < tackts; i++)
        {
            if(cameraExtendingMultiplier>= ValueOfMinimumCameraZoom && cameraExtendingMultiplier<=ValueOfMaximumCameraZoom)
                cameraExtendingMultiplier = cameraExtendingMultiplier + number;
        }

    }
}
