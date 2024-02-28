namespace UniMagazine.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //IABCRepository
        public IUserRepository UserRepository { get; }

        public void Save();
    }
}
