using FSM;
using UnityEngine;

namespace Creature
{
    [CreateAssetMenu(order = 2,
        fileName = "CreatureStateEnterExit",
        menuName = "Creature/State EnterExit")]
    public sealed class CreatureStateEnterExit : CreatureState,
        IStateEnter<CreatureContext>, IStateExit<CreatureContext>
    {
        void IStateEnter.Enter(IContext context) =>
            Enter((CreatureContext) context);
        void IStateExit.Exit(IContext context) =>
            Exit((CreatureContext) context);

        public void Enter(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Enter)}({context})");

            foreach (var action in Actions)
            {
                (action as IActionEnter<CreatureContext>)?.Enter(context);
            }
        }

        public void Exit(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Exit)}({context})");

            foreach (var action in Actions)
            {
                (action as IActionExit<CreatureContext>)?.Exit(context);
            }
        }
    }
}
