using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
  [Header("Health Parameters")]
  [SerializeField]
  private float maxHeatlh = 100;
  private float currentHealth;

  private void OnDamage(float dmg)
  {
    currentHealth -= dmg;
    if (currentHealth <= 0)
    {
      OnDeath();
    }
  }

  private void OnDeath()
  {
    currentHealth = 0;

    Console.("YOU DIED");
  }


}
