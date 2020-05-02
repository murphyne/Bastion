using System.Collections.Generic;

namespace FSM
{
    public interface IContext { }

    public interface IState
    {
        IState Handle(IContext context);
    }

    public interface IStateEnter
    {
        void Enter(IContext context);
    }

    public interface IStateExit
    {
        void Exit(IContext context);
    }

    public interface IAgent
    {
        IState CurrentState { get; }

        void SetState(IState newState);
    }

    public interface IStatePluggable : IState
    {
        IEnumerable<IAction> Actions { get; }
    }

    public interface IAction
    {
        IState Apply(IContext context);
    }

    public interface IActionEnter : IAction
    {
        void Enter(IContext context);
    }

    public interface IActionExit : IAction
    {
        void Exit(IContext context);
    }
}
