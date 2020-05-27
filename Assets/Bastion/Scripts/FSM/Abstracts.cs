using System.Collections.Generic;

namespace Bastion.FSM
{
    public abstract class Context<TContext> : IContext<TContext>
        where TContext : IContext<TContext> { }

    public abstract class State<TContext> : IState<TContext>
        where TContext : IContext<TContext>
    {
        IState IState.Handle(IContext context) => Handle((TContext) context);
        void IState.Enter(IContext context) => Enter((TContext) context);
        void IState.Exit(IContext context) => Exit((TContext) context);

        public abstract IState<TContext> Handle(TContext context);
        public virtual void Enter(TContext context) { }
        public virtual void Exit(TContext context) { }
    }

    public abstract class Agent<TContext> : IAgent<TContext>
        where TContext : IContext<TContext>
    {
        IContext IAgent.Context => _context;
        IState IAgent.CurrentState => _currentState;
        void IAgent.SetState(IState newState) =>
            SetState((IState<TContext>) newState);

        public TContext Context => _context;
        private TContext _context;

        public IState<TContext> CurrentState => _currentState;
        private IState<TContext> _currentState;

        protected Agent(TContext context)
        {
            _context = context;
        }

        public virtual void SetState(IState<TContext> newState)
        {
            if (newState == null) return;

            _currentState?.Exit(_context);
            _currentState = newState;
            _currentState?.Enter(_context);
        }
    }

    public abstract class Action<TContext> : IAction<TContext>
        where TContext : IContext<TContext>
    {
        void IAction.Apply(IContext context) => Apply((TContext) context);
        void IAction.Enter(IContext context) => Enter((TContext) context);
        void IAction.Exit(IContext context) => Exit((TContext) context);

        public virtual void Apply(TContext context) { }
        public virtual void Enter(TContext context) { }
        public virtual void Exit(TContext context) { }
    }

    public abstract class Condition<TContext> : ICondition<TContext>
        where TContext : IContext<TContext>
    {
        bool ICondition.Check(IContext context) => Check((TContext) context);

        public abstract bool Check(TContext context);
    }

    public abstract class Transition<TContext> : ITransition<TContext>
        where TContext : IContext<TContext>
    {
        ICondition ITransition.Condition => _condition;
        IState ITransition.NextState => _nextState;
        IState ITransition.Check(IContext context) => Check((TContext) context);

        public ICondition<TContext> Condition => _condition;
        private readonly ICondition<TContext> _condition;

        public IState<TContext> NextState => _nextState;
        private readonly IState<TContext> _nextState;

        protected Transition(ICondition<TContext> condition,
            IState<TContext> nextState)
        {
            _condition = condition;
            _nextState = nextState;
        }

        protected virtual bool Execute(TContext context)
        {
            return Condition != null && Condition.Check(context);
        }

        public virtual IState<TContext> Check(TContext context)
        {
            return Execute(context) ? NextState : null;
        }
    }

    public abstract class StatePluggable<TContext> : IStatePluggable<TContext>
        where TContext : IContext<TContext>
    {
        IEnumerable<IAction> IStatePluggable.Actions => _actions;
        IEnumerable<ITransition> IStatePluggable.Transitions => _transitions;
        IState IState.Handle(IContext context) => Handle((TContext) context);
        void IState.Enter(IContext context) => Enter((TContext) context);
        void IState.Exit(IContext context) => Exit((TContext) context);

        public IEnumerable<IAction<TContext>> Actions => _actions;
        private readonly IEnumerable<IAction<TContext>> _actions;

        public IEnumerable<ITransition<TContext>> Transitions => _transitions;
        private readonly IEnumerable<ITransition<TContext>> _transitions;

        protected StatePluggable(IEnumerable<IAction<TContext>> actions,
            IEnumerable<ITransition<TContext>> transitions)
        {
            _actions = actions;
            _transitions = transitions;
        }

        public abstract IState<TContext> Handle(TContext context);

        public virtual void Enter(TContext context)
        {
            foreach (var action in Actions)
            {
                action?.Enter(context);
            }
        }

        public virtual void Exit(TContext context)
        {
            foreach (var action in Actions)
            {
                action?.Exit(context);
            }
        }
    }
}
