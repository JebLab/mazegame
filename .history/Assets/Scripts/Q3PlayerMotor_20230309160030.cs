using UnityEngine;

public class Q3PlayerMotor : MonoBehaviour
{
  private CharacterController controller;

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

  void Update()
  {
    isGrounded = controller.isGrounded;
  }


}
