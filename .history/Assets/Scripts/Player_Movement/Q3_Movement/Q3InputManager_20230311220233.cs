using UnityEngine;

public class Q3InputManager : MonoBehaviour
{
  private NewPlayerInput playerControls;
  private NewPlayerInput.PlayerActions p_Movement;
  private void OnEnable()
  {
    p_Movement.Enable();
  }
  private void OnDisable()
  {
    p_Movement.Disable();
  }

}
