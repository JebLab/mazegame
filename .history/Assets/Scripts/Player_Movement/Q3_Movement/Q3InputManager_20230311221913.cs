using Q3Movement;
using UnityEngine;

public class Q3InputManager : MonoBehaviour
{
  private NewPlayerInput playerControls;
  private NewPlayerInput.PlayerActions p_Movement;
  private Q3PlayerMotor Q3_Motor;
  private Q3PlayerLook Q3_Look;

  void FixedUpdate()
  {
    Q3_Motor.ProcessMove(move.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    Q3_Look.ProcessLook(p_Movement.Look.ReadValue<Vector2>());
  }
  private void OnEnable()
  {
    move = p_Movement.Player.Move;
    move.Enable();
  }
  private void OnDisable()
  {
    p_Movement.Disable();
  }

}
