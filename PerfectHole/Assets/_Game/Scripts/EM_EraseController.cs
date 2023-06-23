using UnityEngine;

namespace EscapeMasters {
    public class EM_EraseController : MonoBehaviour {
        private Camera _camera;

        private void Start() {
            _camera = Camera.main;
            InputController.OnInputDown += OnTouchDown;
            InputController.OnInputUp += OnTouchUp;
        }

        private void OnTouchUp(Vector2 vector2) { }

        private void OnTouchDown(Vector2 vector2) {
            var ray = _camera.ScreenPointToRay(vector2);

            Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);

            if (Physics.Raycast(ray, out var hit, 100))
                Debug.Log("Hit: " + hit.transform.name);
            else
                Debug.Log(hit.distance);
        }
    }
}