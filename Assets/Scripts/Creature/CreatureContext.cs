using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Creature
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class CreatureContext : MonoContext
    {
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        [SerializeField] private NavMeshAgent navMeshAgent;

        public CreatureContext enemy;
    }
}
