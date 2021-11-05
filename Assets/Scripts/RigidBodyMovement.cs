using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;

    [SerializeField] private Rigidbody PlayerBody;
    //[SerializeField] private LayerMask FloorMask;
    //[SerializeField] private Transform FeetTransform;
    [Space]
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float RotationSpeed;
    private bool isGrounded;

    private void Update() { }

    private void FixedUpdate() {
        MovePlayer();
    }
    
    void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical"); 
        Vector3 movement = new Vector3(horizontalInput, PlayerBody.transform.position.y, verticalInput).normalized;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
             if(isGrounded)
             {
                PlayerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                isGrounded = false;
                //PlayerBody.MovePosition(Vector3.up + movement * JumpForce * Time.fixedDeltaTime);
            }
        }   
           
        if (movement == Vector3.zero)
        {
            return;
        }    

        Quaternion targetRotation = Quaternion.LookRotation(movement);    
        targetRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            RotationSpeed * Time.fixedDeltaTime); 

        PlayerBody.MovePosition(PlayerBody.position + movement * Speed * Time.fixedDeltaTime);
        PlayerBody.MoveRotation(targetRotation);  
    }
}