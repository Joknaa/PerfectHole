using OknaaEXTENSIONS.CustomWrappers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PerfectHole.V2.Systems {
    public class UIController : Singleton<UIController> {
        public TMP_Text LevelIDText;
        public Button ReloadButton;


        public void Init() {
            LevelSpawnSystem.OnLevelGenerated += UpdateLevelID;
            ReloadButton.onClick.AddListener(LevelSpawnSystem.Instance.ReloadLevel);
        }

        private void UpdateLevelID(int levelID) {
            LevelIDText.text = $"Level {levelID}";
        }

        public override void Dispose() {
            LevelSpawnSystem.OnLevelGenerated -= UpdateLevelID;
            ReloadButton.onClick.RemoveAllListeners();
            
            base.Dispose();
        }
    }
}