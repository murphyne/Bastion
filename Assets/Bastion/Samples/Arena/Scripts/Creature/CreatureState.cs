using System.Collections.Generic;
using Bastion.FSM;
using UnityEngine;

namespace Arena.Creature
{
    [CreateAssetMenu(order = 1,
        fileName = "CreatureState",
        menuName = "Creature/State")]
    public class CreatureState : ScriptableState<CreatureContext>,
        IStatePluggable<CreatureContext>
    {
        IEnumerable<IAction> IStatePluggable.Actions => actions;
        IEnumerable<ITransition> IStatePluggable.Transitions => transitions;

        public IEnumerable<IAction<CreatureContext>> Actions => actions;
        [SerializeField] private CreatureAction[] actions;

        public IEnumerable<ITransition<CreatureContext>> Transitions => transitions;
        [SerializeField] private CreatureTransition[] transitions;

        public override IState<CreatureContext> Handle(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Handle)}({context})");

            foreach (var action in Actions)
            {
                action?.Apply(context);
            }

            foreach (var transition in Transitions)
            {
                var newState = transition?.Check(context);

                if (newState != null)
                {
                    Debug.Log($"{context} -> {nameof(newState)}: {newState}");

                    return newState;
                }
            }

            return null;
        }

        public override void Enter(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Enter)}({context})");

            if (Actions != null)
            {
                foreach (var action in Actions)
                {
                    action?.Enter(context);
                }
            }
        }

        public override void Exit(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Exit)}({context})");

            if (Actions != null)
            {
                foreach (var action in Actions)
                {
                    action?.Exit(context);
                }
            }
        }
    }
}
