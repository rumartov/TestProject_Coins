using System;
using Infrastructure.Factory;

namespace Infrastructure.Services.LevelProgress
{
    public interface ILevelProgressService : IService
    {
        void OnProgressChanged();
        void CompleteLevel();
        bool IsLevelComplete();
        void Initialize(IGameFactory factory);
        Action LevelCompleted { get; set; }
    }
}