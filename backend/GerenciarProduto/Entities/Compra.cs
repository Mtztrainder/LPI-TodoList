namespace GerenciarProduto.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal Total { get; set; }
        public List<CompraItem> Items { get; set; }

        public Compra()
        {
            Items = new List<CompraItem>();
        }
    }
}
