using Data;
using UnityEngine;

namespace Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
        public void ClearProgress()
        {
            PlayerPrefs.DeleteAll();
            Progress = new PlayerProgress(Constants.MainLevel);
        }
    }
}