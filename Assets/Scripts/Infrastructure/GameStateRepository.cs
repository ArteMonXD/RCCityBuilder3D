using Domain.Models;
using UnityEngine;

namespace Infrastructure
{
    public interface IGameStateRepository
    {
        GameStateModel GetCurrentState();
        void SaveState(GameStateModel state);
        void SaveToDisk(string saveSlot);
        bool LoadFromDisk(string saveSlot);
    }

    public class GameStateRepository : IGameStateRepository
    {
        private GameStateModel _currentState;

        public GameStateRepository()
        {
            _currentState = new GameStateModel();
        }

        public GameStateModel GetCurrentState() => _currentState;

        public void SaveState(GameStateModel state) => _currentState = state;

        public void SaveToDisk(string saveSlot)
        {
            var json = JsonUtility.ToJson(_currentState);
            PlayerPrefs.SetString($"save_{saveSlot}", json);
            PlayerPrefs.Save();
        }

        public bool LoadFromDisk(string saveSlot)
        {
            var key = $"save_{saveSlot}";
            if (PlayerPrefs.HasKey(key))
            {
                var json = PlayerPrefs.GetString(key);
                _currentState = JsonUtility.FromJson<GameStateModel>(json);
                return true;
            }
            return false;
        }
    }
}
