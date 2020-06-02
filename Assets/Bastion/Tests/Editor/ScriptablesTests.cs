using Bastion.FSM;
using Moq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Bastion.Tests.Editor
{
    [TestFixture]
    public class ScriptablesTests
    {
        public class FooContext : MonoContext<FooContext>
        {
            public int foo = 42;
        }

        public class FooAgent : ProxyAgent<FooContext> { }

        private static T AsComponent<T>() where T : Component
        {
            var gameObject = new GameObject();
            var component = gameObject.AddComponent<T>();
            return component;
        }

        [Test]
        public void Context_IsNull_With_New()
        {
            var context = new FooContext();

            Assert.AreEqual(42, context.foo);
            Assert.IsNull(context);
        }

        [Test]
        public void Context_IsNull_With_Moq()
        {
            var context = Mock.Of<FooContext>();

            Assert.AreEqual(42, context.foo);
            Assert.IsNull(context);
        }

        [Test]
        public void Context_IsNull_With_NSub()
        {
            var context = Substitute.For<FooContext>();

            Assert.AreEqual(42, context.foo);
            Assert.IsNull(context);
        }

        [Test]
        public void Context_IsNotNull_With_AddComponent()
        {
            var context = AsComponent<FooContext>();

            Assert.AreEqual(42, context.foo);
            Assert.IsNotNull(context);
        }

        [Test]
        public void Context_Fields_Are_Accessible_Moq()
        {
            var context = Mock.Of<FooContext>();

            Assert.AreEqual(42, context.foo);
        }

        [Test]
        public void Context_Fields_Are_Accessible_NSub()
        {
            var context = Substitute.ForPartsOf<FooContext>();

            Assert.AreEqual(42, context.foo);
        }

        [Test]
        public void ProxyAgent_Has_Null_Agent_ByDefault_Moq()
        {
            var proxyAgentMock = new Mock<ProxyAgent<FooContext>>()
            {
                CallBase = true,
            };

            Assert.IsNull(proxyAgentMock.Object.Agent);
        }

        [Test]
        public void ProxyAgent_Has_Null_Agent_ByDefault_NSub()
        {
            var proxyAgent = Substitute.For<ProxyAgent<FooContext>>();

            Assert.IsNull(proxyAgent.Agent);
        }

        [Test]
        public void ProxyAgent_Provides_Agent_Moq()
        {
            var context = Mock.Of<FooContext>();
            var agentMock = new Mock<Agent<FooContext>>(context)
            {
                CallBase = true,
            };
            var proxyAgentMock = new Mock<ProxyAgent<FooContext>>()
            {
                CallBase = true,
            };

            proxyAgentMock.Object.Agent = agentMock.Object;

            Assert.IsNotNull(proxyAgentMock.Object.Agent);
            Assert.AreEqual(agentMock.Object, proxyAgentMock.Object.Agent);
        }

        [Test]
        public void ProxyAgent_Provides_Agent_NSub()
        {
            var context = Substitute.For<FooContext>();
            var agent = Substitute.For<Agent<FooContext>>(context);
            var proxyAgent = Substitute.For<ProxyAgent<FooContext>>();

            proxyAgent.Agent = agent;

            Assert.IsNotNull(proxyAgent.Agent);
            Assert.AreEqual(agent, proxyAgent.Agent);
        }

        [Test]
        public void ProxyAgent_Provides_Context_Moq()
        {
            var context = AsComponent<FooContext>();
            var agentMock = new Mock<Agent<FooContext>>(context)
            {
                CallBase = true,
            };
            var proxyAgentMock = new Mock<ProxyAgent<FooContext>>()
            {
                CallBase = true,
            };

            proxyAgentMock.Object.Agent = agentMock.Object;

            Assert.IsNotNull(proxyAgentMock.Object.Context);
            Assert.AreEqual(context, proxyAgentMock.Object.Context);
        }

        [Test]
        public void ProxyAgent_Provides_Context_NSub()
        {
            var context = AsComponent<FooContext>();
            var agent = Substitute.ForPartsOf<Agent<FooContext>>(context);
            var proxyAgent = Substitute.ForPartsOf<ProxyAgent<FooContext>>();

            proxyAgent.Agent = agent;

            Assert.IsNotNull(proxyAgent.Context);
            Assert.AreEqual(context, proxyAgent.Context);
        }

        [Test]
        public void ProxyAgent_Provides_CurrentState_Moq()
        {
            var context = AsComponent<FooContext>();
            var state = Mock.Of<State<FooContext>>();
            var agentMock = new Mock<Agent<FooContext>>(context)
            {
                CallBase = true,
            };
            var proxyAgentMock = new Mock<ProxyAgent<FooContext>>()
            {
                CallBase = true,
            };

            agentMock.Object.SetState(state);
            proxyAgentMock.Object.Agent = agentMock.Object;

            Assert.IsNotNull(proxyAgentMock.Object.CurrentState);
            Assert.AreEqual(state, proxyAgentMock.Object.CurrentState);
        }

        [Test]
        public void ProxyAgent_Provides_CurrentState_NSub()
        {
            var context = AsComponent<FooContext>();
            var state = Substitute.For<State<FooContext>>();
            var agent = Substitute.ForPartsOf<Agent<FooContext>>(context);
            var proxyAgent = Substitute.ForPartsOf<ProxyAgent<FooContext>>();

            agent.SetState(state);
            proxyAgent.Agent = agent;

            Assert.IsNotNull(proxyAgent.CurrentState);
            Assert.AreEqual(state, proxyAgent.CurrentState);
        }
    }
}
