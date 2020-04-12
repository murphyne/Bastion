using UnityEngine;

namespace FSM
{
    public abstract class MonoContext
        : MonoBehaviour, IContext { }

    public abstract class ScriptableState<TContext>
        : ScriptableObject, IState<TContext>
        where TContext : MonoContext
    {
        public abstract IState<TContext> Handle(TContext context);
    }

    public abstract class MonoAgent<TContext>
        : MonoBehaviour, IAgent<TContext>
        where TContext : MonoContext
    {
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
        where TContext : MonoContext
    {
        public abstract IState<TContext> Apply(TContext context);
    }
}
