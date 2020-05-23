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
        IEnumerable<ITransition> Transitions { get; }
    }

    public interface IAction
    {
        void Apply(IContext context);
        void Enter(IContext context);
        void Exit(IContext context);
    }

    public interface ICondition
    {
        bool Check(IContext context);
    }

    public interface ITransition
    {
        ICondition Condition { get; }
        IState NextState { get; }
        IState Check(IContext context);
    }
}
