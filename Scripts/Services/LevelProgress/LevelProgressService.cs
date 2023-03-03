using System;
using System.Collections;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.ResetLevel;
using Logic.KeyboardControl;
using UnityEngine;

namespace Infrastructure.Services.LevelProgress
{
    public class LevelProgressService : ILevelProgressService
    {
        private const int LootToCompleteLevel = Constants.AmountOfCoinsToCompleteLevel;

        private readonly IPersistentProgressService _progressService;
        private readonly IResetLevelService _resetLevelService;
        private readonly ICoroutineRunner _coroutineRunner;
        private IGameFactory _factory;
        public Action LevelCompleted { get; set; }

        public LevelProgressService(IPersistentProgressService progressService, IResetLevelService resetLevelService,
            ICoroutineRunner coroutineRunner, IGameFactory factory)
        {
            _progressService = progressService;
            _resetLevelService = resetLevelService;
            _coroutineRunner = coroutineRunner;
        }

        public void Initialize(IGameFactory factory)
        {
            _progressService.Progress.WorldData.LootData.Changed += OnProgressChanged;
            _factory = factory;
        }

        public void OnProgressChanged()
        {
            if (IsLevelComplete())
                CompleteLevel();
        }

        public void CompleteLevel()
        {
            _coroutineRunner.StartCoroutine(RestartLevelIn(5));
            LevelCompleted?.Invoke();
            Debug.Log("Win");
        }

        private void DisablePlayerControls()
        {
            _factory.Player.GetComponentInChildren<CharacterController>().enabled = false;
            _factory.Player.GetComponent<KeyboardControl>().enabled = false;
        }

        private void EnablePlayerControls()
        {
            _factory.Player.GetComponentInChildren<CharacterController>().enabled = true;
            _factory.Player.GetComponent<KeyboardControl>().enabled = true;
        }
        private IEnumerator RestartLevelIn(int seconds)
        {
            DisablePlayerControls();

            yield return new WaitForSeconds(seconds);
            _resetLevelService.ResetLevel(Constants.MainLevel);
            
            EnablePlayerControls();
        }
        
        public bool IsLevelComplete() => 
            _progressService.Progress.WorldData.LootData.Collected >= LootToCompleteLevel;
    }
}