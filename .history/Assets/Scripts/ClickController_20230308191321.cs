using UnityEngine;
using UnityEngine.AI;

public class ClickController : MonoBehaviour
{
  public NavMeshAgent agent;

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButton(1))
    {
      Ray movePos = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(movePos, out var hitInfo))
      {
        agent.SetDestination(hitInfo.point);
      }
    }
  }
}
