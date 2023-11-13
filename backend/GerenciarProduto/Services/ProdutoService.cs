using GerenciarProduto.Persistencia;
using GerenciarProduto.Entities;

namespace GerenciarProduto.Services
{
    public class ProdutoService
    {
        private readonly ProdutoRepository _produtoRepository;
        public ProdutoService(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public bool Adicionar(Produto produto)
        {
            try
            {
                return _produtoRepository.Adicionar(produto);
            }
            catch
            {
                return false;
            }
        }

        public bool Atualizar(Produto produto)
        {
            var produtoExistente = _produtoRepository.ObterProduto(produto.Id);

            if (produtoExistente != null)
            {
                produtoExistente.Nome = produto.Nome;
                produtoExistente.Estoque = produto.Estoque;
                produtoExistente.Preco = produto.Preco;

                return _produtoRepository.Atualizar(produtoExistente);
            }

            return false;
        }

        public Produto Obter(int id)
        {
            return _produtoRepository.ObterProduto(id);
        }

        public List<Produto> ObterTodos()
        {
            return _produtoRepository.ObterTodos();
        }


        public void Excluir(int id)
        {
            _produtoRepository.Excluir(id);
        }

        public List<Produto> ObterPorNome(string nome)
        {
            return _produtoRepository.ObterPorNome(nome);
        }

        public void SalvarImagem(byte[] arq, int id)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
            Directory.CreateDirectory(dir);
            var nome = $"{id}.png";
            string nomeArq = Path.Combine(dir, nome);
            System.IO.File.WriteAllBytes(nomeArq, arq);
        }


        public byte[] BaixarImagem(int id)
        {
            byte[]? arq = null;
            var nome = $"{id}.png";
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads", nome);
            if (System.IO.File.Exists(dir))
            {
                arq = System.IO.File.ReadAllBytes(dir);
            }

            return arq;
        }

    }
}
