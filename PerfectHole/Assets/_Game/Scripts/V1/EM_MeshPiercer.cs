using MobileInputSystem;
using UnityEngine;

public class EM_MeshPiercer : MonoBehaviour {
    private MeshFilter meshFilter;
    private Camera _camera;

    void Start() {
        _camera = Camera.main;
        meshFilter = GetComponent<MeshFilter>();
        InputController.OnTouchDown += OnTouchDown;
        InputController.OnTouchUp += OnTouchUp;
    }

    private void OnTouchUp( ) { }

    private void OnTouchDown(Vector3 position) {
        print("Touch down");
        Ray ray = _camera.ScreenPointToRay(position);

        Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);

        if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) return;
        Debug.Log("Hit: " + hit.transform.name);
        
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Erasable")) {
            DeleteTriangle(hit.triangleIndex);
            print("Hit triangle index: " + hit.triangleIndex);
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