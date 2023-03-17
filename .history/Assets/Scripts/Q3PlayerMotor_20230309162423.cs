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
      m_JumpQueue =
    }
  }
}
