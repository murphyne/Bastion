namespace Arena.Creature.Conditions
{
    public class CreatureConditionEnemyBeyondLostRange
    {
        public static bool CheckStatic(CreatureContext context)
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
