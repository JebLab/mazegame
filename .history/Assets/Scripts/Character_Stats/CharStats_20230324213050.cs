using System;
using UnityEngine;

public class CharStats : MonoBehaviour
{
  public int currHP, maxHP, currStam, maxStam;

  bool m_isDead;

  public void checkHP()
  {
    if (currHP >= maxHP)
    {
      currHP = maxHP;
    }
    if (currHP <= 0)
    {
      currHP = 0;
      m_isDead = true;
    }
  }

}
