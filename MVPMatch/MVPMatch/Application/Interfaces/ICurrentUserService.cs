namespace MVPMatch.Application.Interfaces
{
    public interface ICurrentUserService
    {
        public string UserEmail { get; }
        public string UserId{ get; }
        public decimal Deposit { get; }
    }
}
