using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AnimalStatus : MonoBehaviour
{

    [SerializeField] List<Animal> lifeCycle;
    [SerializeField] GameObject deadState;
    int hungerPerSecond = 10;
    int healthPerSecond = 10;
    [HideInInspector] public int minHunger;
    [HideInInspector] public int minHealth;

    Coroutine hungerCoroutine;
    Coroutine starvingCoroutine;
    Coroutine flashRedCoroutine;
    int _health; 
    int _hunger;

    int flashRedDamageMin = 100;

    AnimalBehaviour animal;

    Renderer renderer;
    Color original;

    public int health
    {
        get { return _health; }
        set {
            int previousValue = _health;
            _health = Mathf.Clamp(value, 0, stats.maxHealth);

            if ((previousValue - flashRedDamageMin) > _health)
            {
                if (flashRedCoroutine == null)
                {
                    flashRedCoroutine = StartCoroutine(FlashRed());
                }
            }
        }
    }
    public int hunger
    {
        get { return _hunger; }
        set { _hunger = Mathf.Clamp(value, 0, stats.maxHunger); }
    }
    //int _thirst;
    //public float thirst { get; set; }

    [HideInInspector] public Animal stats;
   

    private void Awake()
    {
        animal = GetComponent<AnimalBehaviour>();

        renderer = GetComponentInChildren<Renderer>(); //could still put renderer on top level?
        original = renderer.material.color;

        stats = lifeCycle[0]; //for now

        health = stats.maxHealth;
        hunger = 700; //stats.maxHunger; //temporary, set back to max...
        minHunger = (stats.maxHunger * 7)/10; 
        minHealth = (stats.maxHealth * 3)/10;
        //thirst = age.maxThirst;
        Debug.Log(minHealth);

        hungerCoroutine = StartCoroutine(HungerPerSecond());
    }

  

    private void Update()
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
            animal.KillAnimal();
        }
    }

    
    IEnumerator HungerPerSecond()
    {
        while (true)
        {
            hunger -= hungerPerSecond;

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator HealthPerSecond()
    {
        while (true)
        {
            health -= healthPerSecond;
            yield return new WaitForSeconds(1);
        }
    }



    IEnumerator FlashRed()
    {
        renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        renderer.material.color = original;
        flashRedCoroutine = null;
    }

    private void OnDestroy()
    {
        renderer.material.color = original;
    }


   
}
