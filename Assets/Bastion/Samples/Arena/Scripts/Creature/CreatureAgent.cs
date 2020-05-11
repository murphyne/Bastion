using Bastion.FSM;
using UnityEngine;

namespace Arena.Creature
{
    [RequireComponent(typeof(CreatureContext))]
    public sealed class CreatureAgent : MonoAgent<CreatureContext>
    {
        public override IState<CreatureContext> CurrentState => currentState;
        [SerializeField] private CreatureState currentState;

        public override CreatureContext Context => context;
        [SerializeField] private CreatureContext context;

        public override void SetState(IState<CreatureContext> newState)
        {
            Debug.Log($"{this}.{nameof(SetState)}({newState})");

            if (newState == null) return;

            if (currentState != null && currentState.Actions != null)
            {
                foreach (var action in currentState.Actions)
                {
                    (action as IActionExit<CreatureContext>)?.Exit(context);
                }
            }

            (currentState as CreatureStateEnterExit)?.Exit(context);
            currentState = newState as CreatureState;
            (currentState as CreatureStateEnterExit)?.Enter(context);

            if (currentState != null && currentState.Actions != null)
            {
                foreach (var action in currentState.Actions)
                {
                    (action as IActionEnter<CreatureContext>)?.Enter(context);
                }
            }
        }

        private new void Start()
        {
            context = GetComponent<CreatureContext>();
            SetState(CurrentState);
        }

        private new void Update()
        {
            // Debug.Log($"{this}.{nameof(Update)}()");

            var newState = CurrentState?.Handle(context);
            if (newState != null) SetState(newState);
        }
    }
}
