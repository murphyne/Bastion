using UnityEngine;

namespace Arena.Creature.Conditions
{
    [CreateAssetMenu(order = 204,
        fileName = "CreatureConditionEnemyVisible",
        menuName = "Creature/Condition EnemyVisible")]
    public class CreatureConditionEnemyVisible : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            var position = context.NavMeshAgent.transform.position;
            var colliders = UnityEngine.Physics.OverlapSphere(position,
                context.SearchRange, context.SearchLayer);
            foreach (var collider in colliders)
            {
                // Ignore itself.
                if (collider.gameObject == context.gameObject) continue;

                var colliderPosition = collider.transform.position;
                var distance = colliderPosition - position;

                // Ignore objects beyond field of view.
                var angle = UnityEngine.Vector3.Angle(context.transform.forward, distance);
                if (angle > context.FieldOfView / 2) continue;

                // Ignore objects without required context component.
                UnityEngine.Physics.Raycast(position, distance, out var hit);
                var creatureContext =
                    hit.collider.gameObject.GetComponent<CreatureContext>();
                if (!creatureContext) continue;

                context.enemy = creatureContext;
                return true;
            }

            return false;
        }
    }
}
