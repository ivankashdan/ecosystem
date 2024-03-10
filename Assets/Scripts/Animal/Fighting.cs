using UnityEngine;
using UnityEngine.AI;

public class Fighting : SearchBehaviour
{

    public Fighting(ref AnimalBehaviour animal) : base(ref animal) { }


    float attackTimePassed = 0;
    
    public void Attack()
    {
        if (IsTargetStillVisible())
        {
            Transform target = animal.GetTarget();
            animal.agent.destination = target.position;

            float targetThickness = target.transform.localScale.magnitude / 2;
            if (animal.agent.remainingDistance < animal.stats.attackRange + targetThickness) //collide if inside collider
            {
                Strike(target);
            }
        }
    }

    void Strike(Transform target)
    {
        attackTimePassed += Time.deltaTime;

        if (target.GetComponent<AnimalBehaviour>())
        {
            AnimalBehaviour prey = target.GetComponent<AnimalBehaviour>();
            if (prey.status.health > 0) //continually update enemy position
            {
                animal.agent.destination = target.position; //keep attacking
                if (attackTimePassed > animal.stats.attackDPS)
                {
                    attackTimePassed = 0;
                    //Debug.Log($"Damage = {status.stats.attackStrength}");
                    prey.status.ChangeHealth(-animal.stats.attackStrength);
                }
            }
        }
        else
        {
            animal.RemoveTarget();
        }
        
        
    }


}
