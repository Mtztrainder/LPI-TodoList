
using GerenciarProduto.Entities;
using UnitOfWorkADONET;

namespace GerenciarProduto.Persistencia
{
    public class TodoRepository : IDisposable
    { 
        private readonly ADONETContext _bd;
        public TodoRepository(UnitOfWorkADONET.IDBContextFactory bd)
        {
            _bd = new ADONETContext(bd);
        }

        public bool Adicionar(Todo todo)
        {
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"insert into Todo (TodoId, Descricao) values (@Id, @Descricao)";
                    cmd.ParameterAdd("@Id", todo.Id);
                    cmd.ParameterAdd("@Descricao", todo.Descricao);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }

                }
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return false;
        }

        public bool Atualizar(Todo todo)
        {
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"update Todo set Descricao = @Descricao where TodoId = @TodoId";
                    cmd.ParameterAdd("@TodoId", todo.Id);
                    cmd.ParameterAdd("@Descricao", todo.Descricao);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


            return false;
        }

        public Todo Obter(string id)
        {
            Todo t = null;
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"select TodoId, Descricao
                                        from Todo
                                        where TodoId = @Id";

                    cmd.ParameterAdd("@Id", id);
                    using (var dr = cmd.ExecuteReader())
                    {

                        if (dr.Read())
                        {
                            t = new Todo ();
                            t.Id = dr["TodoId"].ToString();
                            t.Descricao = dr["Descricao"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return t;
        }

        public List<Todo> ObterTodos()
        {
            List<Todo> ts = new List<Todo>();
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"select TodoId, Descricao
                                        from Todo";

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var t = new Todo();
                        t.Id = dr["TodoId"].ToString();
                        t.Descricao = dr["Descricao"].ToString();

                        ts.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return ts;
        }



        public void Excluir(string id)
        {
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"delete from Todo 
                                        where TodoId = @TodoId";
                    cmd.ParameterAdd("@TodoId", id);

                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }


 
        public void Dispose()
        {
            _bd.Dispose();
        }

    }
}
