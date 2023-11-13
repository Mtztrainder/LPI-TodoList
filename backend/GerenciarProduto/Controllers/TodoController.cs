using GerenciarProduto.Services;
using GerenciarProduto.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GerenciarProduto.Controllers
{

    /// <summary>
    /// Gerenciador de Tarefas.
    /// </summary>
    //[Authorize("APIAuth")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : CustomControllerBase
    {
        private readonly TodoService _todoService;


        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }



        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Adicionar(TodoViewModel todoVM)
        {

            Entities.Todo todo = new Entities.Todo();
            todo.Descricao = todoVM.Descricao;

            var ok = _todoService.Adicionar(todo); 

            if (ok)
            {
                AddSuccessMessage("Tarefa criada com sucesso.");
                return CustomResponse(System.Net.HttpStatusCode.Created, true, todo);
            }
            else
            {
                AddErrorMessage("Não foi possível salvar a nova Tarefa.");
                return CustomResponse(System.Net.HttpStatusCode.UnprocessableEntity, false);
            }
        }


  
        [HttpPut]
        [Route("[action]")]
        public IActionResult Atualizar(string id, TodoAtualizacaoViewModel todoVM)
        {
            var todo = _todoService.Obter(id);

            if (todo == null)
            {
                AddNotFoundMessage("Todo não encontrado.");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false);
            }
            else
            {
                todo.Id = id;
                todo.Descricao = todoVM.Descricao;

                _todoService.Atualizar(todo);
                AddSuccessMessage("Todo alterado com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true);
            }
        }

        [HttpDelete]
        [Route("[action]")]
        public IActionResult Excluir(string id)
        {
            if (_todoService.Obter(id) == null)
            {
                AddNotFoundMessage("Todo não encontrado.");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false);
            }
            else
            {
                _todoService.Excluir(id);
                AddSuccessMessage("Todo excluído com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult ObterTodos()
        {
            var todos = _todoService.ObterTodos();

            if (todos.Count == 0)
            {
                AddNotFoundMessage("Todos não encontrados!");
                return CustomResponse(System.Net.HttpStatusCode.NotFound, false, "");
            }
            else
            {
                List<TodoRetornoViewModel> todosVM = new();

                foreach (var t in todos)
                {
                    TodoRetornoViewModel tVM = new();
                    tVM.Id = t.Id;
                    tVM.Descricao = t.Descricao;
                    todosVM.Add(tVM);
                }


                AddSuccessMessage("Todos encontrados com sucesso!");
                return CustomResponse(System.Net.HttpStatusCode.OK, true, todosVM);
            }

        }

    }
}
