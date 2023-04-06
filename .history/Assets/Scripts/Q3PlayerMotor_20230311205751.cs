/// <summary>
/// This script is a modification of
/// </summary>


using UnityEngine;

namespace Q3Movement
{
  /// <summary>
  /// This script handles Quake III CPM(A) mod style player movement logic.
  /// </summary>
  [RequireComponent(typeof(CharacterController))]

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
  }
}
