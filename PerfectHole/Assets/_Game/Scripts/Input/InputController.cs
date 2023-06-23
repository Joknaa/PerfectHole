using System;
using UnityEngine;

namespace EscapeMasters {
    public class InputController : MonoBehaviour {
        public static Action<Vector2> OnInputDown;
        public static Action<Vector2> OnInputMove;
        public static Action<Vector2> OnInputStationary;
        public static Action<Vector2> OnInputUp;
        
        private void Update() => HandleInput();

        protected virtual void HandleInput() { }
    }
}