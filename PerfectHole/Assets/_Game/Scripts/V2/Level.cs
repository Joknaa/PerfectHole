using MobileInputSystem;
using OknaaEXTENSIONS;
using OknaaEXTENSIONS.CustomWrappers;
using UnityEngine;

namespace _Game.Scripts.V2 {
    public class Level : Singleton<Level> {
        private int _levelID;

        private void Start() => InputController.OnTouchDown += OnTouchDown;

        private void OnTouchDown(Vector3 position) {
            
            Ray ray = Helpers.Camera.ScreenPointToRay(position);
            Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) return;

            var hitObject = hit.transform.gameObject;

            if (hitObject.layer != LayerMask.NameToLayer("Level")) return;
            RemoveTriangle(hitObject.transform.parent.gameObject);

        }

        public static void RemoveTriangle(GameObject triangle) => Destroy(triangle);


        public override void Dispose() => InputController.OnTouchDown -= OnTouchDown;
    }
}