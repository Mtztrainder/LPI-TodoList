using System.ComponentModel.DataAnnotations;

namespace GerenciarProduto.ViewModel
{
    /// <summary>
    /// Dados da Todo
    /// </summary>
    public class SiteUsuarioViewModel
    {


        /// <summary>
        /// Nome do usuário
        /// </summary>
        [Required]
        public string Nome{ get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required]
        public string Senha { get; set; }


    }
}
