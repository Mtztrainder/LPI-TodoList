namespace GerenciarProduto.ViewModel
{
    public class CompraViewModel
    {
        public DateTime Data { get; set; }
        public List<CompraItemViewModel> Itens { get; set; } = new List<CompraItemViewModel>();
    }


    public class CompraItemViewModel
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
