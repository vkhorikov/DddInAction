using System;

using DddInAction.Logic.Common;


namespace DddInAction.Logic.SnackMachines
{
    public class SnackMap : EntityMap<Snack>
    {
        public SnackMap()
        {
            Map(x => x.Name);
        }
    }
}
