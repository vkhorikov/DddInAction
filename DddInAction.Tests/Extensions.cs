using System;

using DddInAction.Logic.Utils;

using FluentAssertions;


namespace DddInAction.Tests
{
    internal static class Extensions
    {
        public static void ShouldBreakContract(this Action action, string because = null, params object[] becauseArgs)
        {
            action.ShouldThrow<ContractException>(because, becauseArgs);
        }
    }
}
