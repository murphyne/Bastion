using Arena.Creature.Conditions;
using Bastion.FSM;
using UnityEngine;

namespace Arena.Creature.Actions
{
    [CreateAssetMenu(order = 101,
        fileName = "CreatureActionWander",
        menuName = "Creature/Action Wander")]
    public class CreatureActionWander : CreatureAction
    {
        [SerializeField] private CreatureState nextState;

        public override void Enter(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Enter)}({context})");
        }

        public override void Exit(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Exit)}({context})");

            context.NavMeshAgent.ResetPath();
        }

        public override IState<CreatureContext> Apply(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Apply)}({context})");

            var position = context.transform.position;

            const float interval = 2f;
            var currRemainder = Time.time % interval;
            var prevRemainder = currRemainder - Time.deltaTime;

            if (currRemainder >= 0f && prevRemainder < 0f)
            {
                const float radius = 5f;
                var targetOffset = Random.onUnitSphere * radius;
                var targetPosition = position + targetOffset;
                context.NavMeshAgent.SetDestination(targetPosition);
            }

            if (CreatureConditionEnemyVisible.CheckStatic(context))
            {
                return nextState;
            }

            return null;
        }
    }
}
