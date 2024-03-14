using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class AnimalStatus : BaseBehaviour
{
    int hungerPerSecond = 10;
    int healthPerSecond = 10;

    [HideInInspector] public int minHunger;
    [HideInInspector] public int minHealth;

    private int _health;
    public int health
    {
        get { return _health; }
    }
    private int _hunger;
    public int hunger
    {
        get { return _hunger; }
    }
    //int _thirst;
    //public float thirst { get; set; }

    Coroutine hungerCoroutine;
    Coroutine starvingCoroutine;
    Coroutine flashRedCoroutine;

    public Color original;
    int flashRedDamageMin = 100;

    public AnimalStatus(ref AnimalBehaviour animal) : base(ref animal)
    {
        //original = animal.model.material.color;

        //stats = lifeCycle[0]; //for now
        minHunger = (animal.stats.maxHunger * 7) / 10;
        minHealth = (animal.stats.maxHealth * 3) / 10;

        _health = animal.stats.maxHealth;
        _hunger = minHunger; //stats.maxHunger; //temporary, set back to max...

        //thirst = age.maxThirst;

        hungerCoroutine = animal.StartCoroutine(HungerPerSecond());

    }

    public void Needs()
    {
        if (hunger <= 0)
        {
            if (starvingCoroutine == null)
            {
                starvingCoroutine = animal.StartCoroutine(HealthPerSecond());
            }
        }
        else
        {
            if (starvingCoroutine != null)
            {
                animal.StopCoroutine(starvingCoroutine);
                starvingCoroutine = null;
            }
        }
        if (health == 0)
        {
            animal.KillAnimal();
        }
    }

    IEnumerator HungerPerSecond()
    {
        while (true)
        {
            ChangeHunger(-hungerPerSecond);

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator HealthPerSecond()
    {
        while (true)
        {
            ChangeHealth(-healthPerSecond);
            yield return new WaitForSeconds(1);
        }
    }


    public void ChangeHealth(int value)
    {
        int newValue = _health + value;
        _health = Mathf.Clamp(newValue, 0, animal.stats.maxHealth);

        if (value < 0)
        {
            if (flashRedCoroutine == null)
            {
                //flashRedCoroutine = animal.StartCoroutine(FlashRed());
            }
        }
    }
    public void ChangeHunger(int value)
    {
        int newValue = _hunger + value;
        _hunger = Mathf.Clamp(newValue, 0, animal.stats.maxHunger);
    }

    //IEnumerator FlashRed()
    //{
    //    animal.model.material.color = Color.red;

    //    yield return new WaitForSeconds(0.1f);

    //    animal.model.material.color = original;
    //    flashRedCoroutine = null;
    //}



   
}
