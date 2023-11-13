//Essa classe contém métodos de extensão para objetos que implementam a interface IDbCommand,
//que é parte do ADO.NET para interagir com bancos de dados.

using System;
using System.Data;

namespace UnitOfWorkADONET
{
    public static class CommandExtensions
    {
        /// <summary>
        /// Adicionar um novo parâmtro.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IDbDataParameter ParameterAdd(this IDbCommand command, string name, object value)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (name == null)
                throw new ArgumentNullException("name");

            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);

            return p;
        }

        /// <summary>
        /// Adicionar um novo parâmtro.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDbDataParameter ParameterAdd(this IDbCommand command, string name, object value, DbType type)
        {
            var p = ParameterAdd(command, name, value);
            p.DbType = type;
            return p;
        }

        /// <summary>
        /// Adicionar um novo parâmtro.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IDbDataParameter ParameterAdd(this IDbCommand command, string name, object value, ParameterDirection direction)
        {
            var p = ParameterAdd(command, name, value);
            p.Direction = direction;
            return p;
        }

        /// <summary>
        /// Adicionar um novo parâmtro.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IDbDataParameter ParameterAdd(this IDbCommand command, string name, object value, DbType type, ParameterDirection direction)
        {
            var p = ParameterAdd(command, name, value);
            p.DbType = type;
            p.Direction = direction;
            return p;
        }

        /// <summary>
        /// Obtem o valor de um parâmetro.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object ParameterValue(this IDbCommand command, string name)
        {
            return ((IDbDataParameter)command.Parameters[name]).Value;

        }

        /// <summary>
        /// Limpa os parâmetros associados ao Command.
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="ArgumentNullException"></exception>

        public static void ParametersClear(this IDbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            command.Parameters.Clear();
        }
    }
}
