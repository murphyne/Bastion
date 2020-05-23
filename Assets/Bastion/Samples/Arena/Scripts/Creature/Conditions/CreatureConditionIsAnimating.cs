namespace Arena.Creature.Conditions
{
    public class CreatureConditionIsAnimating : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            return CheckStatic(context);
        }

        public static bool CheckStatic(CreatureContext context)
        {
            return context.isAnimating;
        }
    }
}
