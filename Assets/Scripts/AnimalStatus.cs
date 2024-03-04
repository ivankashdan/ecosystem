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
    [SerializeField] int hungerPerSecond = 10;
    [SerializeField] public int minHunger = 700;
    [SerializeField] int healthPerSecond = 10;

    Coroutine hungerCoroutine;
    Coroutine starvingCoroutine;
    Coroutine flashRedCoroutine;
    [SerializeField] int _health; 
    [SerializeField] int _hunger;
    public int health
    {
        get { return _health; }
        set {
            int previousValue = _health;
            _health = Mathf.Clamp(value, 0, stats.maxHealth);

            if (previousValue > _health)
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
        stats = lifeCycle[0]; //for now

        //edible = age.edible;
        health = stats.maxHealth;
        hunger = 50; //temporary, set back to max...
        //thirst = age.maxThirst;

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
            Die();
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
        Renderer renderer = transform.GetComponent<Renderer>();
        Color original = renderer.material.color;
        renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        renderer.material.color = original;
        flashRedCoroutine = null;
    }

    public void Die() 
    {
        //AnimalDied();
        GameObject corpse = Instantiate(deadState, transform.position, transform.rotation, transform.parent);
        ConsumableBehaviour consumabale = corpse.GetComponent<ConsumableBehaviour>();
        consumabale.age = stats;
        corpse.name = consumabale.age.objectName + (" (Dead)");
        Destroy(gameObject);

    }
}
