using UnityEngine;

namespace Arena.Creature.Actions
{
    [CreateAssetMenu(order = 102,
        fileName = "CreatureActionChase",
        menuName = "Creature/Action Chase")]
    public class CreatureActionChase : CreatureAction
    {
        public override void Enter(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Enter)}({context})");

            var enemyPosition = context.enemy.transform.position;
            context.NavMeshAgent.SetDestination(enemyPosition);
        }

        public override void Exit(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Exit)}({context})");

            context.NavMeshAgent.ResetPath();
        }

        public override void Apply(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Apply)}({context})");

            var enemyPosition = context.enemy.transform.position;
            context.NavMeshAgent.SetDestination(enemyPosition);
        }
    }
}
