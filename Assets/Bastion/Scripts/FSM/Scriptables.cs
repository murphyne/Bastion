using UnityEngine;

namespace FSM
{
    public abstract class MonoContext<TContext>
        : MonoBehaviour, IContext<TContext>
        where TContext : IContext<TContext> { }

    public abstract class ScriptableState<TContext>
        : ScriptableObject, IState<TContext>
        where TContext : IContext<TContext>
    {
        IState IState.Handle(IContext context) => Handle((TContext) context);

        public abstract IState<TContext> Handle(TContext context);
    }

    public abstract class MonoAgent<TContext>
        : MonoBehaviour, IAgent<TContext>
        where TContext : IContext<TContext>
    {
        IState IAgent.CurrentState => CurrentState;
        void IAgent.SetState(IState newState) =>
            SetState((IState<TContext>) newState);

        public abstract TContext Context { get; }
        public abstract IState<TContext> CurrentState { get; }

        public abstract void SetState(IState<TContext> newState);

        protected void Start()
        {
            SetState(CurrentState);
        }

        protected void Update()
        {
            var newState = CurrentState?.Handle(Context);
            if (newState != null)
            {
                SetState(newState);
            }
        }
    }

    public abstract class ScriptableAction<TContext>
        : ScriptableObject, IAction<TContext>
        where TContext : IContext<TContext>
    {
        IState IAction.Apply(IContext context) => Apply((TContext) context);

        public abstract IState<TContext> Apply(TContext context);
    }
}
