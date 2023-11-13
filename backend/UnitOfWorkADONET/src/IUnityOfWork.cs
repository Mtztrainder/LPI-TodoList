//Esta interface IUnityOfWork é um contrato que especifica
//os métodos SaveChanges e CancelChanges que devem ser implementados
//por qualquer classe que a utilize.
//Além disso, qualquer classe que implemente IUnityOfWork
//também deve lidar com a liberação de recursos
//não gerenciados por meio da implementação do método Dispose da interface IDisposable.


using System;

namespace UnitOfWorkADONET
{
    public interface IUnityOfWork : IDisposable
    {
        void SaveChanges();
        void CancelChanges();

    }
}
