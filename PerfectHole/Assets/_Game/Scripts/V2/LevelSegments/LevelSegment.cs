using System.Collections.Generic;
using UnityEngine;

namespace PerfectHole.V2 {
    public class LevelSegment : MonoBehaviour {
        [SerializeField] private List<LevelSubSegment> _subSegments = new List<LevelSubSegment>();
         
        public void SetActive(bool isActive) {
            _subSegments.ForEach(subSegment => subSegment.SetActive(isActive));
        }
    }
}