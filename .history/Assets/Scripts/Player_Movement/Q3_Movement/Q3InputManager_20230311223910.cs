using Q3Movement;
using UnityEngine;

public class Q3InputManager : MonoBehaviour
{
  private NewPlayerInput playerControls;
  private NewPlayerInput.PlayerActions p_Input;


  private Q3PlayerMotor Q3_Motor;
  private Q3PlayerLook Q3_Look;

  void Awake()
  {
    playerControls = new NewPlayerInput();
    p_Input = playerControls.Player;

    Q3_Motor = GetComponent<Q3PlayerMotor>();
    Q3_Look = GetComponent<Q3PlayerLook>();
  }
  void FixedUpdate()
  {
    Q3_Motor.ProcessMove(p_Input.Move.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    Q3_Look.ProcessLook(p_Input.Look.ReadValue<Vector2>());
  }
  private void OnEnable()
  {
    p_Input.Enable();
  }
  private void OnDisable()
  {
    p_Input.Disable();
  }

}
