using Q3Movement;
using UnityEngine;
using UnityEngine.InputSystem;

public class Q3InputManager : MonoBehaviour
{
  private NewPlayerInput playerControls;
  private InputAction move;
  private InputAction look;


  private Q3PlayerMotor Q3_Motor;
  private Q3PlayerLook Q3_Look;

  void Awake()
  {
  }
  void FixedUpdate()
  {
    Q3_Motor.ProcessMove(move.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    Q3_Look.ProcessLook(look.ReadValue<Vector2>());
  }
  private void OnEnable()
  {
    look = playerControls.Player.Look;
    move = playerControls.Player.Move;
    move.Enable();
    look.Enable();
  }
  private void OnDisable()
  {
    move.Disable();
    look.Disable();
  }

}
