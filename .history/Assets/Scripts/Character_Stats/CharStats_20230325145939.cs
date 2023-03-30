using UnityEngine;

public class CharacterStats : MonoBehaviour
{
  public double currHP, maxHP, currStam, maxStam;

  public bool m_isDead;

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
  public void cheackStam()
  {
    if (currStam >= maxStam)
    {
      currStam = maxStam;
    }
    if (currStam <= 0)
    {
      currStam = 0;
    }
  }

  // public virtual Death()
  // {
  //   // Override
  // }
}
