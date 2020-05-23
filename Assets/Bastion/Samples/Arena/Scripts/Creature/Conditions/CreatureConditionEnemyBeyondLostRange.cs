using UnityEngine;

namespace Arena.Creature.Conditions
{
    [CreateAssetMenu(order = 202,
        fileName = "CreatureConditionEnemyBeyondLostRange",
        menuName = "Creature/Condition EnemyBeyondLostRange")]
    public class CreatureConditionEnemyBeyondLostRange : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            var position = context.transform.position;
            var enemyPosition = context.enemy.transform.position;
            var distance = enemyPosition - position;

            if (distance.sqrMagnitude > context.LostRange * context.LostRange)
            {
                return true;
            }

            return false;
        }
    }
}
