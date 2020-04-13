using System.Collections;
using FSM;
using UnityEngine;

namespace Creature.Actions
{
    [CreateAssetMenu(order = 103,
        fileName = "CreatureActionAttack",
        menuName = "Creature/Action Attack")]
    public class CreatureActionAttack : CreatureAction,
        IActionEnter<CreatureContext>, IActionExit<CreatureContext>
    {
        [SerializeField] private CreatureState nextState;

        public void Enter(CreatureContext context)
        {
            context.StartCoroutine(Animate(context));
        }

        public void Exit(CreatureContext context) { }

        public override IState<CreatureContext> Apply(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Apply)}({context})");

            if (context.isAnimating)
            {
                return null;
            }

            return nextState;
        }

        private IEnumerator Animate(CreatureContext context)
        {
            context.isAnimating = true;
            context.NavMeshAgent.ResetPath();
            context.NavMeshAgent.updateRotation = false;

            const float duration = 0.6f;
            var initialAngle = context.transform.localRotation.eulerAngles.y;
            var targetAngle = initialAngle + 360f;

            for (var time = 0f; time < duration; time += Time.deltaTime)
            {
                var progress = time / duration;
                var blendedProgress = -(Mathf.Cos(Mathf.PI * progress) - 1) / 2;

                var newAngle = Mathf.Lerp(initialAngle, targetAngle, blendedProgress);
                var currentAngle = context.transform.localRotation.eulerAngles.y;
                var deltaAngle = newAngle - currentAngle;

                context.transform.Rotate(Vector3.up, deltaAngle);
                yield return null;
            }

            context.isAnimating = false;
            context.NavMeshAgent.updateRotation = true;
        }
    }
}
