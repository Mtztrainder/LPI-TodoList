using GerenciarProduto.Services;
using GerenciarProduto.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GerenciarProduto.Controllers
{

    /// <summary>
    /// Gerenciador de Tarefas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SiteLoginController : CustomControllerBase
    {


        public SiteLoginController()
        {
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Autenticar(SiteUsuarioViewModel usuarioVM)
        {

            if (usuarioVM.Nome == "adm" && usuarioVM.Senha == "123")
            {
                var userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "andre.menegassi"), //Claim padrão
                    new Claim(ClaimTypes.Role, "Administrador"), //Claim padrão
                    new Claim("id", "123456"),
                    new Claim("cpf", "11111111111"), //sensível (?)  / pode-se cripgrafar
                    new Claim("data", DateTime.Now.ToString()),
                    new Claim("email", "andremenegasssi@hotmail.com"),
                    new Claim("contratoId", "11111111"),
                    new Claim("origem", "cookie")

                };

                var identity = new ClaimsIdentity(userClaims, "Identificação do usuário - Site Adm");
                //Juntar as Claims em um conjunto.
                ClaimsPrincipal identidade = new ClaimsPrincipal(identity);

                //Gerando a cookie e informando que deve ser persistente, isto é, mantém o usuário autenticado.
                AuthenticationHttpContextExtensions.SignInAsync(HttpContext, identidade, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(360)
                });


                AddSuccessMessage("Usuário autenticado com sucesso.");
                return CustomResponse(System.Net.HttpStatusCode.Accepted, true);
            }
            else
            {
                AddErrorMessage("Usuário inválido.");
                return CustomResponse(System.Net.HttpStatusCode.UnprocessableEntity, false);
            }
        }

    }
}