using UnityEngine;
using UnityEngine.InputSystem;

public class Q3InputManager : MonoBehaviour
{
  private NewPlayerInput playerControls;
  private NewPlayerInput.PlayerActions GeneralMovement;

  private PlayerMotor motor;
  private PlayerLook look;
  // Start is called before the first frame update

  void Awake()
  {
    playerControls = new PlayerInput();
    GeneralMovement = playerControls.Player;

    motor = GetComponent<PlayerMotor>();
    look = GetComponent<PlayerLook>();

    GeneralMovement.Sprint.performed += ctx => motor.SprintPressed();
    GeneralMovement.Sprint.canceled += ctx => motor.SprintReleased();

    GeneralMovement.Crouch.performed += ctx => motor.CrouchPressed();
    GeneralMovement.Crouch.canceled += ctx => motor.CrouchReleased();

    GeneralMovement.Jump.performed += ctx => motor.Jump();
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
}
