using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuildingBase : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth = 200f;
    [FormerlySerializedAs("attackRange")] public float attackInterval = 2f;

    
    public TextMeshProUGUI hpText;
    public Slider hpSlider;
    
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHpui();
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            GameManager2.Instance.buildingsList.Remove(gameObject);
        }
    }
    
    
    public virtual void UpdateHpui()
    {
        hpSlider.value = currentHealth / maxHealth;
        hpText.text = currentHealth + "/" + maxHealth;
    }
    
}
