using System;
using UnityEngine;

[Serializable]
public class CharacterStats : MonoBehaviour
{
  private HybridMotor Motor;
  [Header("Health Parameters")]

  private float currentHealth;
  [SerializeField] public float maxHeatlh = 100.0f;

  private float currentStamina;
  [SerializeField] public float maxStamina = 100.0f;
  [SerializeField] public float staminaCost = 5.0f;

  public bool exhausted;
  public bool hasRegened;

  [Header("Stamina Regen Parameters")]
  [Range(0, 50)][SerializeField] public float StaminaDrain;
  [Range(0, 50)][SerializeField] public float StaminaRegen;


  private void OnDeath()
  {
    currentHealth = 0;

    Console.WriteLine("YOU DIED");
  }

  private void Start()
  {

  }

  private void Update()
  {

    //Motor = GetComponent<HybridMotor>();
  }
}
