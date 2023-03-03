namespace Infrastructure.Services.ResetLevel
{
    public interface IResetLevelService : IService
    {
        public void ResetLevel(string transferToLevel);
    }
}