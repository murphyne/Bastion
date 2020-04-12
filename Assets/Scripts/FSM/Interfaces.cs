using System.Collections.Generic;

namespace FSM
{
    public interface IContext<TContext>
        where TContext : IContext<TContext> { }

    public interface IContext : IContext<IContext> { }

    public interface IState<in TContext>
        where TContext : IContext
    {
        IState<TContext> Handle(TContext context);
    }

    public interface IStateEnter<in TContext>
        where TContext : IContext
    {
        void Enter(TContext context);
    }

    public interface IStateExit<in TContext>
        where TContext : IContext
    {
        void Exit(TContext context);
    }

    public interface IAgent<TContext>
        where TContext : IContext
    {
        IState<TContext> CurrentState { get; }

        void SetState(IState<TContext> newState);
    }

    public interface IStatePluggable<in TContext> : IState<TContext>
        where TContext : IContext
    {
        IEnumerable<IAction<TContext>> Actions { get; }
    }

    public interface IAction<in TContext>
        where TContext : IContext
    {
        IState<TContext> Apply(TContext context);
    }

    public interface IActionEnter<in TContext> : IAction<TContext>
        where TContext : IContext
    {
        void Enter(TContext context);
    }

    public interface IActionExit<in TContext> : IAction<TContext>
        where TContext : IContext
    {
        void Exit(TContext context);
    }
}
