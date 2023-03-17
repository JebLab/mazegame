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

}
