using GerenciarProduto.Entities;
using GerenciarProduto.Services;
using GerenciarProduto.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GerenciarProduto.Controllers
{
    [ApiController]
    [Route("api/[controller]", Name = "Compra de Produtos")]
    public class CompraController : CustomControllerBase
    {
        public readonly CompraService _compraService;
        public readonly ProdutoService _produtoService;

        public CompraController(ProdutoService produtoService, CompraService compraService)
        {
            _produtoService = produtoService;
            _compraService = compraService;
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult Comprar(CompraViewModel compraVM)
        {
            Entities.Compra compra = new();
            compra.Data = compraVM.Data;
            
            foreach (var itemVM in compraVM.Itens)
            {
                CompraItem compraItem = new CompraItem();
                compraItem.Produto = _produtoService.Obter(itemVM.ProdutoId);
                compraItem.Quantidade = itemVM.Quantidade;
                compraItem.Total = itemVM.Quantidade * compraItem.Produto.Preco;
                compraItem.PrecoUnitario = compraItem.Produto.Preco;
                compra.Items.Add(compraItem);
            }

            compra.Total += compra.Items.Sum(i => i.Total);

            (var ok, string msg) = _compraService.Adicionar(compra);

            if (ok)
            {
                AddSuccessMessage("Compra criada com sucesso.");
                return CustomResponse(System.Net.HttpStatusCode.Created, true, compra);
            }
            else
            {
                AddErrorMessage(msg);
                return CustomResponse(System.Net.HttpStatusCode.InternalServerError, false);
            }

        }
    }
}
