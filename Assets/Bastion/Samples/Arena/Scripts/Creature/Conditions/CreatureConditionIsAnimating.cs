using UnityEngine;

namespace Arena.Creature.Conditions
{
    [CreateAssetMenu(order = 206,
        fileName = "CreatureConditionIsAnimating",
        menuName = "Creature/Condition IsAnimating")]
    public class CreatureConditionIsAnimating : CreatureCondition
    {
        public override bool Check(CreatureContext context)
        {
            return context.isAnimating;
        }
    }
}
