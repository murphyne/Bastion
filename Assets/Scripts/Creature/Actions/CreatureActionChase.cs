using FSM;
using UnityEngine;

namespace Creature.Actions
{
    [CreateAssetMenu(order = 102,
        fileName = "CreatureActionChase",
        menuName = "Creature/Action Chase")]
    public class CreatureActionChase : CreatureAction,
        IActionEnter<CreatureContext>, IActionExit<CreatureContext>
    {
        public void Enter(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Enter)}({context})");

            var enemyPosition = context.enemy.transform.position;
            context.NavMeshAgent.SetDestination(enemyPosition);
        }

        public void Exit(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Exit)}({context})");

            context.NavMeshAgent.ResetPath();
        }

        public override IState<CreatureContext> Apply(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Apply)}({context})");

            var enemyPosition = context.enemy.transform.position;
            context.NavMeshAgent.SetDestination(enemyPosition);

            return null;
        }
    }
}
