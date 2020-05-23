using UnityEngine;

namespace Arena.Creature.Conditions
{
    [CreateAssetMenu(order = 203,
        fileName = "CreatureConditionEnemyDead",
        menuName = "Creature/Condition EnemyDead")]
    public class CreatureConditionEnemyDead : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            if (context.enemy == null) return true;
            if (context.enemy.Health <= 0) return true;
            return false;
        }
    }
}
