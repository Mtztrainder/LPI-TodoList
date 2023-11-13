using GerenciarProduto.Services;
using GerenciarProduto.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GerenciarProduto.Controllers
{

    /// <summary>
    /// Gerenciador de Produtos.
    /// </summary>
    [Authorize("APIAuth")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : CustomControllerBase
    {
        private readonly ProdutoService _produtoService;


        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }



        /// <summary>
        /// Adiciona um Produto.
        /// </summary>
        /// <remarks>
        /// Algum complemento/observações...
        /// </remarks>
        /// <param name="produtoVM">Dados do produto.</param>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Adicionar(ProdutoViewModel produtoVM)
        {

            Entities.Produto produto = new Entities.Produto();
            produto.Preco = produtoVM.Preco;
            produto.Estoque = produtoVM.Estoque;
            produto.Nome = produtoVM.Nome;

            var ok = _produtoService.Adicionar(produto); 

            if (ok)
            {
                AddSuccessMessage("Produto criado com sucesso.");
                return CustomResponse(System.Net.HttpStatusCode.Created, true, produto);
            }
            else
            {
                AddErrorMessage("Não foi possível salvar o novo Produto.");
                return CustomResponse(System.Net.HttpStatusCode.UnprocessableEntity, false);
            }
        }


        /// <summary>
        /// Atualizar produtos de uma Lista de Produtos do tipo Integrada.
        /// </summary>
        /// <param name="id">Id da Lista de Produtos que irá receber os produtos.</param>
        /// <param name="produtoVM">Produtos a serem inseridos.</param>
        [HttpPut]
        [Route("[action]")]
        public IActionResult Atualizar(int id, ProdutoAtualizacaoViewModel produtoVM)
        {
            var produto = _produtoService.Obter(id);

            if (produto == null)
            {
                AddNotFoundMessage("Produto não encontrado.");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false);
            }
            else
            {
                produto.Preco = produtoVM.Preco;
                produto.Estoque = produtoVM.Estoque;
                produto.Nome = produtoVM.Nome;

                _produtoService.Atualizar(produto);
                AddSuccessMessage("Produto alterado com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true);
            }
        }

        [HttpDelete]
        [Route("[action]")]
        public IActionResult Excluir(int id)
        {
            if (_produtoService.Obter(id) == null)
            {
                AddNotFoundMessage("Produto não encontrado.");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false);
            }
            else
            {
                _produtoService.Excluir(id);
                AddSuccessMessage("Produto excluído com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AdicionarImagem(int id)
        {
            if (Request.Form.Files.Count == 0)
            {
                AddNotFoundMessage("Arquivo não enviado.");
                return CustomResponse(System.Net.HttpStatusCode.BadRequest, false);
            }
            else
            {
                if (_produtoService.Obter(id) == null)
                {
                    AddNotFoundMessage("Produto não encontrado.");
                    return CustomResponse(System.Net.HttpStatusCode.NotFound, false);
                }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        Request.Form.Files[0].CopyTo(ms);
                        string tipo = Request.Form.Files[0].ContentType;
                        var tamanhoBytes = Request.Form.Files[0].Length;
                        var tamanhoMegaBytes = (double)tamanhoBytes / (1024 * 1024);
                        var arq = ms.ToArray();
                        if (tamanhoMegaBytes > 1)
                        {
                            AddErrorMessage("Imagem do produto é maior que 1MB");
                            return CustomResponse(System.Net.HttpStatusCode.BadRequest, false, id);
                        }
                        else
                        {
                            if (tipo != "image/png")
                            {
                                AddErrorMessage("Imagem do produto não está no formato correto!");
                                return CustomResponse(System.Net.HttpStatusCode.BadRequest, false, id);
                            }
                            else
                            {
                                _produtoService.SalvarImagem(arq, id);
                                AddSuccessMessage("Imagem do produto inserida com sucesso!");
                                return CustomResponse(System.Net.HttpStatusCode.OK, true, id);
                            }
                        }
                    }
                }
            }
            
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult BaixarImagem(int id)
        {
            if (_produtoService.Obter(id) == null)
            {
                AddNotFoundMessage("Produto não encontrado.");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false);
            }
            else
            {
                byte[] arq = _produtoService.BaixarImagem(id);
                if (arq == null)
                {
                    return File(arq, "image/png", $"{id}.png");
                }
                else
                {
                    AddNotFoundMessage("Arquivo não encontrado!");
                    return CustomResponse(System.Net.HttpStatusCode.NotFound, false, id);
                }
            }
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult ObterPorId(int id)
        {
            var produto = _produtoService.Obter(id);
            if (produto == null)
            {
                AddNotFoundMessage("Produto não encontrado.");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false);
            }
            else
            {
                ProdutoRetornoViewModel produtoVM = new ProdutoRetornoViewModel();
                produtoVM.Id = produto.Id;
                produtoVM.Preco = produto.Preco;
                produtoVM.Estoque = produto.Estoque;
                produtoVM.Nome = produto.Nome;

                AddSuccessMessage("Produto encontrado com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true, produtoVM);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult ObterTodos()
        {
            var nome = HttpContext.User.Identity?.Name;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var cpf = HttpContext.User.Claims?.FirstOrDefault(a => a.Type == "cpf")?.Value;
           
            var produtos = _produtoService.ObterTodos();

            if (produtos.Count == 0)
            {
                AddNotFoundMessage("Produtos não encontrados!");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false, "");
            }
            else
            {
                List<ProdutoRetornoViewModel> produtosVM = new();

                foreach (var produto in produtos)
                {
                    ProdutoRetornoViewModel produtoVM = new ProdutoRetornoViewModel();
                    produtoVM.Id = produto.Id;
                    produtoVM.Preco = produto.Preco;
                    produtoVM.Estoque = produto.Estoque;
                    produtoVM.Nome = produto.Nome;

                    produtosVM.Add(produtoVM);
                }


                AddSuccessMessage("Produtos encontrados com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true, produtosVM);
            }

        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ObterPorNome(string nome)
        {

            var produtos = _produtoService.ObterPorNome(nome);

            if (produtos.Count == 0)
            {
                AddNotFoundMessage("Produtos não encontrados!");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false, "");
            }
            else
            {
                List<ProdutoRetornoViewModel> produtosVM = new();

                foreach (var produto in produtos)
                {
                    ProdutoRetornoViewModel produtoVM = new ProdutoRetornoViewModel();
                    produtoVM.Id = produto.Id;
                    produtoVM.Preco = produto.Preco;
                    produtoVM.Estoque = produto.Estoque;
                    produtoVM.Nome = produto.Nome;

                    produtosVM.Add(produtoVM);
                }


                AddSuccessMessage("Produtos encontrados com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true, produtosVM);
            }

        }

    }
}
