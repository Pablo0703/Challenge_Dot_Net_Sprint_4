# Challenge 2025 - Advanced Business Development with .NET

## Grupo

* Nome: Pablo Lopes Doria de Andrade
* RM: 556834

* Nome: Vinicius Leopoldino de Oliveira
* RM: 557047

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


A API estará disponível em:
👉 https://localhost:7030/swagger

📑 Endpoints e Exemplos
🔹 Moto

GET /api/ControllerMoto

GET /api/ControllerMoto/{id}

GET /api/ControllerMoto/porPlaca?placa=ABC1234

POST /api/ControllerMoto

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

GET /api/ControllerPatio

GET /api/ControllerPatio/{id}

GET /api/ControllerPatio/porNome?nome=Pátio

POST /api/ControllerPatio

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

GET /api/ControllerZonaPatio

GET /api/ControllerZonaPatio/{id}

GET /api/ControllerZonaPatio/porNome?nomeZona=Zona A

POST /api/ControllerZonaPatio

Request (POST)

{
  "idPatio": 1,
  "nomeZona": "Zona A",
  "tipoZona": "ESTACIONAMENTO",
  "capacidade": 20
}

<img width="1766" height="547" alt="image" src="https://github.com/user-attachments/assets/6307ffa7-fc66-4173-a0a4-0e46521dd494" />


🔹 Endereço

GET /api/ControllerEndereco

GET /api/ControllerEndereco/{id}

GET /api/ControllerEndereco/porCidade?cidade=São Paulo

POST /api/ControllerEndereco

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

GET /api/ControllerFilial

GET /api/ControllerFilial/{id}

GET /api/ControllerFilial/porNome?nome=SP

POST /api/ControllerFilial

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

GET /api/ControllerHistoricoLocalizacao

GET /api/ControllerHistoricoLocalizacao/{id}

GET /api/ControllerHistoricoLocalizacao/porIdMoto?idMoto=1

POST /api/ControllerHistoricoLocalizacao

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

GET /api/ControllerLocalizacaoMoto

GET /api/ControllerLocalizacaoMoto/{id}

GET /api/ControllerLocalizacaoMoto/porIdMoto?idMoto=1

POST /api/ControllerLocalizacaoMoto

Request (POST)

{
  "idMoto": 1,
  "idZona": 1,
  "dataHoraEntrada": "2025-05-01T08:00:00",
  "dataHoraSaida": null
}

<img width="1779" height="542" alt="image" src="https://github.com/user-attachments/assets/2ea8528e-af45-4067-9f1f-944e566c4831" />


🔹 Motociclista

GET /api/ControllerMotociclista

GET /api/ControllerMotociclista/{id}

GET /api/ControllerMotociclista/porNome?nome=João

POST /api/ControllerMotociclista

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

GET /api/ControllerNotaFiscal

GET /api/ControllerNotaFiscal/{id}

GET /api/ControllerNotaFiscal/porNumero?numero=12345

POST /api/ControllerNotaFiscal

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

GET /api/ControllerStatusMoto

GET /api/ControllerStatusMoto/{id}

GET /api/ControllerStatusMoto/porNome?nome=Disponível

POST /api/ControllerStatusMoto

Request (POST)

{
  "id": 9,
  "descricao": "Disponível",
  "disponivel": "S"
}

<img width="1772" height="465" alt="image" src="https://github.com/user-attachments/assets/d81c3225-2992-49fb-8508-7a6b0751a197" />


🔹 Status Operação

GET /api/ControllerStatusOperacao

GET /api/ControllerStatusOperacao/{id}

GET /api/ControllerStatusOperacao/porDescricao?descricao=Locação

POST /api/ControllerStatusOperacao

Request (POST)

{
  "descricao": "Locação",
  "tipoMovimentacao": "SAIDA"
}

🔹 Tipo Moto

GET /api/TipoMoto

GET /api/TipoMoto/{id}

POST /api/TipoMoto

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

🧪 Testes

Para rodar os testes:

dotnet test
