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
            GameStateSystem.OnGameStateChanged += UpdateUI;
            ReloadButton.onClick.AddListener(LevelSpawnSystem.Instance.ReloadLevel);
        }

        private void UpdateUI(GameState gameState) {
            
        }

        private void UpdateLevelID(int levelID) {
            LevelIDText.text = $"Level {levelID}";
        }

        public override void Dispose() {
            LevelSpawnSystem.OnLevelGenerated -= UpdateLevelID;
            GameStateSystem.OnGameStateChanged -= UpdateUI;
            ReloadButton.onClick.RemoveAllListeners();
            
            base.Dispose();
        }
    }
}