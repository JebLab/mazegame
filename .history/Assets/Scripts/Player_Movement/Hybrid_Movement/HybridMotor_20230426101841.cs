using System;
using UnityEngine;

public class HybridMotor : MonoBehaviour
{
  private HybridManager h_Man;
  private CharacterStats p_Stats;

  [Serializable]
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
  // [Tooltip("Automatically sprint if there is enough stamina")]
  // [SerializeField] private bool m_AutoSprint = false;
  [Tooltip("How precise air control is")]
  [SerializeField] private float m_AirControl = 0.3f;
  [SerializeField] private MovementSettings m_GroundSettings = new MovementSettings(7, 14, 10);
  [SerializeField] private MovementSettings m_AirSettings = new MovementSettings(7, 2, 2);
  [SerializeField] private MovementSettings m_StrafeSettings = new MovementSettings(1, 50, 50);

  /// <summary>
  /// Returns player's current speed.
  /// </summary>

  public float Speed { get { return m_Character.velocity.magnitude; } }

  public static CharacterController m_Character;
  private Vector3 m_MoveDirectionNorm = Vector3.zero;
  private Vector3 m_PlayerVelocity = Vector3.zero;

  // Used to queue the next jump just before hitting the ground.
  private bool m_JumpQueued = false;

  private bool m_isSprinting = false;
  private bool m_hasRegened;

  private bool m_CrouchQueued = false;

  // Used to display real time friction values.
  private float m_PlayerFriction = 0;

  private double m_Stamina;

  private int cooldown;

  private Vector3 m_MoveInput;
  public Transform m_Tran;

  void Start()
  {
    m_Tran = gameObject.transform;
    m_Character = GetComponent<CharacterController>();
    h_Man = GetComponent<HybridManager>();
    p_Stats = GetComponent<CharacterStats>();

    m_Stamina = 30;//p_Stats.maxStamina;
  }

  private void Update()
  {
    QueueJump();
    isSprinting();
    QueueCrouch();


    // Set movement state.
    if (m_Character.isGrounded)
    {
      GroundMove();
    }
    else
    {
      AirMove();
    }
  }

  public void MoveInput(Vector2 p_Input)
  {
    m_MoveInput = new Vector3(p_Input.x, 0, p_Input.y);
    m_Character.Move(m_PlayerVelocity * Time.deltaTime);
  }

  private void QueueJump()
  {
    if (m_AutoBunnyHop)
    {
      m_JumpQueued = h_Man.g_Player.Jump.IsPressed();
      return;
    }

    if (h_Man.g_Player.Jump.WasPressedThisFrame() && !m_JumpQueued)
    {
      m_JumpQueued = true;
      m_CrouchQueued = false;
    }

    if (h_Man.g_Player.Jump.WasReleasedThisFrame())
    {
      m_JumpQueued = false;
    }
  }

  public void isSprinting()
  {
    if (h_Man.g_Player.Sprint.IsPressed() && (m_Stamina > 10) && m_MoveInput != Vector3.zero && m_hasRegened)
    {
      {
        // Debug.Log(m_Stamina);
        // Debug.Log("sprint pressed");
        m_isSprinting = true;
        m_CrouchQueued = false;
        //p_Stats.StaminaDrain = 5;
        m_Stamina -= 1;

      }
      // m_isSprinting = true;
      // m_CrouchQueued = false;

    }
    else if (!h_Man.g_Player.Sprint.IsPressed())
    {
      if (m_Stamina < 30)
        m_Stamina += 1;
      m_isSprinting = false;
    }
    if (m_Stamina < 15)
      m_isSprinting = false;
    if (m_Stamina <= 0) { m_hasRegened = false; }

    if (m_isSprinting == false)
    {
      if (currentStamina <= maxStamina - 0.01f)
      {
        StaminaRegen = 2;
        currentStamina += StaminaRegen * Time.deltaTime;
        if (currentStamina >= maxStamina)
        {
          currentStamina = maxStamina;
          hasRegened = true;
        }
      }
      if (!hasRegened)
      {
        StaminaRegen = 0.5f;
        currentStamina += StaminaRegen * Time.deltaTime;
      }

    }
  }
}
