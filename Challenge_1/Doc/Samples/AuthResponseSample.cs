using Challenge_1.Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class AuthResponseSample : IExamplesProvider<AuthResponseDTO>
    {
        public AuthResponseDTO GetExamples()
        {
            return new AuthResponseDTO
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                Usuario = "admin",
                Perfil = "Administrador"
            };
        }
    }
}
