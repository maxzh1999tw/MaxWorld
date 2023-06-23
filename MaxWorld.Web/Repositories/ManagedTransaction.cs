namespace MaxWorld.Web.Repositories
{
    public class ManagedTransaction : IDisposable
    {
        private Repository _repository;

        public ManagedTransaction(Repository repository)
        {
            _repository = repository;
        }

        public void Commit()
        {
            if (_repository.Transaction == null)
            {
                throw new InvalidOperationException();
            }

            _repository.Transaction.Commit();
            _repository.Transaction = null;
        }

        public void Rollback()
        {
            if (_repository.Transaction == null)
            {
                throw new InvalidOperationException();
            }

            _repository.Transaction.Rollback();
            _repository.Transaction = null;
        }

        public void Dispose()
        {
            if (_repository.Transaction != null)
            {
                _repository.Transaction.Dispose();
                _repository.Transaction = null;
            }
        }
    }
}
