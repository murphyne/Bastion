using Creature;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class AgentStateNameDrawer
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        private static void DrawAgentStateName(CreatureAgent agent, GizmoType gizmoType)
        {
            var agentState = agent.CurrentState as ScriptableObject;
            var agentStateName = $" {agentState.name.Trim()}";
            var guiContent = new GUIContent(agentStateName);
            var guiStyle = new GUIStyle(GUI.skin.label);

            var agentPosition = agent.transform.position;
            var gizmoPosition = agentPosition + Vector3.up * 0.7f;

            DrawLabel(gizmoPosition, guiContent, guiStyle);
        }

        private static void DrawLabel(Vector3 position, GUIContent content, GUIStyle style)
        {
            var camera = Camera.current;

            Vector3 point = camera.WorldToScreenPoint(position);
            Vector3 size = style.CalcSize(content);

            System.Func<Vector3, Vector3> f = camera.ScreenToWorldPoint;
            // https://en.wikipedia.org/wiki/Quadrant_(plane_geometry)
            var quadrant1 = f(point + new Vector3(size.x / 2, size.y / 2));
            var quadrant2 = f(point + new Vector3(-size.x / 2, size.y / 2));
            var quadrant3 = f(point + new Vector3(-size.x / 2, -size.y / 2));
            var quadrant4 = f(point + new Vector3(size.x / 2, -size.y / 2));

            Vector3[] vertices = {quadrant1, quadrant2, quadrant3, quadrant4};
            Handles.DrawSolidRectangleWithOutline(vertices, Color.black, Color.black);
            Handles.Label(quadrant2, content, style);
        }
    }
}
