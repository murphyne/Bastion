using FSM;
using UnityEngine;

namespace Creature.Actions
{
    [CreateAssetMenu(order = 101,
        fileName = "CreatureActionWander",
        menuName = "Creature/Action Wander")]
    public class CreatureActionWander : CreatureAction,
        IActionEnter<CreatureContext>, IActionExit<CreatureContext>
    {
        [SerializeField] private CreatureState nextState;

        public void Enter(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Enter)}({context})");
        }

        public void Exit(CreatureContext context)
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

            var colliders = Physics.OverlapSphere(position,
                context.SearchRange, context.SearchLayer);
            foreach (var collider in colliders)
            {
                // Ignore itself.
                if (collider.gameObject == context.gameObject) continue;

                var colliderPosition = collider.transform.position;
                var distance = colliderPosition - position;

                // Ignore objects beyond field of view.
                var angle = Vector3.Angle(context.transform.forward, distance);
                if (angle > context.FieldOfView / 2) continue;

                // Ignore objects without required context component.
                Physics.Raycast(position, distance, out var hit);
                var creatureContext =
                    hit.collider.gameObject.GetComponent<CreatureContext>();
                if (!creatureContext) continue;

                context.enemy = creatureContext;
                return nextState;
            }

            return null;
        }
    }
}
