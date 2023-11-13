using Microsoft.AspNetCore.Mvc;

namespace GerenciarProduto.Controllers
{
    public class CustomControllerBase : ControllerBase
    {

        protected CustomControllerBase()
        {
            //Messages = new();
            Messages = new List<Message>();
        }

        protected enum TypeMessage
        {
            Success = 1,
            InvalidField = 2,
            Error= 3,
            Information = 4,
            Alert = 5,
            Created = 6,
            NotFound = 7
        }

        protected struct Message
        {
            public string Text { get; set; }
            public string Type { get; set; }

        }

        protected List<Message> Messages { get; set; }


        protected void AddSuccessMessage(string text)
        {
            Messages.Add(new Message()
            {
                Text = text,
                Type = TypeMessage.Success.ToString()
            });
        }

        protected void AddNotFoundMessage(string text)
        {
            Messages.Add(new Message()
            {
                Text = text,
                Type = TypeMessage.NotFound.ToString()
            });
        }

        protected void AddErrorMessage(string text)
        {
            Messages.Add(new Message()
            {
                Text = text,
                Type = TypeMessage.Error.ToString()
            });
        }



        protected IActionResult CustomResponse(System.Net.HttpStatusCode statusCode,
            bool success, object data = null)
            
        {
            var response = new
            {
                success = success,
                messages = Messages,
                data = data
            };

            return StatusCode((int)statusCode, response);
        }
    }
}
