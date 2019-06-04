using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    private float InputX, InputZ, Speed, verticalVelocity;

    private Camera cam;
    private CharacterController characterController;
    private Animator anim;
    private Rigidbody rb;

    private Vector3 desiredMoveDirection;
    public bool isWalking;
    public float movementSpeed = 1f;
    public float gravity = 0.5f;
    public float jumpSpeed = 10.0f;

    [SerializeField] float rotationSpeed = 0.3f;
    [SerializeField] float allowRotation = 0.1f; 
    [SerializeField] bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        InputDecider();
        MovementManager();
        GroundCheck();
    }


    void InputDecider() 
    {
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        isWalking = Speed > 0;
        anim.SetBool("isRunning", isWalking);

        if (Speed > allowRotation)
        {
            RotationManager();
        }
        else
        {
            desiredMoveDirection = Vector3.zero;
        }

    }



    void RotationManager()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);

    }

    void MovementManager()
    {

        Vector3 moveDirection = desiredMoveDirection * (movementSpeed * Time.deltaTime);
        moveDirection = new Vector3(moveDirection.x, verticalVelocity, moveDirection.z);
        characterController.Move(moveDirection); 

    }

    void GroundCheck()
    {
        grounded = characterController.isGrounded;

        if(grounded)
        {
            verticalVelocity = -0.1f;
            anim.SetBool("isGrounded", grounded);
            anim.SetBool("inAir", false);

            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpSpeed * Time.deltaTime;
            }
        }
        else
        {
            anim.SetBool("isGrounded", grounded);
            anim.SetBool("inAir", true);

            verticalVelocity -= gravity * Time.deltaTime;
        }

    }



}
