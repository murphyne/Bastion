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

            if (currentState != null) currentState.Exit(context);
            currentState = newState as CreatureState;
            if (currentState != null) currentState.Enter(context);
        }

        protected override void Start()
        {
            context = GetComponent<CreatureContext>();
            SetState(CurrentState);
        }

        protected override void Update()
        {
            // Debug.Log($"{this}.{nameof(Update)}()");

            var newState = CurrentState?.Handle(context);
            if (newState == null) return;
            if (ReferenceEquals(newState, currentState)) return;
            SetState(newState);
        }
    }
}
