
using GerenciarProduto.Entities;
using UnitOfWorkADONET;

namespace GerenciarProduto.Persistencia
{
    public class ProdutoRepository : IDisposable
    { 
        private readonly ADONETContext _bd;
        public ProdutoRepository(UnitOfWorkADONET.IDBContextFactory bd)
        {
            _bd = new ADONETContext(bd);
        }

        public bool Adicionar(Produto produto)
        {
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"insert into Produto (Nome, Estoque, Preco) values (@Nome, @Estoque, @Preco)";
                    cmd.ParameterAdd("@Nome", produto.Nome);
                    cmd.ParameterAdd("@Estoque", produto.Estoque);
                    cmd.ParameterAdd("@Preco", produto.Preco);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cmd.ParametersClear();
                        cmd.CommandText = "select LAST_INSERT_ID();";
                        produto.Id = Convert.ToInt32(cmd.ExecuteScalar());

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

        public bool Atualizar(Produto produto)
        {
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"update Produto set Nome = @Nome, Estoque = @Estoque, Preco = @Preco where ProdutoId = @ProdutoId";
                    cmd.ParameterAdd("@Nome", produto.Nome);
                    cmd.ParameterAdd("@Estoque", produto.Estoque);
                    cmd.ParameterAdd("@Preco", produto.Preco);
                    cmd.ParameterAdd("@ProdutoId", produto.Id);

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

        public Produto ObterProduto(int id)
        {
            Produto p = null;
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"select ProdutoId, Nome, Estoque, Preco
                                        from Produto
                                        where ProdutoId = @Id";

                    cmd.ParameterAdd("@Id", id);
                    using (var dr = cmd.ExecuteReader())
                    {

                        if (dr.Read())
                        {
                            p = new Produto();
                            p.Id = Convert.ToInt32(dr["ProdutoId"]);
                            p.Nome = dr["Nome"].ToString();
                            p.Estoque = Convert.ToInt32(dr["Estoque"]);
                            p.Preco = Convert.ToDecimal(dr["Preco"]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return p;
        }

        public List<Produto> ObterTodos()
        {
            List<Produto> ps = new List<Produto>();
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"select ProdutoId, Nome, Estoque, Preco
                                        from Produto
                                        order by Nome";

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Produto p = new Produto();
                        p.Id = Convert.ToInt32(dr["ProdutoId"]);
                        p.Nome = dr["Nome"].ToString();
                        p.Estoque = Convert.ToInt32(dr["Estoque"]);
                        p.Preco = Convert.ToDecimal(dr["Preco"]);
                        ps.Add(p);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return ps;
        }



        public void Excluir(int id)
        {
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"delete Produto where ProdutoId = @ProdutoId";
                    cmd.ParameterAdd("@ProdutoId", id);

                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }


        public List<Produto> ObterPorNome(string nome)
        {
            List<Produto> ps = new List<Produto>();
            try
            {
                using (var cmd = _bd.CreateCommand())
                {
                    cmd.CommandText = @"select ProdutoId, Nome, Estoque, Preco
                                        from Produto
                                        where Nome like @Nome
                                        order by Nome";

                    cmd.ParameterAdd("@Nome", nome);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Produto p = new Produto();
                        p.Id = Convert.ToInt32(dr["ProdutoId"]);
                        p.Nome = dr["Nome"].ToString();
                        p.Estoque = Convert.ToInt32(dr["Estoque"]);
                        p.Preco = Convert.ToDecimal(dr["Preco"]);
                        ps.Add(p);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return ps;
        }


 
        public void Dispose()
        {
            _bd.Dispose();
        }

    }
}
