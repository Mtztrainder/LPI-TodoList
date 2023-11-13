using GerenciarProduto.Persistencia;
using GerenciarProduto.Entities;

namespace GerenciarProduto.Services
{
    public class CompraService
    {
        private readonly CompraRepository _compraRepository;
        public CompraService(CompraRepository compraRepository)
        {
            _compraRepository = compraRepository;
        }

        public (bool, string) Adicionar(Compra compra)
        {
            string msg = "";
            try
            {
                if (compra.Items.Count == 0)
                {
                    msg = "Sem itens";

                    return (false, msg);
                }

                foreach (var itemVM in compra.Items)
                {
                    CompraItem compraItem = new CompraItem();
                    compraItem.Total = itemVM.Quantidade * compraItem.Produto.Preco;
                    compra.Total += compraItem.Total;
                }

                var ok = _compraRepository.Adicionar(compra);

                return (ok, "");
            }
            catch (Exception ex) 
            {
                return (false, ex.Message);
            }
        }

      

    }
}
