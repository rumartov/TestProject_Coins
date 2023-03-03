using Infrastructure.Services;
using Infrastructure.Services.ResetLevel;
using UnityEngine;

namespace Logic.KeyboardControl
{
    public class KeyboardControl : MonoBehaviour
    {
        private IResetLevelService _resetLevelService;

        public void Construct(IResetLevelService resetLevelService)
        {
            _resetLevelService = resetLevelService;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
        }

        private void RestartLevel()
        {
            _resetLevelService.ResetLevel(Constants.MainLevel);
        }
    }
}