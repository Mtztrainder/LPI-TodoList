//Esta interface ter por objetivo implementar o padrão de design Unit of Work
//utilizando ADO.NET para interagir com diferentes provedores de banco de dados,
//como MySQL, SQL Server e Oracle.

//uma "Factory" (ou fábrica) geralmente se refere
//a um padrão de design chamado "Factory Pattern".
//Esse padrão é usado para criar objetos de maneira
//flexível e encapsular a lógica de criação. Uma "Factory"
//é responsável por instanciar objetos de classes específicas
//com base em parâmetros ou condições fornecidas, tornando o
//código mais modular e de fácil manutenção.

using System.Data.Common;

namespace UnitOfWorkADONET
{
    public interface IDBContextFactory
    {
        public enum TpProvider
        {
            MySQL = 1,
            SQLServer = 2,
            Oracle = 3
        }


        //DbConnection é uma classe abstrata que representa uma conexão de dados e é parte do ADO.NET.
        DbConnection Create();
    }
}
