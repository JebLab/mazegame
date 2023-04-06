using System;
using UnityEngine;

public class PlayerStats : CharStats
{
  private void Start()
  {
    maxHP = 100;
    currHP = maxHP;

    maxStam = 100;
    currStam = maxStam;
  }
}
