using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OknaaEXTENSIONS {
    
    public static class Helpers {
            
        /// <summary>
        /// Returns the main camera.
        /// </summary>
        private static Camera _mainCamera;
        public static Camera Camera => _mainCamera ? _mainCamera : _mainCamera = Camera.main;
        
        
        /// <summary>
        /// Returns a reusable WaitForSeconds object.
        /// </summary>
        private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds WaitForSeconds(float seconds) {
            if (_waitForSecondsCache.TryGetValue(seconds, out var wait)) return wait;
            _waitForSecondsCache.Add(seconds, wait = new WaitForSeconds(seconds));
            return wait;
        }
        
        
        
        /// <summary>
        /// Checks if the mouse if over a UI Element.
        /// </summary>
        private static PointerEventData _eventDataPosition;
        private static List<RaycastResult> _results;
        public static bool IsOverUI() {
            _eventDataPosition = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition
            };
            
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataPosition, _results);
            return _results.Count > 0;
        }


        
        /// <summary>
        /// Returns the world position of a canvas element.
        /// </summary>
        /// <param name="canvasElement"></param>
        /// <returns></returns>
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform canvasElement) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasElement, canvasElement.position, Camera, out var result);
            return result;
        }
        
    }
}