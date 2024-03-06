using UnityEngine;
using UnityEngine.AI;



public class Fighting : SearchBehaviour
{

    float attackTimePassed = 0;
    
    public void Attack()
    {
        if (IsTargetStillVisible())
        {
            Transform target = animal.GetTarget();
            agent.destination = target.position;

            float targetThickness = target.transform.localScale.magnitude / 2;
            if (agent.remainingDistance < status.stats.attackRange + targetThickness) //collide if inside collider
            {
                Strike(target);
            }
        }
    }

    void Strike(Transform target)
    {
        attackTimePassed += Time.deltaTime;

        AnimalStatus prey = target.GetComponent<AnimalStatus>();
        if (prey.health > 0) //continually update enemy position
        {
            agent.destination = target.position; //keep attacking
            if (attackTimePassed > status.stats.attackDPS)
            {
                attackTimePassed = 0;
                //Debug.Log($"Damage = {status.stats.attackStrength}");
                prey.health -= status.stats.attackStrength;
            }
        }
        
    }


}
