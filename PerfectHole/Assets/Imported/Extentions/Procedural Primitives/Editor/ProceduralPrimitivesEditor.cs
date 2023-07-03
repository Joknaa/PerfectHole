using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralPrimitivesUtil
{
    [InitializeOnLoad]
    public static class ProceduralPrimitivesEditor
    {
        static ProceduralPrimitivesEditor()
        {
            PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdate;
        }

        static void OnPrefabInstanceUpdate(GameObject instance)
        {
            PPBase ppbase = instance.GetComponent<PPBase>();
            if (ppbase != null)
            {
                ppbase.Apply();
            }
        }

        static void InitItem(GameObject go, MenuCommand menuCommand)
        {
            if (!EditorApplication.isPlaying) go.transform.position = SceneView.lastActiveSceneView.pivot;
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        //Instance
        [MenuItem("GameObject/Procedural Primitives/Create Instance", false, 0)]
        static void CreateInstance(MenuCommand menuCommand)
        {
            GameObject context = menuCommand.context as GameObject;
            GameObject go = ProceduralPrimitives.CreateInstance();
            if (context != null)
            {
                PPBase pp = context.GetComponent<PPBase>();
                if (pp != null)
                {
                    go.GetComponent<PPInstance>().ApplySource(pp);
                    go.name = context.name + " Instance";
                }
                go.transform.SetParent(context.transform.parent, false);
                go.transform.localPosition = context.transform.localPosition;
                go.transform.localRotation = context.transform.localRotation;
                go.transform.localScale = context.transform.localScale;
                //go.transform.SetSiblingIndex(context.transform.GetSiblingIndex() + 1);
            }
            else
            {
                if (!EditorApplication.isPlaying) go.transform.position = SceneView.lastActiveSceneView.pivot;
            }
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            List<GameObject> gos = new List<GameObject>(Selection.gameObjects);
            gos.Remove(context);
            gos.Add(go);
            Selection.objects = gos.ToArray();
        }

        //Combiner
        [MenuItem("GameObject/Procedural Primitives/Create Combiner", false, 0)]
        static void CreateCombiner(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreateCombiner(), menuCommand);
        }

        //Flat
        [MenuItem("GameObject/Procedural Primitives/Flat/Plane", false, 11)]
        static void CreatePlane(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Plane), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Flat/Circle", false, 11)]
        static void CreateCircle(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Circle), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Flat/Ring", false, 11)]
        static void CreateRing(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Ring), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Flat/Triangle", false, 11)]
        static void CreateTriangle(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Triangle), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Flat/Trapezoid", false, 11)]
        static void CreateTrapezoid(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Trapezoid), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Flat/Chamfer Plane", false, 11)]
        static void CreateChamferPlane(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.ChamferPlane), menuCommand);
        }

        //Extended
        [MenuItem("GameObject/Procedural Primitives/Extended/Capsule", false, 11)]
        static void CreateCapsule(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Capsule), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Extended/Chamfer Box", false, 11)]
        static void CreateChamferBox(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.ChamferBox), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Extended/Chamfer Cylinder", false, 11)]
        static void CreateChamferCylinder(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.ChamferCylinder), menuCommand);
        }

        //[MenuItem("GameObject/Procedural Primitives/Extended/Chamfer Pyramid", false, 11)]
        //static void CreateChamferPyramid(MenuCommand menuCommand)
        //{
        //    InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.ChamferPyramid), menuCommand);
        //}

        [MenuItem("GameObject/Procedural Primitives/Extended/Arrow", false, 11)]
        static void CreateArrow(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Arrow), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Extended/Prism", false, 11)]
        static void CreatePrism(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Prism), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Extended/Double Sphere", false, 11)]
        static void CreateHemisphere(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.DoubleSphere), menuCommand);
        }

        //Standard
        [MenuItem("GameObject/Procedural Primitives/Box", false, 11)]
        static void CreateBox(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Box), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Cylinder", false, 11)]
        static void CreateCylinder(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Cylinder), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Pyramid", false, 11)]
        static void CreatePyramid(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Pyramid), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Cone", false, 11)]
        static void CreateCone(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Cone), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Rect Tube", false, 11)]
        static void CreateRectangularTube(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.RectTube), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Tube", false, 11)]
        static void CreateTube(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Tube), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Sphere", false, 11)]
        static void CreateSphere(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Sphere), menuCommand);
        }

        [MenuItem("GameObject/Procedural Primitives/Torus", false, 11)]
        static void CreateTorus(MenuCommand menuCommand)
        {
            InitItem(ProceduralPrimitives.CreatePrimitive(ProceduralPrimitiveType.Torus), menuCommand);
        }
    }
}