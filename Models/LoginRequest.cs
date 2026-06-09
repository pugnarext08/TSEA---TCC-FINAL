// namespace onde ficam os modelos da API
namespace TSEA.API.Models
{
    // classe que representa os dados enviados no login
    public class LoginRequest
    {
        // email digitado pelo usuário
        public string Email { get; set; }

        // senha digitada pelo usuário
        public string Senha { get; set; }
    }
}