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

        public float AttackRange => attackRange;
        [SerializeField] private float attackRange = 2.0f;

        public float SearchRange => searchRange;
        [SerializeField] private float searchRange = 3.6f;

        public float LostRange => lostRange;
        [SerializeField] private float lostRange = 4.6f;

        public int Health => health;
        [SerializeField] private int health = 10;

        public int Damage => damage;
        [SerializeField] private int damage = 10;

        public CreatureContext enemy;

        public bool isAnimating;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                // Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        private readonly Color _attackColor = new Color(1f, 0.22f, 0.22f, 0.82f);
        private readonly Color _searchColor = new Color(0f, 0.55f, 0.84f, 0.82f);
        private readonly Color _lostColor = new Color(0.62f, 0.48f, 0.11f, 0.82f);

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.color = _attackColor;
            Gizmos.DrawWireSphere(position, attackRange);
            Gizmos.color = _searchColor;
            Gizmos.DrawWireSphere(position, searchRange);
            Gizmos.color = _lostColor;
            Gizmos.DrawWireSphere(position, lostRange);
        }
    }
}
