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
            var position = context.transform.position;
            var enemyPosition = context.enemy.transform.position;
            var distance = enemyPosition - position;
            var attackRangeSquare = context.AttackRange * context.AttackRange;
            var withinRange = distance.sqrMagnitude < attackRangeSquare;
            return withinRange;
        }
    }
}
