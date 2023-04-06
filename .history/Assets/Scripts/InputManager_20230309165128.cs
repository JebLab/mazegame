using UnityEngine;

public class InputManager : MonoBehaviour
{
  private PlayerInput playerControls;
  private PlayerInput.PlayerActions GeneralMovement;

  private PlayerMotor motor;
  private PlayerLook look;
  // Start is called before the first frame update

  void Awake()
  {
    playerControls = new PlayerInput();
    GeneralMovement = playerControls.Player;

    motor = GetComponent<PlayerMotor>();
    look = GetComponent<PlayerLook>();

  }
  void FixedUpdate()
  {
    motor.ProcessMove(GeneralMovement.Movement.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    look.ProcessLook(GeneralMovement.Camera.ReadValue<Vector2>());
  }
  private void OnEnable()
  {
    GeneralMovement.Enable();
  }
  private void OnDisable()
  {
    GeneralMovement.Disable();
  }
}
