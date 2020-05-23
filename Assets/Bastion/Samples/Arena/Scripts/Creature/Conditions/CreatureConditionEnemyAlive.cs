namespace Arena.Creature.Conditions
{
    public class CreatureConditionEnemyAlive
    {
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
