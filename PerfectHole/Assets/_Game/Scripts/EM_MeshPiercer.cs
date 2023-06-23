using System;
using System.Collections;
using System.Collections.Generic;
using EscapeMasters;
using Unity.Mathematics;
using UnityEngine;

public class EM_MeshPiercer : MonoBehaviour {
    private MeshFilter meshFilter;
    private Camera _camera;
    private InputController inputController;

    void Start() {
        _camera = Camera.main;
        meshFilter = GetComponent<MeshFilter>();
        InputController.OnInputDown += OnTouchDown;
        InputController.OnInputUp += OnTouchUp;
    }

    private void OnTouchUp(Vector2 vector2) { }

    private void OnTouchDown(Vector2 vector2) {
        Ray ray = _camera.ScreenPointToRay(vector2);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Erasable")) {
                
                DeleteTriangle(hit.triangleIndex);
            }
        }
    }


    void Update() {
        if (!Input.GetMouseButton(0)) return;
        
    }

    void DeleteTriangle(int index) {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;

        while (j < mesh.triangles.Length) {
            if (j != index * 3) {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else {
                j += 3;
            }
        }

        meshFilter.mesh.triangles = newTriangles;
        this.gameObject.AddComponent<MeshCollider>();
    }

    private void OnCollisionStay(Collision collision) {
        collision.transform.position += Mathf.Epsilon * Vector3.up;
    }
}