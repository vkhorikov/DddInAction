using System;


namespace DddInAction.Logic.Utils
{
    internal static class Contracts
    {
        public static void Require(params bool[] preconditions)
        {
            foreach (bool precondition in preconditions)
            {
                if (!precondition)
                    throw new ContractException();
            }
        }


        public static void RequireNotNull<T>(params T[] objs)
        {
            foreach (T obj in objs)
            {
                Require(obj != null);
            }
        }
    }
}
