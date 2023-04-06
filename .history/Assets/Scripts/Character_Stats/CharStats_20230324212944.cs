using System;
using UnityEngine;

public class CharStats : MonoBehaviour
{
  public int currHP, maxHP, currStam, maxStam;

  bool isDead;

  public void checkHP()
  {
    if (currHP >= maxHP)
    {
      curr_HP = maxHP;
    }
    if (currHP <= 0)
    {

    }
  }

}
