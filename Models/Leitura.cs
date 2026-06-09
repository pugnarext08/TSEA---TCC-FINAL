// namespace onde fica o modelo da aplicação
namespace TSEA.API.Models
{
    // classe que representa uma leitura da máquina
    public class Leitura
    {
        // id da leitura (identificador único)
        public int Id { get; set; }

        // data e hora em que a leitura foi feita
        public DateTime Data { get; set; }

        // valor da temperatura da máquina
        public double Temperatura { get; set; }

        // valor da vibração da máquina
        public double Vibracao { get; set; }

        // valor da corrente elétrica da máquina
        public double Corrente { get; set; }

        // nome da máquina que gerou a leitura
        public string Maquina { get; set; }
    }
}