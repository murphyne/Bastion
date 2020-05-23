namespace Arena.Creature.Conditions
{
    public class CreatureConditionEnemyAlive : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            return CheckStatic(context);
        }

        public static bool CheckStatic(CreatureContext context)
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
