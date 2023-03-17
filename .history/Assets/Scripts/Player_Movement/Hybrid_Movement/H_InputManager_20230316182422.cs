using UnityEngine;

public class HybridManager : MonoBehaviour
{
  private NewPlayerInput p_Controls;
  // private NewPlayerInput.PlayerActions g_Movement;

  private HybridMotor h_Motor;

  void Awake()
  {
    p_Controls = new NewPlayerInput();
    g_Movement = p_Controls.Player.Move;
    g_Camera = p_Controls.Player.Look;

    h_Motor = GetComponent<MoveInput>();

  }

  void FixedUpdate()
  {
    h_Motor.MoveInput(g_Movement.Move.ReadValue<Vector2>());
  }
  private void OnEnable()
  {
    g_Movement.Enable();
  }
  private void OnDisable()
  {
    g_Movement.Disable();
  }

}
