using GerenciarProduto.Persistencia;
using GerenciarProduto.Entities;

namespace GerenciarProduto.Services
{
    public class TodoService
    {
        private readonly TodoRepository _todoRepository;
        public TodoService(TodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public bool Adicionar(Todo todo)
        {
            try
            {
                todo.Id = Guid.NewGuid().ToString();
                return _todoRepository.Adicionar(todo);
            }
            catch
            {
                return false;
            }
        }

        public bool Atualizar(Todo todo)
        {
            var todoExistente = _todoRepository.Obter(todo.Id);

            if (todoExistente != null)
            {
                todoExistente.Descricao = todo.Descricao;

                return _todoRepository.Atualizar(todo);
            }

            return false;
        }

        public Todo Obter(string id)
        {
            return _todoRepository.Obter(id);
        }

        public List<Todo> ObterTodos()
        {
            return _todoRepository.ObterTodos();
        }


        public void Excluir(string id)
        {
            _todoRepository.Excluir(id);
        }


    }
}
