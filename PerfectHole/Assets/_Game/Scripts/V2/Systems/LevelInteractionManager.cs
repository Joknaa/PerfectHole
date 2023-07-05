using MobileInputSystem;
using OknaaEXTENSIONS;
using OknaaEXTENSIONS.CustomWrappers;
using PerfectHole.V2.Systems;
using UnityEngine;

namespace PerfectHole.V2.Systems {
    public class LevelInteractionManager : Singleton<LevelInteractionManager> {
        private int _levelID;

        public void Init() => InputController.OnTouchDown += OnTouchDown;

        private static void OnTouchDown(Vector3 position) {
            
            Ray ray = Helpers.Camera.ScreenPointToRay(position);
            Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) return;

            var hitObject = hit.transform;
            if (hitObject.gameObject.layer != LayerMask.NameToLayer("Level")) return;
            
            LevelSpawnSystem.RemoveTriangle(hitObject.parent.gameObject);
        }

    


        public override void Dispose() {
            InputController.OnTouchDown -= OnTouchDown;
            
            base.Dispose();
        }
    }
}