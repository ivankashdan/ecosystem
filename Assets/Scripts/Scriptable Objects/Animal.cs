using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animal")]
public class Animal : Lifeform
{
    public Diet diet; 
    public int maxHealth;
    public int maxHunger;
    public int maxThirst;
    public int attackStrength;
    public int attackRange;
    public int attackDPS;
    public float walkSpeedMultiplier;
    public float runSpeedMultiplier;
    public float searchRadius;
    public GameObject model;
    //public LifeCycle lifeCycle; 
}

public enum Diet
{   
    Herbavore = 0,
    Carnivore = 1,
    Omnivore = 2,
}
