using DddInPractice.Logic.Common;
using DddInPractice.Logic.Management;

namespace DddInPractice.Logic.Utils
{
    public static class Initer
    {
        public static void Init(string connectionString)
        {
            SessionFactory.Init(connectionString);
            HeadOfficeInstance.Init();
            DomainEvents.Init();
        }
    }
}
