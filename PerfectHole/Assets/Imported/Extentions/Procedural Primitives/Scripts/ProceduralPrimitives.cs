using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralPrimitivesUtil
{
    public enum ProceduralPrimitiveType
    {
        Plane, Circle, Ring, Triangle, Trapezoid, ChamferPlane, 
        Capsule, Arrow, Prism, ChamferBox, ChamferCylinder, /*ChamferPyramid, */DoubleSphere,
        Box, Sphere, Cylinder, Cone, Tube, RectTube, Torus, Pyramid
    }

    public static class ProceduralPrimitives
    {
        static GameObject CreateBase()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.GetComponent<MeshFilter>().mesh = null;
#if UNITY_EDITOR
            GameObject.DestroyImmediate(go.GetComponent<Collider>());
#else
            GameObject.Destroy(go.GetComponent<Collider>());
#endif
            return go;
        }

        public static GameObject CreateInstance(PPBase source = null)
        {
            GameObject go = CreateBase();
            go.name = "Procdural Primitive Instance";
            go.AddComponent<PPInstance>().ApplySource(source);
            return go;
        }

        public static GameObject CreateCombiner()
        {
            GameObject go = CreateBase();
            go.name = "Procdural Primitive Combiner";
            go.AddComponent<PPCombiner>();
            return go;
        }

        public static GameObject CreatePrimitive(ProceduralPrimitiveType type)
        {
            GameObject go = CreateBase();
            go.name = type.ToString();
            switch(type)
            {
                case ProceduralPrimitiveType.Plane:
                    go.AddComponent<Plane>();
                    break;
                case ProceduralPrimitiveType.Circle:
                    go.AddComponent<Circle>();
                    break;
                case ProceduralPrimitiveType.Ring:
                    go.AddComponent<Ring>();
                    break;
                case ProceduralPrimitiveType.Triangle:
                    go.AddComponent<Triangle>();
                    break;
                case ProceduralPrimitiveType.Trapezoid:
                    go.AddComponent<Trapezoid>();
                    break;
                case ProceduralPrimitiveType.ChamferPlane:
                    go.AddComponent<ChamferPlane>();
                    break;
                case ProceduralPrimitiveType.Capsule:
                    go.AddComponent<Capsule>();
                    break;
                case ProceduralPrimitiveType.Prism:
                    go.AddComponent<Prism>();
                    break;
                case ProceduralPrimitiveType.ChamferBox:
                    go.AddComponent<ChamferBox>();
                    break;
                case ProceduralPrimitiveType.ChamferCylinder:
                    go.AddComponent<ChamferCylinder>();
                    break;
                //case ProceduralPrimitiveType.ChamferPyramid:
                //    go.AddComponent<ChamferPyramid>();
                //    break;
                case ProceduralPrimitiveType.DoubleSphere:
                    go.AddComponent<DoubleSphere>();
                    break;
                case ProceduralPrimitiveType.Arrow:
                    go.AddComponent<Arrow>();
                    break;
                case ProceduralPrimitiveType.Box:
                    go.AddComponent<Box>();
                    break;
                case ProceduralPrimitiveType.Sphere:
                    go.AddComponent<Sphere>();
                    break;
                case ProceduralPrimitiveType.Cylinder:
                    go.AddComponent<Cylinder>();
                    break;
                case ProceduralPrimitiveType.Cone:
                    go.AddComponent<Cone>();
                    break;
                case ProceduralPrimitiveType.Tube:
                    go.AddComponent<Tube>();
                    break;
                case ProceduralPrimitiveType.RectTube:
                    go.AddComponent<RectTube>();
                    break;
                case ProceduralPrimitiveType.Torus:
                    go.AddComponent<Torus>();
                    break;
                case ProceduralPrimitiveType.Pyramid:
                    go.AddComponent<Pyramid>();
                    break;
                default: break;
            }
            return go;
        }
    }
}