using Bastion.FSM;
using UnityEngine;

namespace Arena.Creature
{
    [System.Serializable]
    public class CreatureTransition : ScriptableTransition<CreatureContext>
    {
        [SerializeField] private CreatureCondition condition;
        [SerializeField] private CreatureState nextState;

        public override ICondition<CreatureContext> Condition => condition;
        public override IState<CreatureContext> NextState => nextState;
    }
}
