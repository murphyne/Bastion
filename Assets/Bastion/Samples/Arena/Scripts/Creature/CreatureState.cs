using System.Collections.Generic;
using FSM;
using UnityEngine;

namespace Creature
{
    [CreateAssetMenu(order = 1,
        fileName = "CreatureState",
        menuName = "Creature/State")]
    public class CreatureState : ScriptableState<CreatureContext>,
        IStatePluggable<CreatureContext>
    {
        IEnumerable<IAction> IStatePluggable.Actions => actions;

        public IEnumerable<IAction<CreatureContext>> Actions => actions;
        [SerializeField] private CreatureAction[] actions;

        public override IState<CreatureContext> Handle(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Handle)}({context})");

            foreach (var action in Actions)
            {
                var newState = action?.Apply(context);

                if (newState != null)
                {
                    Debug.Log($"{context} -> {nameof(newState)}: {newState}");

                    return newState;
                }
            }

            return null;
        }
    }
}
