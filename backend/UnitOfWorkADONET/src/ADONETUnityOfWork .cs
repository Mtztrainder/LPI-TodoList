//Basicamente, esta classe encapsula uma transação de banco de dados e
//fornece métodos para confirmar, reverter ou descartar a transação,
//além de implementar a interface IDisposable para a limpeza de recursos.
//Ela também permite a execução de ações personalizadas após um commit ou rollback.

using System;
using System.Data;

namespace UnitOfWorkADONET
{
    public class ADONETUnityOfWork : IUnityOfWork
    {
        private IDbTransaction _transaction;
        private readonly Action<ADONETUnityOfWork> _rolledBack;
        private readonly Action<ADONETUnityOfWork> _committed;
        public IDbTransaction Transaction { get; private set; }

        public ADONETUnityOfWork(IDbTransaction transaction, Action<ADONETUnityOfWork> rolledBack, Action<ADONETUnityOfWork> committed)
        {
            Transaction = transaction;
            _transaction = transaction;
            _rolledBack = rolledBack;
            _committed = committed;
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Não é permitido chamar 'salvar alterações' duas vezes.");

            _transaction.Commit();
            _committed(this);
            _transaction = null;
        }

        public void CancelChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Não é permitido chamar 'cancelar alterações' duas vezes.");

            _transaction.Rollback();
            _rolledBack(this);
            _transaction = null;
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;

            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack(this);
            _transaction = null;
        }
    }
}
