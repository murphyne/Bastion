using UnityEngine;

namespace Arena.Creature.Conditions
{
    [CreateAssetMenu(order = 205,
        fileName = "CreatureConditionEnemyWithinAttackRange",
        menuName = "Creature/Condition EnemyWithinAttackRange")]
    public class CreatureConditionEnemyWithinAttackRange : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            return CheckStatic(context);
        }

        public static bool CheckStatic(CreatureContext context)
        {
            var position = context.transform.position;
            var enemyPosition = context.enemy.transform.position;
            var distance = enemyPosition - position;

            if (distance.sqrMagnitude < context.AttackRange * context.AttackRange)
            {
                return true;
            }

            return false;
        }
    }
}
