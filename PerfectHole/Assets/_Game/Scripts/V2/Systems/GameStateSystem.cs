using System;
using OknaaEXTENSIONS.CustomWrappers;

namespace PerfectHole.V2.Systems {
    public enum GameState {
        MainMenu,
        Playing,
        Paused,
        LevelFinished,
        LevelRestart,
        LevelFailed,
    }
    public class GameStateSystem : Singleton<GameStateSystem>{
        public static Action<GameState> OnGameStateChanged;
        
        public GameState GameState {
            get => GetGameState();
            set => SetGameState(value);
        }
        private GameState _gameState = GameState.MainMenu;

        private GameState GetGameState() => _gameState;

        private void SetGameState(GameState gameState) {
            if (_gameState == gameState) return;
            
            _gameState = gameState;
            OnGameStateChanged?.Invoke(_gameState);
        }
        
    }
}