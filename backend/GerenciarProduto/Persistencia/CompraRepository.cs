using GerenciarProduto.Entities;
using UnitOfWorkADONET;

namespace GerenciarProduto.Persistencia
{
    public class CompraRepository : IDisposable
    { 
        private readonly ADONETContext _bd;
        public CompraRepository(UnitOfWorkADONET.IDBContextFactory bd)
        {
            _bd = new ADONETContext(bd);
        }

        public bool Adicionar(Compra compra)
        {
            using (var uow = _bd.CreateUnitOfWork())
            {
                try
                {
                    using (var cmd = _bd.CreateCommand())
                    {
                        cmd.CommandText = @"insert into Compra (Data, Total) values (@Data, @Total)";
                        cmd.ParameterAdd("@Data", compra.Data);
                        cmd.ParameterAdd("@Total", compra.Total);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            cmd.ParametersClear();
                            cmd.CommandText = "select LAST_INSERT_ID();";
                            compra.Id = Convert.ToInt32(cmd.ExecuteScalar());


                            cmd.CommandText = @"insert into CompraItem (CompraId, ProdutoId, Quantidade, PrecoUnitario, Total)
                                                values (@CompraId, @ProdutoId, @Quantidade, @PrecoUnitario, @Total)";
                              
                            foreach (var item in compra.Items)
                            {
                                cmd.ParametersClear();
                                cmd.ParameterAdd("@CompraId", compra.Id);
                                cmd.ParameterAdd("@ProdutoId", item.Produto.Id);
                                cmd.ParameterAdd("@Quantidade", item.Quantidade);
                                cmd.ParameterAdd("@PrecoUnitario", item.PrecoUnitario);
                                cmd.ParameterAdd("@Total", item.Total);
                                cmd.ExecuteNonQuery();
                            }

                        }

                    }
                    uow.SaveChanges();
                    return true;

                }
                catch (Exception ex)
                {
                    uow.CancelChanges();
                    throw new Exception(ex.Message, ex);
                }
            }

            return false;
        }
         
        public void Dispose()
        {
            _bd.Dispose();
        }

    }
}
