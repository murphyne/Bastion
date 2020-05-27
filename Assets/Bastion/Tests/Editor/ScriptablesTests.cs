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
    }
}
