﻿using System.ComponentModel.DataAnnotations;

namespace GerenciarProduto.ViewModel
{
    /// <summary>
    /// Dados da Todo
    /// </summary>
    public class TodoViewModel
    {


        /// <summary>
        /// Tarefa
        /// </summary>
        [Required]
        public string Descricao{ get; set; }

        
    }
}
