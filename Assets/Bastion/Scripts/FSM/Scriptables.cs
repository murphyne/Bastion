using UnityEngine;

namespace Bastion.FSM
{
    public abstract class MonoContext<TContext>
        : MonoBehaviour, IContext<TContext>
        where TContext : IContext<TContext> { }

    public abstract class ScriptableState<TContext>
        : ScriptableObject, IState<TContext>
        where TContext : IContext<TContext>
    {
        IState IState.Handle(IContext context) => Handle((TContext) context);
        void IState.Enter(IContext context) => Enter((TContext) context);
        void IState.Exit(IContext context) => Exit((TContext) context);

        public abstract IState<TContext> Handle(TContext context);
        public virtual void Enter(TContext context) { }
        public virtual void Exit(TContext context) { }
    }

    public abstract class MonoAgent<TContext>
        : MonoBehaviour, IAgent<TContext>
        where TContext : IContext<TContext>
    {
        IContext IAgent.Context => Context;
        IState IAgent.CurrentState => CurrentState;
        void IAgent.SetState(IState newState) =>
            SetState((IState<TContext>) newState);

        public abstract TContext Context { get; }
        public abstract IState<TContext> CurrentState { get; }

        public abstract void SetState(IState<TContext> newState);

        protected virtual void Start()
        {
            SetState(CurrentState);
        }

        protected virtual void Update()
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
        void IAction.Enter(IContext context) => Enter((TContext) context);
        void IAction.Exit(IContext context) => Exit((TContext) context);

        public abstract IState<TContext> Apply(TContext context);
        public virtual void Enter(TContext context) { }
        public virtual void Exit(TContext context) { }
    }
}
