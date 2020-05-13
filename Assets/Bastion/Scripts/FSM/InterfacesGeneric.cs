using System.Collections.Generic;

namespace Bastion.FSM
{
    // https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern
    // https://ericlippert.com/2011/02/02/curiouser-and-curiouser/
    // https://zpbappi.com/curiously-recurring-template-pattern-in-csharp/
    public interface IContext<TContext> : IContext
        where TContext : IContext<TContext> { }

    public interface IState<in TContext> : IState
        where TContext : IContext<TContext>
    {
        IState<TContext> Handle(TContext context);
    }

    public interface IStateEnter<in TContext> : IStateEnter
        where TContext : IContext<TContext>
    {
        void Enter(TContext context);
    }

    public interface IStateExit<in TContext> : IStateExit
        where TContext : IContext<TContext>
    {
        void Exit(TContext context);
    }

    public interface IAgent<TContext> : IAgent
        where TContext : IContext<TContext>
    {
        new IState<TContext> CurrentState { get; }

        void SetState(IState<TContext> newState);
    }

    public interface IStatePluggable<in TContext> : IState<TContext>, IStatePluggable
        where TContext : IContext<TContext>
    {
        new IEnumerable<IAction<TContext>> Actions { get; }
    }

    public interface IAction<in TContext> : IAction
        where TContext : IContext<TContext>
    {
        IState<TContext> Apply(TContext context);
    }

    public interface IActionEnter<in TContext> : IAction<TContext>, IActionEnter
        where TContext : IContext<TContext>
    {
        void Enter(TContext context);
    }

    public interface IActionExit<in TContext> : IAction<TContext>, IActionExit
        where TContext : IContext<TContext>
    {
        void Exit(TContext context);
    }
}
