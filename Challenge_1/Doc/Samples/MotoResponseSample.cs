using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class MotoResponseSample : IExamplesProvider<MotoDTO>
    {
        public MotoDTO GetExamples()
        {
            return new MotoDTO
            {
                Id = 10,
                IdTipo = 2,
                IdStatus = 1,
                Placa = "ABC1234",
                Modelo = "Honda CG 160",
                AnoFabricacao = 2021,
                AnoModelo = 2022,
                Chassi = "9C2KC1670LR012345",
                Cilindrada = 160,
                Cor = "Preta",
                DataAquisicao = DateTime.Parse("2023-01-10"),
                ValorAquisicao = 12500.00M,
                IdNotaFiscal = null
            };
        }
    }
}
