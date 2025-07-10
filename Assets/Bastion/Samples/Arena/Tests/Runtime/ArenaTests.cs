using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arena.Creature;
using Bastion.FSM;
using HarmonyLib;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace Bastion.Samples.Arena.Tests
{
    [TestFixture]
    public class ArenaTests
    {
        [UnityTest]
        public IEnumerator SetState_Called_Just_Enough_Times()
        {
            Debug.Log("Before Harmony");

            var harmony = new Harmony("com.example.patch");
            var processor = harmony.CreateClassProcessor(typeof(PatchCreatureAgent));
            processor.Patch();

            Assert.IsNotEmpty(harmony.GetPatchedMethods());

            var callCount = 0;
            PatchCreatureAgent.Event += (newState) => callCount++;

            Debug.Log("Before Plane");

            var planeGameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);

            Debug.Log("Before NavMeshData");

            bool edgeFound;
            edgeFound = NavMesh.FindClosestEdge(Vector3.zero, out _, NavMesh.AllAreas);
            Assert.AreEqual(false, edgeFound);
            var navMeshDataInstance = NavMesh.AddNavMeshData(BuildNavMeshData());
            edgeFound = NavMesh.FindClosestEdge(Vector3.zero, out _, NavMesh.AllAreas);
            Assert.AreEqual(true, edgeFound);

            Debug.Log("Before Camera");

            var cameraGameObject = new GameObject();
            cameraGameObject.name = "Camera";
            var camera = cameraGameObject.AddComponent<Camera>();
            camera.transform.position = new Vector3(0, 4, -4);
            camera.transform.Rotate(Vector3.right, 60);

            Debug.Log("Before Light");

            var lightGameObject = new GameObject();
            lightGameObject.name = "Light";
            var light = lightGameObject.AddComponent<Light>();
            light.transform.position = new Vector3(-2, 2, -2);
            light.transform.Rotate(Vector3.up, 45, Space.Self);
            light.transform.Rotate(Vector3.right, 60, Space.Self);
            light.type = LightType.Directional;

            Debug.Log("Before yield return null");

            yield return null;

            Debug.Log("Before Alice");

            var aGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            aGameObject.name = "Alice";
            var aContext = aGameObject.AddComponent<CreatureContext>();
            var aAgent = aGameObject.AddComponent<CreatureAgent>();
            aGameObject.transform.position = new Vector3(0, 0.5f, -3.5f);

            Debug.Log("Before Alice NavMeshAgent");

            var aNavMeshAgent = aGameObject.GetComponent<NavMeshAgent>();
            Assert.IsNull(aContext.NavMeshAgent);
            GetField(aContext, "navMeshAgent").SetValue(aContext, aNavMeshAgent);
            Assert.AreEqual(aNavMeshAgent, aContext.NavMeshAgent);

            Debug.Log("Before Alice Layer");

            var aLayer = LayerMask.NameToLayer("Creature");

            Assert.AreNotEqual(aLayer, aGameObject.layer);
            aGameObject.layer = aLayer;
            Assert.AreEqual(aLayer, aGameObject.layer);

            LayerMask layerMask = LayerMask.GetMask("Creature");

            Assert.AreNotEqual(layerMask, aContext.SearchLayer);
            GetField(aContext, "searchLayer").SetValue(aContext, layerMask);
            Assert.AreEqual(layerMask, aContext.SearchLayer);

            Debug.Log("Before Alice CurrentState");

            var guids = AssetDatabase.FindAssets("StateWander t:ScriptableObject");
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var states = paths.Select(AssetDatabase.LoadAssetAtPath<CreatureState>);
            var state = states.Single();
            Assert.IsNull(aAgent.CurrentState);
            GetField(aAgent, "currentState").SetValue(aAgent, state);
            Assert.AreEqual(state, aAgent.CurrentState);

            Debug.Log("Before Bob");

            var bGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bGameObject.name = "Bob";
            var bContext = bGameObject.AddComponent<CreatureContext>();

            Debug.Log("Before Alice Enemy");

            aContext.enemy = bContext;

            Debug.Log("Before Bob NavMeshAgent");

            var bNavMeshAgent = bGameObject.GetComponent<NavMeshAgent>();
            Assert.IsNull(bContext.NavMeshAgent);
            GetField(bContext, "navMeshAgent").SetValue(bContext, bNavMeshAgent);
            Assert.AreEqual(bNavMeshAgent, bContext.NavMeshAgent);

            Debug.Log("Before Bob Layer");

            var bLayer = LayerMask.NameToLayer("Creature");

            Assert.AreNotEqual(bLayer, bGameObject.layer);
            bGameObject.layer = bLayer;
            Assert.AreEqual(bLayer, bGameObject.layer);

            Debug.Log("Before Alice Context");

            Assert.IsNull(aAgent.Context);
            yield return null;
            Assert.AreEqual(aContext, aAgent.Context);

            Debug.Log("After Everything");

            yield return new WaitForSeconds(2.5f);

            Assert.AreEqual(4, callCount);

            Debug.Log("Before Cleanup");

            GameObject.Destroy(aGameObject);
            GameObject.Destroy(bGameObject);
            GameObject.Destroy(lightGameObject);
            GameObject.Destroy(cameraGameObject);
            NavMesh.RemoveNavMeshData(navMeshDataInstance);
            GameObject.Destroy(planeGameObject);
            harmony.UnpatchAll();
        }

        private static FieldInfo GetPropertyField(object obj, string propertyName)
        {
            var fieldName = GetPropertyFieldName(propertyName);
            return GetField(obj, fieldName);
        }

        private static FieldInfo GetField(object obj, string fieldName)
        {
            var type = obj.GetType();
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var field = type.GetField(fieldName, flags);
            return field;
        }

        private static string GetPropertyFieldName(string propertyName)
        {
            var fieldName = $"<{propertyName}>k__BackingField";
            return fieldName;
        }

        private static NavMeshData BuildNavMeshData()
        {
            var settings = NavMesh.GetSettingsByIndex(0);
            var bounds = new Bounds(Vector3.zero, 100 * Vector3.one);
            var markups = new List<NavMeshBuildMarkup>();
            var results = new List<NavMeshBuildSource>();
            NavMeshBuilder.CollectSources(
                new Bounds(),
                255,
                NavMeshCollectGeometry.RenderMeshes,
                0,
                markups,
                results);
            var data = NavMeshBuilder.BuildNavMeshData(
                settings,
                results,
                bounds,
                Vector3.zero,
                Quaternion.identity);
            return data;
        }

        [HarmonyPatch(typeof(CreatureAgent), nameof(CreatureAgent.SetState))]
        [HarmonyPatch(new[] {typeof(IState<CreatureContext>)})]
        private class PatchCreatureAgent
        {
            public static event System.Action<IState<CreatureContext>> Event;

            [HarmonyCleanup]
            private static void Cleanup()
            {
                Event= null;
            }

            [HarmonyPatch(new[] {typeof(IState<CreatureContext>)})]
            private static void Prefix(IState<CreatureContext> newState)
            {
                Event?.Invoke(newState);
            }
        }
    }
}
