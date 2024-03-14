
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Needs : MonoBehaviour
{
    int hungerPerSecond = 10;
    int healthPerSecond = 10;

    [HideInInspector] public int minHunger;
    [HideInInspector] public int minHealth;

    [SerializeField] private int _health;
    public int health
    {
        get { return _health; }
    }
    [SerializeField] private int _hunger;
    public int hunger
    {
        get { return _hunger; }
    }

    Coroutine starvingCoroutine;
    Animal stats;

    //public delegate void Del(); //testing event...
    //public event Del AnimalDeath;
    public delegate void Del();
    public event Del HealthLoss;

    public void Awake()
    {
        stats = GetComponent<Stats>().stats; //does this update all the way?
        Body body = GetComponent<Body>();
        HealthLoss += body.FlashRed;
        //AnimalDeath += brain.KillCount; 

        minHunger = (stats.maxHunger * 7) / 10;
        minHealth = (stats.maxHealth * 3) / 10;

        _health = stats.maxHealth;
        _hunger = stats.maxHunger; 

        StartCoroutine(HungerPerSecond());
    }

    public void Update()
    {
        if (hunger <= 0)
        {
            if (starvingCoroutine == null)
            {
                starvingCoroutine = StartCoroutine(HealthPerSecond());
            }
        }
        else
        {
            if (starvingCoroutine != null)
            {
                StopCoroutine(starvingCoroutine);
                starvingCoroutine = null;
            }
        }
        if (health == 0)
        {
            KillAnimal();
        }
    }



    void KillAnimal() //WIP
    {
        //AnimalDeath(); //event?
        //Destroy(agent);
        gameObject.AddComponent<Rigidbody>();
        //CorpseBehaviour corpseComponent = gameObject.AddComponent<CorpseBehaviour>();
        //corpseComponent.stats = stats;
        name += (" (Dead)");
        gameObject.tag = "Meat";
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
        _health = Mathf.Clamp(newValue, 0, stats.maxHealth);

        if (value < 0)
        {
            HealthLoss();
            //GetComponent<Body>().FlashRed(); //lostHealth Event
        }
    }
    public void ChangeHunger(int value)
    {
        int newValue = _hunger + value;
        _hunger = Mathf.Clamp(newValue, 0, stats.maxHunger);
    }

  
}