using OknaaEXTENSIONS.CustomWrappers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.V2 {
    public class UIController : Singleton<UIController> {
        public TMP_Text LevelIDText;
        public Button ReloadButton;


        private void Start() {
            LevelSpawnSystem.OnLevelGenerated += UpdateLevelID;
            ReloadButton.onClick.AddListener(LevelSpawnSystem.Instance.ReloadLevel);
        }

        private void UpdateLevelID(int levelID) {
            LevelIDText.text = $"Level {levelID}";
        }

        public override void Dispose() {
            LevelSpawnSystem.OnLevelGenerated -= UpdateLevelID;
            ReloadButton.onClick.RemoveAllListeners();
        }
    }
}