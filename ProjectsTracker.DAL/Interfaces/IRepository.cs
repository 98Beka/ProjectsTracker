namespace ProjectsTracker.DAL.Interfaces {
    public interface IRepository<T> where T : class {
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        Task Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
