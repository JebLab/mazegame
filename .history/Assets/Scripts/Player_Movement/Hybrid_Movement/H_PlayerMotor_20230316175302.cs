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
}
