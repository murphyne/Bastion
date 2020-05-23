using UnityEngine;

namespace Arena.Creature.Conditions
{
    [CreateAssetMenu(order = 201,
        fileName = "CreatureConditionEnemyAlive",
        menuName = "Creature/Condition EnemyAlive")]
    public class CreatureConditionEnemyAlive : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            if (context.enemy == null || context.enemy.Health <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
