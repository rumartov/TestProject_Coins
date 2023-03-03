using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Infrastructure.Services.LevelProgress;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.ResetLevel;
using Infrastructure.States;
using Logic;
using Logic.KeyboardControl;
using Loot;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        private readonly GameStateMachine _stateMachine;
        private readonly IResetLevelService _resetLevelService;
        private readonly ILevelProgressService _levelProgressService;

        public GameObject Player { get; set; }
        public GameFactory(
            IAssetProvider assets,
            IPersistentProgressService progressService,
            IResetLevelService resetLevelService,
            ILevelProgressService levelProgressService)
        {
            _assets = assets;
            _progressService = progressService;
            _resetLevelService = resetLevelService;
            _levelProgressService = levelProgressService;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public GameObject CreatePlayer(GameObject at)
        {
            GameObject player = InstantiateRegistered(AssetPath.PlayerPath, at.transform.position);

            player.GetComponent<KeyboardControl>().Construct(_resetLevelService);

            Player = player;
            
            return player;
        }

        public void CreateCoins()
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag(Constants.CoinSpawnPosition);

            foreach (GameObject spawn in spawns)
            {
                GameObject coin = InstantiateRegistered(AssetPath.Coin, spawn.transform.position);
                
                LootPiece lootPiece = coin.GetComponent<LootPiece>();
                lootPiece.Construct(_progressService.Progress.WorldData);
                Data.Loot lootValue = new Data.Loot();
                lootValue.Value = 1;
                lootPiece.Initialize(lootValue);
            }
        }
        
        public void CreateResetTriggers()
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag(Constants.ResetTriggerSpawnPosition);

            foreach (GameObject spawn in spawns)
            {
                GameObject resetTrigger = InstantiateRegistered(AssetPath.RedCoin, spawn.transform.position);
                resetTrigger.GetComponent<LevelResetTrigger>().Construct(_resetLevelService);
            }
        }
        
        public void CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);
            hud.GetComponentInChildren<WinScreen>()
                .Construct(_levelProgressService);
        }

        public void CreateUniqueLevelId()
        {
            GameObject uniqueId = InstantiateRegistered(AssetPath.UniqueLevelId);
            uniqueId.GetComponentInChildren<UniqueId>()
                .GenerateId();
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assets.Instantiate(prefabPath, at);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assets.Instantiate(prefabPath);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}