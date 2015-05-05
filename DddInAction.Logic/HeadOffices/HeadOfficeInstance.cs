using System;

using DddInAction.Logic.Utils;


namespace DddInAction.Logic.HeadOffices
{
    public static class HeadOfficeInstance
    {
        private const long HeadOfficeId = 1;

        public static HeadOffice Instance { get; private set; }


        public static void Init()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Instance = unitOfWork.Get<HeadOffice>(HeadOfficeId).Value;
            }
        }
    }
}
