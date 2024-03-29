﻿using UnityEngine;

namespace Arena.Creature.Actions
{
    [CreateAssetMenu(order = 101,
        fileName = "CreatureActionWander",
        menuName = "Creature/Action Wander")]
    public class CreatureActionWander : CreatureAction
    {
        public override void Enter(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Enter)}({context})");
        }

        public override void Exit(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Exit)}({context})");

            context.NavMeshAgent.ResetPath();
        }

        public override void Apply(CreatureContext context)
        {
            // Debug.Log($"{this}.{nameof(Apply)}({context})");

            var position = context.transform.position;

            const float interval = 2f;
            var currRemainder = Time.time % interval;
            var prevRemainder = currRemainder - Time.deltaTime;

            if (currRemainder >= 0f && prevRemainder < 0f)
            {
                const float radius = 5f;
                var targetOffset = Random.onUnitSphere * radius;
                var targetPosition = position + targetOffset;
                context.NavMeshAgent.SetDestination(targetPosition);
            }
        }
    }
}
