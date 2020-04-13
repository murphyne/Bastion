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

        public LayerMask SearchLayer => searchLayer;
        [SerializeField] private LayerMask searchLayer;

        public float FieldOfView => fieldOfView;
        [SerializeField] private float fieldOfView = 60f;

        public float SearchRange => searchRange;
        [SerializeField] private float searchRange = 3.6f;

        public CreatureContext enemy;

        private readonly Color _searchColor = new Color(0f, 0.55f, 0.84f, 0.82f);

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.color = _searchColor;
            Gizmos.DrawWireSphere(position, searchRange);
        }
    }
}
