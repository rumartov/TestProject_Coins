namespace Infrastructure.Services.Utils
{
    public class UtilService : IUtilService
    {
        private readonly Utils _utils;
        public UtilService()
        {
            _utils = new Utils();
        }
        public Utils GetUtils() => _utils;
    }
}