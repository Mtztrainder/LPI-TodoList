namespace GerenciarProduto.Entities
{
    public class CompraItem
    {
        public Produto Produto { get; set; }

        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
