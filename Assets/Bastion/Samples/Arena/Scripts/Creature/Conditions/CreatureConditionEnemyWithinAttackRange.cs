namespace Arena.Creature.Conditions
{
    public class CreatureConditionEnemyWithinAttackRange
    {
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
