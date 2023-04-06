using UnityEngine;

public class HybridManager : MonoBehaviour
{
  private NewPlayerInput p_Controls;
  private NewPlayerInput.PlayerActions g_Player;

  private HybridMotor h_Motor;


  void Awake()
  {
    p_Controls = new NewPlayerInput();
    g_Player = p_Controls.Player;

    h_Motor = GetComponent<HybridMotor>();

  }

  void FixedUpdate()
  {
    var g_Movement = g_Player.Move;
    h_Motor.MoveInput(g_Movement.ReadValue<Vector2>());
  }
  void LateUpdate()
  {
    var g_Camera = g_Player.Look;

  }

  private void OnEnable()
  {
    g_Player.Enable();
  }
  private void OnDisable()
  {
    g_Player.Disable();
  }

}
