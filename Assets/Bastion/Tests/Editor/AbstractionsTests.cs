using Bastion.FSM;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Bastion.Tests.Editor
{
    [TestFixture]
    public class AbstractionsTests
    {
        public class FooContext : Context<FooContext> { }

        [Test]
        public void Agent_Constructor_Sets_Context_Moq()
        {
            var context = Mock.Of<FooContext>();
            var agentMock = new Mock<Agent<FooContext>>(context)
            {
                CallBase = true,
            };

            Assert.AreEqual(context, agentMock.Object.Context);
        }

        [Test]
        public void Agent_Constructor_Sets_Context_NSub()
        {
            var context = Substitute.For<FooContext>();
            var agent = Substitute.ForPartsOf<Agent<FooContext>>(context);

            Assert.AreEqual(context, agent.Context);
        }

        [Test]
        public void Agent_SetState_Changes_CurrentState_Moq()
        {
            var context = Mock.Of<FooContext>();
            var state1 = Mock.Of<State<FooContext>>();
            var state2 = Mock.Of<State<FooContext>>();
            var agentMock = new Mock<Agent<FooContext>>(context)
            {
                CallBase = true,
            };

            Assert.IsNull(agentMock.Object.CurrentState);

            agentMock.Object.SetState(state1);
            Assert.AreEqual(state1, agentMock.Object.CurrentState);

            agentMock.Object.SetState(state2);
            Assert.AreEqual(state2, agentMock.Object.CurrentState);
        }

        [Test]
        public void Agent_SetState_Changes_CurrentState_NSub()
        {
            var context = Substitute.For<FooContext>();
            var state1 = Substitute.For<State<FooContext>>();
            var state2 = Substitute.For<State<FooContext>>();
            var agent = Substitute.ForPartsOf<Agent<FooContext>>(context);

            Assert.IsNull(agent.CurrentState);

            agent.SetState(state1);
            Assert.AreEqual(state1, agent.CurrentState);

            agent.SetState(state2);
            Assert.AreEqual(state2, agent.CurrentState);
        }

        [Test]
        public void Agent_SetState_Keeps_CurrentState_IfNewStateIsNull_Moq()
        {
            var context = Mock.Of<FooContext>();
            var state = Mock.Of<State<FooContext>>();
            var agentMock = new Mock<Agent<FooContext>>(context)
            {
                CallBase = true,
            };

            Assert.IsNull(agentMock.Object.CurrentState);

            agentMock.Object.SetState(state);
            Assert.AreEqual(state, agentMock.Object.CurrentState);

            agentMock.Object.SetState(null);
            Assert.AreEqual(state, agentMock.Object.CurrentState);
        }

        [Test]
        public void Agent_SetState_Keeps_CurrentState_IfNewStateIsNull_NSub()
        {
            var context = Substitute.For<FooContext>();
            var state = Substitute.For<State<FooContext>>();
            var agent = Substitute.ForPartsOf<Agent<FooContext>>(context);

            Assert.IsNull(agent.CurrentState);

            agent.SetState(state);
            Assert.AreEqual(state, agent.CurrentState);

            agent.SetState(null);
            Assert.AreEqual(state, agent.CurrentState);
        }

        [Test]
        public void Agent_SetState_Calls_State_EnterExit_InProperOrder_Moq()
        {
            var context = Mock.Of<FooContext>();
            var stateMock1 = new Mock<State<FooContext>>(MockBehavior.Strict);
            var stateMock2 = new Mock<State<FooContext>>(MockBehavior.Strict);
            var agentMock = new Mock<Agent<FooContext>>(context)
            {
                CallBase = true,
            };

            var sequence = new MockSequence();
            stateMock1.InSequence(sequence).Setup(x => x.Enter(context));
            stateMock1.InSequence(sequence).Setup(x => x.Exit(context));
            stateMock2.InSequence(sequence).Setup(x => x.Enter(context));

            agentMock.Object.SetState(stateMock1.Object);
            agentMock.Object.SetState(stateMock2.Object);
        }

        [Test]
        public void Agent_SetState_Calls_State_EnterExit_InProperOrder_NSub()
        {
            var context = Substitute.For<FooContext>();
            var state1 = Substitute.For<State<FooContext>>();
            var state2 = Substitute.For<State<FooContext>>();
            var agent = Substitute.ForPartsOf<Agent<FooContext>>(context);

            agent.SetState(state1);
            agent.SetState(state2);

            Received.InOrder(() =>
            {
                state1.Enter(context);
                state1.Exit(context);
                state2.Enter(context);
            });
        }

        [Test]
        public void Transition_Constructor_Sets_ConditionAndNextState_Moq()
        {
            var state = Mock.Of<State<FooContext>>();
            var condition = Mock.Of<Condition<FooContext>>();
            var transitionMock =
                new Mock<Transition<FooContext>>(condition, state)
                {
                    CallBase = true,
                };

            Assert.AreEqual(condition, transitionMock.Object.Condition);
            Assert.AreEqual(state, transitionMock.Object.NextState);
        }

        [Test]
        public void Transition_Constructor_Sets_ConditionAndNextState_NSub()
        {
            var state = Substitute.For<State<FooContext>>();
            var condition = Substitute.For<Condition<FooContext>>();
            var transition =
                Substitute.ForPartsOf<Transition<FooContext>>(condition, state);

            Assert.AreEqual(condition, transition.Condition);
            Assert.AreEqual(state, transition.NextState);
        }

        [Test]
        public void Transition_Check_Calls_Condition_Check_Moq()
        {
            var context = Mock.Of<FooContext>();
            var state = Mock.Of<State<FooContext>>();
            var condition = Mock.Of<Condition<FooContext>>();
            var transitionMock =
                new Mock<Transition<FooContext>>(condition, state)
                {
                    CallBase = true,
                };

            transitionMock.Object.Check(context);

            Mock.Get(condition).Verify(c => c.Check(context));
        }

        [Test]
        public void Transition_Check_Calls_Condition_Check_NSub()
        {
            var context = Substitute.For<FooContext>();
            var state = Substitute.For<State<FooContext>>();
            var condition = Substitute.For<Condition<FooContext>>();
            var transition =
                Substitute.ForPartsOf<Transition<FooContext>>(condition, state);

            transition.Check(context);

            condition.Received().Check(context);
        }

        [Test]
        public void State_Constructor_Sets_ActionsAndTransitions_Moq()
        {
            var action = Mock.Of<IAction<FooContext>>();
            var transition = Mock.Of<ITransition<FooContext>>();
            IAction<FooContext>[] actions = {action};
            ITransition<FooContext>[] transitions = {transition};
            var stateMock =
                new Mock<StatePluggable<FooContext>>(actions, transitions)
                {
                    CallBase = true,
                };

            Assert.AreEqual(actions, stateMock.Object.Actions);
            Assert.AreEqual(transitions, stateMock.Object.Transitions);
        }

        [Test]
        public void State_Constructor_Sets_ActionsAndTransitions_NSub()
        {
            var action = Substitute.For<IAction<FooContext>>();
            var transition = Substitute.For<ITransition<FooContext>>();
            IAction<FooContext>[] actions = {action};
            ITransition<FooContext>[] transitions = {transition};
            var state =
                Substitute.ForPartsOf<StatePluggable<FooContext>>(actions, transitions);

            Assert.AreEqual(actions, state.Actions);
            Assert.AreEqual(transitions, state.Transitions);
        }

        [Test]
        public void State_EnterExit_Calls_Action_EnterExit_Moq()
        {
            var context = Mock.Of<FooContext>();
            var action = Mock.Of<Action<FooContext>>();
            Action<FooContext>[] actions = {action};
            Transition<FooContext>[] transitions = {null};
            var stateMock =
                new Mock<StatePluggable<FooContext>>(actions, transitions)
                {
                    CallBase = true,
                };

            stateMock.Object.Enter(context);
            Mock.Get(action).Verify(a => a.Enter(context));

            stateMock.Object.Exit(context);
            Mock.Get(action).Verify(a => a.Exit(context));
        }

        [Test]
        public void State_EnterExit_Calls_Action_EnterExit_NSub()
        {
            var context = Substitute.For<FooContext>();
            var action = Substitute.For<Action<FooContext>>();
            Action<FooContext>[] actions = {action};
            Transition<FooContext>[] transitions = {null};
            var state =
                Substitute.ForPartsOf<StatePluggable<FooContext>>(actions, transitions);

            state.Enter(context);
            action.Received().Enter(context);

            state.Exit(context);
            action.Received().Exit(context);
        }
    }
}
