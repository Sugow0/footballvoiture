using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Vector2 = System.Numerics.Vector2;

public class PlayerController : MonoBehaviour
{
   //public InputActionAsset inputAsset;

   
   [SerializeField] private LayerMask groundLayer;
   [SerializeField] private WheelCollider[] wheelColliders;
   
    public float Speed = 600.0f;
    public float AccelerationAmount = 0;
    public bool isAccelerating = false;
    
    public float Boost = 30.0f;
    public float BoostAmount = 0;
    public bool isBoosting = false;
    
    public float RotationSpeed = 60.0f;
    public float RotationAmount = 0;
    public bool isRotating = false;
    
    public float jumpForce = 10.0f;
    public float JumpingAmount = 0;
    public bool isJumping = false;
    
    public CinemachineCamera MainCamera;
    //public CinemachineFollow FollowTarget;
    public AudioSource AS;
    public GameManager GM;
    
    public bool isGrounded = false;
    public int jumpLeft = 2;
    
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AS = GetComponent<AudioSource>();
        GM = FindFirstObjectByType<GameManager>();
        AS.volume = GM.audioVolume;
    }

    public void AccelerateCallback(InputAction.CallbackContext obj)
    {
        
        AccelerationAmount = obj.ReadValue<float>();
        isAccelerating = AccelerationAmount != 0;
    }

    public void RotateCallback(InputAction.CallbackContext obj)
    {
        
        RotationAmount = obj.ReadValue<float>();
        isRotating = RotationAmount != 0;
    }
    
    public void JumpCallback(InputAction.CallbackContext obj)
    {
        
        JumpingAmount = obj.ReadValue<float>();
        isJumping = JumpingAmount != 0;
    }
    
    public void BoostCallback(InputAction.CallbackContext obj)
    {
        
        BoostAmount = obj.ReadValue<float>();
        isBoosting = BoostAmount != 0;
    }
    
    void FixedUpdate()
    {
        bool wheelonfloor = false;
        foreach (WheelCollider wheel in wheelColliders)
        {
            WheelHit hit;
            if (wheel.GetGroundHit(out hit)) // Vérifie si la roue touche quelque chose
            {
                if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0) // Vérifie si c'est le sol
                {
                    wheelonfloor = true;
                    
                }
            }
        }
        
        
        isGrounded = wheelonfloor;

        if (isBoosting)
        {
            rb.linearVelocity += transform.forward * (Speed * BoostAmount * Time.fixedDeltaTime * Boost);
        }

        if (isAccelerating)
        {
            if (!AS.isPlaying)
            {
                AS.Play();
            }
            
            rb.linearVelocity += transform.forward * (Speed * AccelerationAmount * Time.fixedDeltaTime);
            if (isBoosting)
            {
                rb.linearVelocity += transform.forward * (Speed * BoostAmount * AccelerationAmount * Time.fixedDeltaTime * Boost);
            }
          

        }
        if (isRotating)
        {
            rb.transform.Rotate(transform.up *(RotationAmount * RotationSpeed * Time.fixedDeltaTime));
        }
        if (isJumping && jumpLeft != 0)
        {
            isJumping = false;
            rb.AddForce(Vector3.up * (jumpForce * JumpingAmount), ForceMode.Impulse);
            jumpLeft -= 1;

        }
        if (isGrounded)
        {
            jumpLeft = 2;
        }

       
        AS.pitch = Mathf.Lerp(-1f, 2f, rb.linearVelocity.magnitude / Speed);
    }
}
