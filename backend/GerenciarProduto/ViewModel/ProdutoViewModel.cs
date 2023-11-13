using System.ComponentModel.DataAnnotations;

namespace GerenciarProduto.ViewModel
{
    /// <summary>
    /// Dados do Produto
    /// </summary>
    public class ProdutoViewModel
    {

        /// <summary>
        /// Nome do Produto
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Preço do Produto
        /// </summary>
        [Required]
        public decimal Preco { get; set; }

        /// <summary>
        /// Estoque do Produto
        /// </summary>
        [Required]
        public int Estoque { get; set; }
    }
}
