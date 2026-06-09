// classe que representa um usuário do sistema
public class Usuario
{
    // identificador único do usuário
    public int Id { get; set; }

    // nome do usuário
    public string Nome { get; set; }

    // email usado para login
    public string Email { get; set; }

    // senha do usuário
    public string Senha { get; set; }

    // perfil do usuário (ex: ADMIN ou OPERADOR)
    public string Perfil { get; set; }
}