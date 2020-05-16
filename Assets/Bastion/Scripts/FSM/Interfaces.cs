using System.Collections.Generic;

namespace Bastion.FSM
{
    public interface IContext { }

    public interface IState
    {
        IState Handle(IContext context);
        void Enter(IContext context);
        void Exit(IContext context);
    }

    public interface IAgent
    {
        IContext Context { get; }
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
        void Enter(IContext context);
        void Exit(IContext context);
    }
}
