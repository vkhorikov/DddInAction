namespace DddInPractice.Logic.SnackMachines
{
    public class SnackMachineDto
    {
        public long Id { get; private set; }
        public decimal MoneyInside { get; private set; }

        public SnackMachineDto(long id, decimal moneyInside)
        {
            Id = id;
            MoneyInside = moneyInside;
        }
    }
}
