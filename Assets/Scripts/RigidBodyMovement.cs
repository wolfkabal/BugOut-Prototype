using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;

    [SerializeField] private Rigidbody PlayerBody;
    //[SerializeField] private LayerMask FloorMask;
    //[SerializeField] private Transform FeetTransform;
    [Space]
    [SerializeField] private Vector3 CenterMass;
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float LiftForce;
    private bool isGrounded;

    private void Update() { }

    private void FixedUpdate() {
        MovePlayer();
        PlayerBody.centerOfMass = CenterMass;
    }
    
    void OnCollisionEnter(Collision other)
    {
        isGrounded = true;

        Rigidbody body = other.collider.attachedRigidbody;
        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        Debug.Log(body.name);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        Debug.Log(body.name);

        // We don't want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        
        // Apply the push
        body.velocity = pushDir * LiftForce;
    }

    /// <summary>
    /// Performs movement on the applied RigidBody
    /// </summary>
    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical"); 
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

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

        // Performs the rotation math. Gets the destination rotation Quarternion
        Quaternion targetRotation = Quaternion.LookRotation(movement);    
        targetRotation = Quaternion.RotateTowards(
            transform.rotation, //current player rotation
            targetRotation, //target rotation
            RotationSpeed * Time.fixedDeltaTime); 
        
        PlayerBody.MoveRotation(targetRotation);
        PlayerBody.MovePosition(PlayerBody.position + movement * Speed * Time.fixedDeltaTime);
          
    }
}
