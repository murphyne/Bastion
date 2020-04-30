using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Creature
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class CreatureContext : MonoContext<CreatureContext>
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
            DrawFovGizmos();
            DrawRangesGizmos();
            DrawDistanceGizmos();
        }

        private void DrawFovGizmos()
        {
            var position = transform.position;
            var forward = transform.forward;

            var sideL = Quaternion.Euler(0f, -fieldOfView / 2, 0f) * forward;
            var sideR = Quaternion.Euler(0f, fieldOfView / 2, 0f) * forward;

            var attackL = position + sideL * attackRange;
            var attackR = position + sideR * attackRange;
            var searchL = position + sideL * searchRange;
            var searchR = position + sideR * searchRange;
            // var lostL = position + sideL * lostRange;
            // var lostR = position + sideR * lostRange;

            // Gizmos.color = _attackColor;
            // Gizmos.DrawLine(position, attackL);
            // Gizmos.DrawLine(position, attackR);
            Gizmos.color = _searchColor;
            Gizmos.DrawLine(attackL, searchL);
            Gizmos.DrawLine(attackR, searchR);
            // Gizmos.color = _lostColor;
            // Gizmos.DrawLine(searchL, lostL);
            // Gizmos.DrawLine(searchR, lostR);
        }

        private void DrawRangesGizmos()
        {
            var position = transform.position;
            var forward = transform.forward;

            const float interval = 15f;
            for (float angle = interval; angle <= 360f; angle += interval)
            {
                var vectorA = Quaternion.Euler(0f, angle, 0f) * forward;
                var vectorZ = Quaternion.Euler(0f, angle - interval, 0f) * forward;

                Gizmos.color = _attackColor;
                Gizmos.DrawLine(position + vectorA * attackRange, position + vectorZ * attackRange);
                Gizmos.color = _searchColor;
                Gizmos.DrawLine(position + vectorA * searchRange, position + vectorZ * searchRange);
                Gizmos.color = _lostColor;
                Gizmos.DrawLine(position + vectorA * lostRange, position + vectorZ * lostRange);
            }
        }

        private void DrawDistanceGizmos()
        {
            if (enemy == null) return;

            var position = transform.position;
            var enemyPosition = enemy.transform.position;
            var enemyDistance = enemyPosition - position;

            if (enemyDistance.sqrMagnitude < attackRange * attackRange)
                Gizmos.color = _attackColor;
            else if (enemyDistance.sqrMagnitude < searchRange * searchRange)
                Gizmos.color = _searchColor;
            else if (enemyDistance.sqrMagnitude < lostRange * lostRange)
                Gizmos.color = _lostColor;
            else
                Gizmos.color = Color.white;

            Gizmos.DrawLine(position, enemyPosition);
        }

        private void OnGUI()
        {
            PrintHealthGui();
            DrawFovGui();
            DrawRangesGui();
        }

        private void PrintHealthGui()
        {
            var worldPoint = gameObject.transform.position;
            var screenPoint = Camera.current.WorldToScreenPoint(worldPoint);
            screenPoint.y = Screen.height - screenPoint.y;

            var content = new GUIContent(health.ToString("D"));
            var style = new GUIStyle(GUI.skin.label)
            {
                normal = {textColor = Color.black},
            };
            Vector3 size = style.CalcSize(content);
            var position = new Rect(screenPoint - size / 2, size);
            GUI.Label(position, content, style);
        }

        private void DrawFovGui()
        {
            var sideL = Quaternion.Euler(0f, -fieldOfView / 2, 0f) * Vector3.forward;
            var sideR = Quaternion.Euler(0f, fieldOfView / 2, 0f) * Vector3.forward;
            var attackL = sideL * attackRange;
            var attackR = sideR * attackRange;
            var searchL = sideL * searchRange;
            var searchR = sideR * searchRange;
            // var lostL = sideL * lostRange;
            // var lostR = sideR * lostRange;

            var shader = Shader.Find("Hidden/Internal-Colored");
            var material = new Material(shader);
            material.SetPass(0);

            // https://docs.unity3d.com/ScriptReference/GL.html
            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(
                transform.position, transform.rotation, Vector3.one));

            GL.Begin(GL.LINE_STRIP);
            GL.Color(_searchColor);
            // GL.Vertex3(0, 0, 0);
            GL.Vertex3(attackL.x, attackL.y, attackL.z);
            GL.Vertex3(searchL.x, searchL.y, searchL.z);
            // GL.Vertex3(lostL.x, lostL.y, lostL.z);
            GL.End();

            GL.Begin(GL.LINE_STRIP);
            GL.Color(_searchColor);
            // GL.Vertex3(0, 0, 0);
            GL.Vertex3(attackR.x, attackR.y, attackR.z);
            GL.Vertex3(searchR.x, searchR.y, searchR.z);
            // GL.Vertex3(lostR.x, lostR.y, lostR.z);
            GL.End();

            GL.PopMatrix();
        }

        private void DrawRangesGui()
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            var material = new Material(shader);
            material.SetPass(0);

            // https://docs.unity3d.com/ScriptReference/GL.html
            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(
                transform.position, transform.rotation, Vector3.one));

            const float interval = 15f;
            for (float angle = interval; angle <= 360f; angle += interval)
            {
                var vectorA = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                var vectorZ = Quaternion.Euler(0f, angle - interval, 0f) * Vector3.forward;
                GL.Begin(GL.LINE_STRIP);
                GL.Color(_attackColor);
                var attackA = vectorA * attackRange;
                var attackZ = vectorZ * attackRange;
                GL.Vertex3(attackA.x, attackA.y, attackA.z);
                GL.Vertex3(attackZ.x, attackZ.y, attackZ.z);
                GL.End();
                GL.Begin(GL.LINE_STRIP);
                GL.Color(_searchColor);
                var searchA = vectorA * searchRange;
                var searchZ = vectorZ * searchRange;
                GL.Vertex3(searchA.x, searchA.y, searchA.z);
                GL.Vertex3(searchZ.x, searchZ.y, searchZ.z);
                GL.End();
                GL.Begin(GL.LINE_STRIP);
                GL.Color(_lostColor);
                var lostA = vectorA * lostRange;
                var lostZ = vectorZ * lostRange;
                GL.Vertex3(lostA.x, lostA.y, lostA.z);
                GL.Vertex3(lostZ.x, lostZ.y, lostZ.z);
                GL.End();
            }

            GL.PopMatrix();
        }
    }
}
