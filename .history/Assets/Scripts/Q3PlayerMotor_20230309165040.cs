// Based off of code from  IsaiahKelly &  WiggleWizard

using UnityEngine;

public class Q3PlayerMotor : MonoBehaviour
{

  [System.Serializable]
  public class MovementSettings
  {
    public float MaxSpeed;
    public float Acceleration;
    public float Deceleration;

    public MovementSettings(float maxSpeed, float accel, float decel)
    {
      MaxSpeed = maxSpeed;
      Acceleration = accel;
      Deceleration = decel;
    }
  }

  [Header("Movement")]
  [SerializeField] private float m_Friction = 6;
  [SerializeField] private float m_Gravity = 20;
  [SerializeField] private float m_JumpForce = 8;
  [Tooltip("Automatically jump when holding jump button")]
  [SerializeField] private bool m_AutoBunnyHop = false;
  [Tooltip("How precise air control is")]
  [SerializeField] private float m_AirControl = 0.3f;
  [SerializeField] private MovementSettings m_GroundSettings = new MovementSettings(7, 14, 10);
  [SerializeField] private MovementSettings m_AirSettings = new MovementSettings(7, 2, 2);
  [SerializeField] private MovementSettings m_StrafeSettings = new MovementSettings(1, 50, 50);

  public float Speed { get { return m_Character.velocity.magnitude; } }

  private CharacterController m_Character;
  private Vector3 m_MoveDirectionNorm = Vector3.zero;
  private Vector3 m_PlayerVelocity = Vector3.zero;


  void Start()
  {
    m_Tran = transform;
    m_Character = GetComponent<CharacterController>();
    if (!m_Camera)
      m_Camera = Camera.main;

    m_CamTran = m_Camera.transform;
    m_MouseLook.Init(m_Tran, m_CamTran);
  }

  void Update()
  {
    m_MoveInput = new Vector3(input.x, 0, input.y);
    m_MouseLook.UpdateCursorLock();
    QueueJump();

    if (m_Character.isGrounded)
    {
      GroundMove();
    }
    else
    {
      AirMove();
    }
    // Rotate the character and camera.
    m_MouseLook.LookRotation(m_Tran, m_CamTran);

    // Move the character.
    m_Character.Move(m_PlayerVelocity * Time.deltaTime);

  }
  // Queues the next jump.
  private void QueueJump()
  {
    if (m_AutoBunnyHop)
    {
      m_JumpQueued = InputManager.GeneralMovement.Jump.performed;
      return;
    }

    if (InputManager.GeneralMovement.Jump.isPressed && !m_JumpQueued)
    {
      m_JumpQueued = true;
    }

    if (InputManager.GeneralMovement.Jump.canceled)
    {
      m_JumpQueued = false;
    }
  }
  private void AirMove()
  {
    float accel;

    var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
    wishdir = m_Tran.TransformDirection(wishdir);

    float wishspeed = wishdir.magnitude;
    wishspeed *= m_AirSettings.MaxSpeed;

    wishdir.Normalize();
    m_MoveDirectionNorm = wishdir;

    // CPM Air control.
    float wishspeed2 = wishspeed;
    if (Vector3.Dot(m_PlayerVelocity, wishdir) < 0)
    {
      accel = m_AirSettings.Deceleration;
    }
    else
    {
      accel = m_AirSettings.Acceleration;
    }

    // If the player is ONLY strafing left or right
    if (m_MoveInput.z == 0 && m_MoveInput.x != 0)
    {
      if (wishspeed > m_StrafeSettings.MaxSpeed)
      {
        wishspeed = m_StrafeSettings.MaxSpeed;
      }

      accel = m_StrafeSettings.Acceleration;
    }

    Accelerate(wishdir, wishspeed, accel);
    if (m_AirControl > 0)
    {
      AirControl(wishdir, wishspeed2);
    }

    // Apply gravity
    m_PlayerVelocity.y -= m_Gravity * Time.deltaTime;
  }

  // Air control occurs when the player is in the air, it allows players to move side
  // to side much faster rather than being 'sluggish' when it comes to cornering.
  private void AirControl(Vector3 targetDir, float targetSpeed)
  {
    // Only control air movement when moving forward or backward.
    if (Mathf.Abs(m_MoveInput.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
    {
      return;
    }

    float zSpeed = m_PlayerVelocity.y;
    m_PlayerVelocity.y = 0;
    /* Next two lines are equivalent to idTech's VectorNormalize() */
    float speed = m_PlayerVelocity.magnitude;
    m_PlayerVelocity.Normalize();

    float dot = Vector3.Dot(m_PlayerVelocity, targetDir);
    float k = 32;
    k *= m_AirControl * dot * dot * Time.deltaTime;

    // Change direction while slowing down.
    if (dot > 0)
    {
      m_PlayerVelocity.x *= speed + targetDir.x * k;
      m_PlayerVelocity.y *= speed + targetDir.y * k;
      m_PlayerVelocity.z *= speed + targetDir.z * k;

      m_PlayerVelocity.Normalize();
      m_MoveDirectionNorm = m_PlayerVelocity;
    }

    m_PlayerVelocity.x *= speed;
    m_PlayerVelocity.y = zSpeed; // Note this line
    m_PlayerVelocity.z *= speed;
  }

  // Handle ground movement.
  private void GroundMove()
  {
    // Do not apply friction if the player is queueing up the next jump
    if (!m_JumpQueued)
    {
      ApplyFriction(1.0f);
    }
    else
    {
      ApplyFriction(0);
    }

    var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
    wishdir = m_Tran.TransformDirection(wishdir);
    wishdir.Normalize();
    m_MoveDirectionNorm = wishdir;

    var wishspeed = wishdir.magnitude;
    wishspeed *= m_GroundSettings.MaxSpeed;

    Accelerate(wishdir, wishspeed, m_GroundSettings.Acceleration);

    // Reset the gravity velocity
    m_PlayerVelocity.y = -m_Gravity * Time.deltaTime;

    if (m_JumpQueued)
    {
      m_PlayerVelocity.y = m_JumpForce;
      m_JumpQueued = false;
    }
  }

  private void ApplyFriction(float t)
  {
    // Equivalent to VectorCopy();
    Vector3 vec = m_PlayerVelocity;
    vec.y = 0;
    float speed = vec.magnitude;
    float drop = 0;

    // Only apply friction when grounded.
    if (m_Character.isGrounded)
    {
      float control = speed < m_GroundSettings.Deceleration ? m_GroundSettings.Deceleration : speed;
      drop = control * m_Friction * Time.deltaTime * t;
    }

    float newSpeed = speed - drop;
    m_PlayerFriction = newSpeed;
    if (newSpeed < 0)
    {
      newSpeed = 0;
    }

    if (speed > 0)
    {
      newSpeed /= speed;
    }

    m_PlayerVelocity.x *= newSpeed;
    // playerVelocity.y *= newSpeed;
    m_PlayerVelocity.z *= newSpeed;
  }

  // Calculates acceleration based on desired speed and direction.
  private void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
  {
    float currentspeed = Vector3.Dot(m_PlayerVelocity, targetDir);
    float addspeed = targetSpeed - currentspeed;
    if (addspeed <= 0)
    {
      return;
    }

    float accelspeed = accel * Time.deltaTime * targetSpeed;
    if (accelspeed > addspeed)
    {
      accelspeed = addspeed;
    }

    m_PlayerVelocity.x += accelspeed * targetDir.x;
    m_PlayerVelocity.z += accelspeed * targetDir.z;
  }
}

