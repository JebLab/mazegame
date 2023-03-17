using System;
using UnityEngine;

public class HybridMotor : MonoBehaviour
{
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
  [SerializeField] private float m_Friction = 1;
  [SerializeField] private float m_Gravity = -9.81;
  [SerializeField] private float m_JumpForce = 1;
  [Tooltip("How precise air control is")]
  [SerializeField] private float m_AirControl = 0.3f;
  [SerializeField] private MovementSettings m_GroundSettings = new MovementSettings(7, 14, 10);
  [SerializeField] private MovementSettings m_AirSettings = new MovementSettings(7, 2, 2);
  [SerializeField] private MovementSettings m_StrafeSettings = new MovementSettings(1, 50, 50);

  private CharacterController m_Character;
  public float Speed { get { return m_Character.velocity.magnitude; } }
  private Vector3 m_MoveDirectionNorm = Vector3.zero;
  private Vector3 m_PlayerVelocity = Vector3.zero;

}
