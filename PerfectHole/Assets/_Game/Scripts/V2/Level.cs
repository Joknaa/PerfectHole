using System;
using MobileInputSystem;
using UnityEngine;

namespace _Game.Scripts.V2 {
    public class Level : MonoBehaviour {
        private Camera _camera;

        void Start() {
            _camera = Camera.main;
            InputController.OnTouchDown += OnTouchDown;
        }

        private void OnTouchDown(Vector3 position) {
            print("Touch down");
            Ray ray = _camera.ScreenPointToRay(position);
            Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) return;

            var hitObject = hit.transform.gameObject;
            Debug.Log("Hit: " + hitObject.name);

            if (hitObject.layer != LayerMask.NameToLayer("Level")) return;
            
            Destroy(hitObject);
            print("Hit triangle : " + hitObject.name);
        }

        private void OnDestroy() {
            InputController.OnTouchDown -= OnTouchDown;
        }
    }
}