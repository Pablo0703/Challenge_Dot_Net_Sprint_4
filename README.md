# Challenge 2025 - Advanced Business Development with .NET

## Grupo

* Nome: Pablo Lopes Doria de Andrade
* RM: 556834

* Nome: Vinicius Leopoldino de Oliveira
* RM: 557047

📢 Atualizações Sprint 4

Durante a Sprint 4, o projeto foi expandido com novos recursos de segurança, monitoramento, inteligência artificial e automação de testes.
As principais melhorias e implementações foram:

✅ Autenticação JWT – inclusão de autenticação via token JWT Bearer, garantindo acesso seguro aos endpoints.
✅ Versionamento da API – implementação do versionamento via URL (/api/v1/...).
✅ Health Check – endpoint /health para verificar a integridade da aplicação e do banco Oracle.
✅ Endpoint ML.NET – adicionado um endpoint com modelo de Machine Learning, integrando IA para previsão de dados.
✅ Testes Automatizados (xUnit) – criação de testes unitários e de integração com boas práticas (Assert.IsType, async/await, Data).
✅ Rate Limiting e Response Compression – para otimizar performance e segurança.
✅ Pipeline CI/CD (Azure DevOps) – automação completa de build, testes e deploy da aplicação na nuvem.
✅ Swagger Atualizado – documentação expandida com exemplos, modelos e responses detalhados.

---
🏗️ Justificativa da Arquitetura

O projeto foi desenvolvido em .NET 8 (Web API) com arquitetura em camadas (Domain, Application, Infrastructure, Presentation).

Challenge_1/
│── Application/        → serviços e interfaces
│── Domain/             → entidades e regras de negócio
│── Doc/                → samples usados no Swagger (request e response)
│── Infrastructure/     → contexto do banco de dados (Oracle + EF Core)
│── Presentation/       → controllers, DTOs, documentação via Swagger
│── Challenge_1.sln     → Solution


Além disso, foram implementados:
✅ Paginação nos endpoints de listagem
✅ Swagger com exemplos de requests/responses
✅ DTOs para evitar exposição direta de entidades
✅ Validações via DataAnnotations
✅ HATEOAS nos endpoints principais
✅ Tratamento de erros

🛠️ Tecnologias Utilizadas

.NET 8 (C#)

Entity Framework Core

Oracle Database

Swagger (Swashbuckle)

xUnit (para testes)

▶️ Como Executar
1. Pré-requisitos

Visual Studio 2022 ou VS Code

.NET 8 SDK

Banco de dados Oracle

2. Clonar o repositório
git clone https://github.com/Pablo0703/Challenge_3_Dot_net.git

3. Configurar conexão

No arquivo appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "User Id=RMXXXXX;Password=XXXXX;Data Source=oracle.fiap.com.br:1521/ORCL"
}

4. Rodar a aplicação
dotnet run

🔹 Versionamento da API

A partir da Sprint 4, todos os endpoints foram organizados com versionamento via URL, garantindo a compatibilidade entre versões futuras da aplicação.

Exemplo de Endpoint (v1)
GET /api/v1/ControllerMoto

A API estará disponível em:
👉 https://localhost:7030/swagger

📑 Endpoints e Exemplos

🔹 Health Check

GET /api/v1/health/live

Response (200 OK)

{
  "status": "Healthy",
  "checks": [
    {
      "name": "self",
      "status": "Healthy"
    },
    {
      "name": "Oracle Database",
      "status": "Healthy"
    }
  ]
}


📋 Descrição:
Este endpoint foi adicionado na Sprint 4 e tem como objetivo verificar a integridade da aplicação e a conexão com o banco Oracle, 
permitindo que o time de DevOps e QA monitore o status do sistema de forma rápida.

<img width="433" height="343" alt="image" src="https://github.com/user-attachments/assets/fbcb378b-b7aa-4cfe-974a-cc5341caa753" />

🔹 Admin

Endpoints protegidos por JWT. Enviar o header:
Authorization: Bearer {seu_token}

Autenticação / Token

POST /api/Admin/login

Gestão de Usuários (Admin-only)
POST /api/v1/Auth/login
{
    "username": "admin",
    "password": "12345"
}

POST /api/Admin/usuarios

<img width="1748" height="1147" alt="image" src="https://github.com/user-attachments/assets/adf7e9c0-0500-4fdd-8ba0-3feec1ccdf1a" />

🔹 Moto

GET /api/v1/ControllerMoto

GET /api/v1/ControllerMoto/{id}

GET /api/v1/ControllerMoto/porPlaca?placa=ABC1234

PUT /api/v1/ControllerMoto/{id}

DELETE /api/v1/ControllerMoto/{id}

POST /api/v1/ControllerMoto

Request (POST)

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

<img width="1769" height="628" alt="image" src="https://github.com/user-attachments/assets/2b8a342a-c3d4-4abb-ab87-a673710d0f26" />


🔹 Pátio

GET /api/v1/ControllerPatio

GET /api/v1/ControllerPatio/{id}

GET /api/v1/ControllerPatio/porNome?nome=Pátio

PUT /api/v1/ControllerPatio/{id}

DELETE /api/v1/ControllerPatio/{id}

POST /api/v1/ControllerPatio

Request (POST)

{
  "idFilial": 1,
  "nome": "Pátio Central SP",
  "areaM2": 500,
  "capacidade": 50,
  "ativo": "S"
}

<img width="1767" height="558" alt="image" src="https://github.com/user-attachments/assets/000bbb01-38e0-403c-8dae-e51477300053" />


🔹 Zona de Pátio

GET /api/v1/ControllerZonaPatio

GET /api/v1/ControllerZonaPatio/{id}

GET /api/v1/ControllerZonaPatio/porNome?nomeZona=Zona A

PUT /api/v1/ControllerZonaPatio/{id}

DELETE /api/v1/ControllerZonaPatio/{id}

POST /api/v1/ControllerZonaPatio

Request (POST)

{
  "idPatio": 1,
  "nomeZona": "Zona A",
  "tipoZona": "ESTACIONAMENTO",
  "capacidade": 20
}

<img width="1766" height="547" alt="image" src="https://github.com/user-attachments/assets/6307ffa7-fc66-4173-a0a4-0e46521dd494" />


🔹 Endereço

GET /api/v1/ControllerEndereco

GET /api/v1/ControllerEndereco/{id}

GET /api/v1/ControllerEndereco/porCidade?cidade=São Paulo

PUT /api/v1/ControllerEndereco/{id}

DELETE /api/v1/ControllerEndereco/{id}

POST /api/v1/ControllerEndereco

Request (POST)

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

<img width="1742" height="1258" alt="image" src="https://github.com/user-attachments/assets/c62680bb-d713-4dd9-a7a9-9db91440a50c" />


🔹 Filial

GET /api/v1/ControllerFilial

GET /api/v1/ControllerFilial/{id}

GET /api/v1/ControllerFilial/porNome?nome=SP

PUT /api/v1/ControllerFilial/{id}

DELETE /api/v1/ControllerFilial/{id}

POST /api/v1/ControllerFilial

Request (POST)

{
  "nome": "Mottu São Paulo",
  "cnpj": "98765432000777",
  "telefone": "(11)95439-8488",
  "email": "sp@mottu.com.br",
  "ativo": "S",
  "idEndereco": 1
}

<img width="1772" height="580" alt="image" src="https://github.com/user-attachments/assets/f43de448-280a-4382-ad83-888d2ae26e1f" />


🔹 Histórico de Localização

GET /api/v1/ControllerHistoricoLocalizacao

GET /api/v1/ControllerHistoricoLocalizacao/{id}

PUT /api/v1/ControllerHistoricoLocalizacao/{id}

GET /api/v1/ControllerHistoricoLocalizacao/porIdMoto?idMoto=1

POST /api/v1/ControllerHistoricoLocalizacao

DELETE /api/v1/ControllerHistoricoLocalizacao/{id}

Request (POST)

{
  "idMoto": 1,
  "idMotociclista": 1,
  "idZonaPatio": 1,
  "dataHoraSaida": "2025-05-01T09:00:00",
  "dataHoraEntrada": "2025-05-02T18:00:00",
  "kmRodados": 120.5,
  "idStatusOperacao": 1
}

<img width="1759" height="716" alt="image" src="https://github.com/user-attachments/assets/d102abe5-6b31-4c7b-adef-2502472ddfe6" />


🔹 Localização Moto

GET /api/v1/ControllerLocalizacaoMoto

GET /api/v1/ControllerLocalizacaoMoto/{id}

GET /api/v1/ControllerLocalizacaoMoto/porIdMoto?idMoto=1

PUT /api/v1/ControllerLocalizacaoMoto/{id}

DELETE /api/v1/ControllerLocalizacaoMoto/{id}

POST /api/v1/ControllerLocalizacaoMoto

Request (POST)

{
  "idMoto": 1,
  "idZona": 1,
  "dataHoraEntrada": "2025-05-01T08:00:00",
  "dataHoraSaida": null
}

<img width="1779" height="542" alt="image" src="https://github.com/user-attachments/assets/2ea8528e-af45-4067-9f1f-944e566c4831" />


🔹 Motociclista

GET /api/v1/ControllerMotociclista

GET /api/v1/ControllerMotociclista/{id}

GET /api/v1/ControllerMotociclista/porNome?nome=João

PUT /api/v1/ControllerMotociclista/{id}

DELETE /api/v1/ControllerMotociclista/{id}

POST /api/v1/ControllerMotociclista

Request (POST)

{
  "nome": "João da Silva",
  "cpf": "12345678902",
  "cnh": "SP12345777",
  "dataValidadeCnh": "2030-01-01T00:00:00",
  "telefone": "(11)98765-4333",
  "email": "joaodasilva@email.com",
  "dataCadastro": "2025-04-25T00:00:00",
  "ativo": "S",
  "idEndereco": 1
}

<img width="1768" height="605" alt="image" src="https://github.com/user-attachments/assets/a63aa21e-35e1-4288-b395-9792c9990236" />


🔹 Nota Fiscal

GET /api/v1/ControllerNotaFiscal

GET /api/v1/ControllerNotaFiscal/{id}

GET /api/v1/ControllerNotaFiscal/porNumero?numero=12345

PUT /api/v1/ControllerNotaFiscal/{id}

DELETE /api/v1/ControllerNotaFiscal/{id}

POST /api/v1/ControllerNotaFiscal

Request (POST)

{
  "numero": "12348",
  "serie": "1",
  "modelo": "55",
  "chaveAcesso": "35190304552144000125550010012345678901234570",
  "dataEmissao": "2023-03-01T00:00:00",
  "valorTotal": 150000,
  "fornecedor": "Mottu Motors",
  "cnpjFornecedor": "12345678000199"
}

<img width="1774" height="619" alt="image" src="https://github.com/user-attachments/assets/8d5f015f-a64f-461a-9223-91ef60b23d4d" />


🔹 Status Moto

GET /api/v1/ControllerStatusMoto

GET /api/v1/ControllerStatusMoto/{id}

GET /api/v1/ControllerStatusMoto/porNome?nome=Disponível

PUT /api/v1/ControllerStatusMoto/{id}

DELETE /api/v1/ControllerStatusMoto/{id}

POST /api/v1/ControllerStatusMoto

Request (POST)

{
  "id": 9,
  "descricao": "Disponível",
  "disponivel": "S"
}

<img width="1772" height="465" alt="image" src="https://github.com/user-attachments/assets/d81c3225-2992-49fb-8508-7a6b0751a197" />


🔹 Status Operação

GET /api/v1/ControllerStatusOperacao

GET /api/v1/ControllerStatusOperacao/{id}

GET /api/v1/ControllerStatusOperacao/porDescricao?descricao=Locação

PUT /api/v1/ControllerStatusOperacao/{id}

DELETE /api/v1/ControllerStatusOperacao/{id}

POST /api/v1/ControllerStatusOperacao

Request (POST)

{
  "descricao": "Locação",
  "tipoMovimentacao": "SAIDA"
}

🔹 Tipo Moto

GET /api/v1/ControllerTipoMoto

GET /api/v1/ControllerTipoMoto/{id}

GET /api/v1/ControllerTipoMoto/porDescricao

PUT /api/v1/ControllerTipoMoto{id}

DELETE /api/v1/ControllerTipoMoto{id}

POST /api/v1/ControllerTipoMoto

Request (POST)

{
  "descricao": "Mottu Super Sport",
  "categoria": "Super Esportiva"
}

<img width="1776" height="506" alt="image" src="https://github.com/user-attachments/assets/5a5aea58-eb2f-43c0-b66d-7697f45bee2d" />


✅ Requisitos Atendidos

CRUD completo para todas as entidades

Paginação nas listagens

Swagger com exemplos de requests/responses

DTOs

HATEOAS nos endpoints principais

Validações e tratamento de erros

Conexão com Oracle DB

Autenticação JWT

Versionamento da API

Endpoint de Health Check

Endpoint com ML.NET

Testes automatizados com xUnit

Rate Limiting e Response Compression

🧪 Testes Automatizados (xUnit)

Os testes automatizados foram criados com o framework xUnit, cobrindo a lógica principal e os endpoints críticos da aplicação.

Principais Tipos de Teste:

✅ Testes unitários para regras de negócio (serviços e validações)

✅ Testes de integração usando WebApplicationFactory

✅ Testes com métodos assíncronos (async/await)

✅ Uso de Assert.IsType e Assert.Contains

✅ Estrutura de resposta encapsulada na propriedade Data

✅ Tratamento seguro de UrlHelper nulo
