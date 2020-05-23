namespace Arena.Creature.Conditions
{
    public class CreatureConditionIsAnimating
    {
        public static bool CheckStatic(CreatureContext context)
        {
            return context.isAnimating;
        }
    }
}
