using UnityEngine;

public class Q3InputManager : MonoBehaviour
{
  private NewPlayerInput playerControls;
  private NewPlayerInput.PlayerActions GeneralMovement;

  private Q3PlayerMotor motor;
  private PlayerLook look;
  // Start is called before the first frame update

  void Awake()
  {
    playerControls = new NewPlayerInput();
    GeneralMovement = playerControls.Player;

    motor = GetComponent<Q3PlayerMotor>();
    // look = GetComponent<Q3PlayerLook>();

    // GeneralMovement.Sprint.performed += ctx => motor.SprintPressed();
    // GeneralMovement.Sprint.canceled += ctx => motor.SprintReleased();

    // GeneralMovement.Crouch.performed += ctx => motor.CrouchPressed();
    // GeneralMovement.Crouch.canceled += ctx => motor.CrouchReleased();
  }
  void FixedUpdate()
  {
    motor.ProcessMove(GeneralMovement.Move.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    // look.ProcessLook(GeneralMovement.Look.ReadValue<Vector2>());
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
