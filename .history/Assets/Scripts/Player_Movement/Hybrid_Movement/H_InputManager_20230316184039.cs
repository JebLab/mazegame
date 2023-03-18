using UnityEngine;

public class HybridManager : MonoBehaviour
{
  private NewPlayerInput p_Controls;
  private NewPlayerInput.PlayerActions g_Player;

  private HybridMotor h_Motor;
  private var g_Movement, g_Camera;


  void Awake()
  {
    p_Controls = new NewPlayerInput();
    g_Player = p_Controls.Player;
    var g_Movement = g_Player.Move;
    var g_Camera = g_Player.Look;

    h_Motor = GetComponent<HybridMotor>();

  }

  void FixedUpdate()
  {
    h_Motor.MoveInput(g_Movement.ReadValue<Vector2>());
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