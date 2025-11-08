Challenge 2025 - Advanced Business Development with .NET
Grupo

Nome: Pablo Lopes Doria de Andrade
RM: 556834

Nome: Vinicius Leopoldino de Oliveira
RM: 557047

Justificativa da Arquitetura

O projeto foi desenvolvido em .NET 8 (Web API) com arquitetura em camadas (Clean Architecture), garantindo separação de responsabilidades e facilidade de manutenção.

Challenge_1/
│── Application/ → serviços, interfaces e regras de aplicação
│── Domain/ → entidades e regras de negócio
│── Doc/ → samples usados no Swagger (request e response)
│── Infrastructure/ → contexto do banco de dados (Oracle + EF Core)
│── Presentation/ → controllers, DTOs e documentação via Swagger
│── Challenge_1.sln → solution principal

Principais características implementadas

Arquitetura em camadas (Domain, Application, Infrastructure e Presentation)

Paginação em endpoints de listagem

Swagger documentado com exemplos de request e response

DTOs para isolar as entidades de domínio

Validações utilizando DataAnnotations

Implementação de HATEOAS nos endpoints principais

Tratamento global de erros

Testes unitários e de integração com xUnit

Autenticação via JWT

Health Checks (verificação de disponibilidade da API e do Oracle)

Endpoints de Machine Learning (ML.NET) para predição e recomendação

Tecnologias Utilizadas

.NET 8 (C#)

Entity Framework Core 8

Oracle Database

Swagger (Swashbuckle)

ML.NET (Machine Learning)

xUnit (Testes unitários e integração)

JWT Authentication

Health Checks

Moq (para mocking em testes)

Como Executar
Pré-requisitos

Visual Studio 2022 ou Visual Studio Code

.NET 8 SDK

Banco de Dados Oracle (conexão FIAP)

Clonar o repositório
git clone https://github.com/Pablo0703/Challenge_Dot_Net_Sprint_4.git

Configurar conexão com o banco de dados

No arquivo appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "User Id=RMXXXXX;Password=XXXXX;Data Source=oracle.fiap.com.br:1521/ORCL"
}

Executar a aplicação
dotnet run


A API estará disponível em:
https://localhost:7030/swagger

Endpoints e Exemplos
Moto

GET /api/ControllerMoto

GET /api/ControllerMoto/{id}

GET /api/ControllerMoto/porPlaca?placa=ABC1234

POST /api/ControllerMoto

Request:

{
  "idTipo": 2,
  "idStatus": 1,
  "placa": "ABC1234",
  "modelo": "Honda CG 160",
  "anoFabricacao": 2021,
  "anoModelo": 2022,
  "chassi": "9C2KC1670LR012345",
  "cilindrada": 160,
  "cor": "Preta",
  "dataAquisicao": "2023-01-10T00:00:00",
  "valorAquisicao": 12500.00,
  "idNotaFiscal": null
}

Pátio

GET /api/ControllerPatio

GET /api/ControllerPatio/{id}

GET /api/ControllerPatio/porNome?nome=Pátio

POST /api/ControllerPatio

Request:

{
  "idFilial": 1,
  "nome": "Pátio Central SP",
  "areaM2": 500,
  "capacidade": 50,
  "ativo": "S"
}

Endereço

GET /api/ControllerEndereco

GET /api/ControllerEndereco/{id}

GET /api/ControllerEndereco/porCidade?cidade=São Paulo

POST /api/ControllerEndereco

Request:

{
  "logradouro": "Av. Paulista",
  "numero": "1000",
  "complemento": "Conjunto 101",
  "bairro": "Bela Vista",
  "cep": "01310000",
  "cidade": "São Paulo",
  "estado": "SP",
  "pais": "Brasil"
}

Filial

GET /api/ControllerFilial

GET /api/ControllerFilial/{id}

GET /api/ControllerFilial/porNome?nome=SP

POST /api/ControllerFilial

Request:

{
  "nome": "Mottu São Paulo",
  "cnpj": "98765432000777",
  "telefone": "(11)95439-8488",
  "email": "sp@mottu.com.br",
  "ativo": "S",
  "idEndereco": 1
}

Localização da Moto

GET /api/ControllerLocalizacaoMoto

GET /api/ControllerLocalizacaoMoto/{id}

GET /api/ControllerLocalizacaoMoto/porIdMoto?idMoto=1

POST /api/ControllerLocalizacaoMoto

Request:

{
  "idMoto": 1,
  "idZona": 1,
  "dataHoraEntrada": "2025-05-01T08:00:00",
  "dataHoraSaida": null
}

Histórico de Localização

GET /api/ControllerHistoricoLocalizacao

GET /api/ControllerHistoricoLocalizacao/{id}

GET /api/ControllerHistoricoLocalizacao/porIdMoto?idMoto=1

POST /api/ControllerHistoricoLocalizacao

Request:

{
  "idMoto": 1,
  "idMotociclista": 1,
  "idZonaPatio": 1,
  "dataHoraSaida": "2025-05-01T09:00:00",
  "dataHoraEntrada": "2025-05-02T18:00:00",
  "kmRodados": 120.5,
  "idStatusOperacao": 1
}

Health Check

GET /api/v1/Health/live
Verifica se a API está ativa.

GET /api/v1/Health/ready
Verifica se o Oracle e dependências estão disponíveis.

Autenticação (JWT)

POST /api/v1/Auth/login

Request:

{
  "username": "admin",
  "password": "12345"
}


Response:

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

Predição (ML.NET)

POST /api/v1/Predicao/consumo
Prediz o consumo médio (km/L) de uma moto.

Request:

{
  "cilindrada": 160,
  "peso": 120,
  "velocidadeMedia": 80
}


Response:

{
  "input": { "cilindrada": 160, "peso": 120, "velocidadeMedia": 80 },
  "consumoPrevisto": 33.5,
  "observacao": "Previsão gerada via modelo ML.NET"
}


POST /api/v1/Predicao/treinar
Treina o modelo ML.NET com novos dados.

Request:

{
  "cilindrada": 160,
  "peso": 125,
  "velocidadeMedia": 70,
  "consumoReal": 32.1
}

Recomendação (ML.NET)

POST /api/v1/Recomendacao/moto
Gera uma recomendação de moto para o usuário com base no perfil e afinidade.

Request:

{
  "usuarioId": 1,
  "motoId": 42
}


Response:

{
  "usuarioId": 1,
  "motoId": 42,
  "score": 0.873,
  "observacao": "Score de afinidade gerado via modelo ML.NET"
}

Requisitos Atendidos

CRUD completo para todas as entidades

Paginação em listagens

Swagger com exemplos e documentação

DTOs e validações

HATEOAS nos endpoints principais

Health Checks (liveness e readiness)

Autenticação JWT

Endpoints com ML.NET (predição e recomendação)

Testes unitários e de integração (xUnit + Moq)

Conexão com Oracle Database

Testes
Executar os testes
dotnet test


Os testes cobrem:

Criação e consulta de entidades via Services e Controllers

Mock de dependências com Moq

Verificações de status HTTP

Testes de integração com WebApplicationFactory

Cobertura dos endpoints de CRUD, ML.NET e autenticação

Conclusão

Este projeto atende a todos os requisitos da Sprint 4 - Advanced Business Development with .NET,
demonstrando domínio em:

Arquitetura multicamadas

Integração com Oracle via EF Core

Documentação e versionamento de API

Machine Learning (ML.NET)

Testes automatizados com xUnit

Boas práticas de desenvolvimento e segurança (JWT, validações, Health Checks)
