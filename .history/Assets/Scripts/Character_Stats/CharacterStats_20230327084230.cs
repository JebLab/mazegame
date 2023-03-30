using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
  [Header("Health Parameters")]
  [SerializeField]
  private float maxHeatlh = 100;
  private float currentHealth;
  public static Action<float> OnDamage;
  public static Action<float> OnTakeDamage;
  public static Action<float> OnHeal;


  private void OnEnable()
  {

  }
  private void OnDisable()
  {

  }
  private void ApplyDamage(float dmg)
  {
    currentHealth -= dmg;
    OnDamage?.Invoke(currentHealth);
    if (currentHealth <= 0)
    {
      OnDeath();
    }
  }

  private void OnDeath()
  {
    currentHealth = 0;

    Console.WriteLine("YOU DIED");
  }


}
