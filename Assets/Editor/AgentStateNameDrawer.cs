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
            var agentStateName = agentState == null
                ? " [null]"
                : $" {agentState.name.Trim()}";
            var guiContent = new GUIContent(agentStateName);
            var guiStyle = new GUIStyle(GUI.skin.label);

            var hashCode = ComputeHashFromString(agentStateName);
            var backgroundColor = ComputeColorFromHash(hashCode);
            var textColor = ComputeContrastColor(backgroundColor);
            GUI.backgroundColor = backgroundColor;
            guiStyle.normal.textColor = textColor;

            var agentPosition = agent.transform.position;
            var gizmoPosition = agentPosition + Vector3.up * 0.7f;

            DrawLabel(gizmoPosition, guiContent, guiStyle);
        }

        private static int ComputeHashFromString(string name)
        {
            var hash = Hash128.Compute(name);
            var hashCode = hash.GetHashCode();
            return hashCode;
        }

        private static Color ComputeColorFromHash(int hashCode)
        {
            var r = (float) (hashCode >> 24 & 0xff) / 256;
            var g = (float) (hashCode >> 16 & 0xff) / 256;
            var b = (float) (hashCode >> 8 & 0xff) / 256;

            return new Color(r, g, b);
        }

        private static Color ComputeContrastColor(Color color)
        {
            // Calculate relative luminance of the color.
            var threshold =
                color.r * 2126 + color.g * 7152 + color.b * 722 > 5000;
            return threshold ? Color.black : Color.white;
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
            Handles.DrawSolidRectangleWithOutline(vertices,
                GUI.backgroundColor, style.normal.textColor);

            Handles.Label(quadrant2, content, style);
        }
    }
}
