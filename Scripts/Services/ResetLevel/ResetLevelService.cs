using Infrastructure.Services.PersistentProgress;
using Infrastructure.States;

namespace Infrastructure.Services.ResetLevel
{
    class ResetLevelService : IResetLevelService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly GameStateMachine _stateMachine;

        public ResetLevelService(IPersistentProgressService progressService, GameStateMachine stateMachine)
        {
            _progressService = progressService;
            _stateMachine = stateMachine;
        }
        public void ResetLevel(string transferToLevel)
        {
            _progressService.ClearProgress();
            _stateMachine.Enter<LoadLevelState, string>(transferToLevel);
        }
    }
}