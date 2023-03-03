using System;
using Infrastructure.Services.LevelProgress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class WinScreen : MonoBehaviour
    {
        private ILevelProgressService _levelProgressService;
        public void Construct(ILevelProgressService levelProgressService)
        {
            Hide();
            _levelProgressService = levelProgressService;
            _levelProgressService.LevelCompleted += Show;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _levelProgressService.LevelCompleted -= Show;
        }
    }
}