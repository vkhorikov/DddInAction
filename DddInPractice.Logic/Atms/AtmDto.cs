namespace DddInPractice.Logic.Atms
{
    public class AtmDto
    {
        public long Id { get; private set; }
        public decimal Cash { get; private set; }

        public AtmDto(long id, decimal cash)
        {
            Id = id;
            Cash = cash;
        }
    }
}
