using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject Player { get; set; }
        GameObject CreatePlayer(GameObject at);
        void CreateHud();
        void CreateUniqueLevelId();
        void Cleanup();
        void CreateCoins();
        void CreateResetTriggers();
    }
}