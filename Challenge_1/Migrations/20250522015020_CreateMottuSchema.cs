using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Challenge1.Migrations
{
    /// <inheritdoc />
    public partial class CreateMottuSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOTTU_ENDERECO",
                columns: table => new
                {
                    IDENDERECO = table.Column<long>(name: "ID_ENDERECO", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    LOGRADOURO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    NUMERO = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    COMPLEMENTO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    BAIRRO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    CEP = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    CIDADE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ESTADO = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_ENDERECO", x => x.IDENDERECO);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_FILIAL",
                columns: table => new
                {
                    IDFILIAL = table.Column<long>(name: "ID_FILIAL", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CNPJ = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    ATIVO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: true),
                    IDENDERECO = table.Column<long>(name: "ID_ENDERECO", type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_FILIAL", x => x.IDFILIAL);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_HISTORICO_LOCALIZACAO",
                columns: table => new
                {
                    IDHISTORICO = table.Column<long>(name: "ID_HISTORICO", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IDMOTO = table.Column<long>(name: "ID_MOTO", type: "NUMBER(19)", nullable: false),
                    IDMOTOCICLISTA = table.Column<long>(name: "ID_MOTOCICLISTA", type: "NUMBER(19)", nullable: true),
                    IDZONAPATIO = table.Column<long>(name: "ID_ZONA_PATIO", type: "NUMBER(19)", nullable: true),
                    DATAHORASAIDA = table.Column<DateTime>(name: "DATA_HORA_SAIDA", type: "TIMESTAMP(7)", nullable: false),
                    DATAHORAENTRADA = table.Column<DateTime>(name: "DATA_HORA_ENTRADA", type: "TIMESTAMP(7)", nullable: true),
                    KMRODADOS = table.Column<decimal>(name: "KM_RODADOS", type: "DECIMAL(18, 2)", nullable: true),
                    IDSTATUSOPERACAO = table.Column<long>(name: "ID_STATUS_OPERACAO", type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_HISTORICO_LOCALIZACAO", x => x.IDHISTORICO);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_LOCALIZACAO_MOTO",
                columns: table => new
                {
                    IDLOCALIZACAO = table.Column<long>(name: "ID_LOCALIZACAO", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IDMOTO = table.Column<long>(name: "ID_MOTO", type: "NUMBER(19)", nullable: false),
                    IDZONA = table.Column<long>(name: "ID_ZONA", type: "NUMBER(19)", nullable: false),
                    DATAHORAENTRADA = table.Column<DateTime>(name: "DATA_HORA_ENTRADA", type: "TIMESTAMP(7)", nullable: false),
                    DATAHORASAIDA = table.Column<DateTime>(name: "DATA_HORA_SAIDA", type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_LOCALIZACAO_MOTO", x => x.IDLOCALIZACAO);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_MOTO",
                columns: table => new
                {
                    IDMOTO = table.Column<long>(name: "ID_MOTO", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IDTIPO = table.Column<long>(name: "ID_TIPO", type: "NUMBER(19)", nullable: false),
                    IDSTATUS = table.Column<long>(name: "ID_STATUS", type: "NUMBER(19)", nullable: false),
                    PLACA = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    MODELO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ANOFABRICACAO = table.Column<int>(name: "ANO_FABRICACAO", type: "NUMBER(10)", nullable: false),
                    ANOMODELO = table.Column<int>(name: "ANO_MODELO", type: "NUMBER(10)", nullable: false),
                    CHASSI = table.Column<string>(type: "NVARCHAR2(17)", maxLength: 17, nullable: false),
                    CILINDRADA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    COR = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DATAAQUISICAO = table.Column<DateTime>(name: "DATA_AQUISICAO", type: "TIMESTAMP(7)", nullable: false),
                    VALORAQUISICAO = table.Column<decimal>(name: "VALOR_AQUISICAO", type: "DECIMAL(18, 2)", nullable: false),
                    IDNF = table.Column<long>(name: "ID_NF", type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_MOTO", x => x.IDMOTO);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_MOTOCILCISTA",
                columns: table => new
                {
                    IDMOTOCICLISTA = table.Column<long>(name: "ID_MOTOCICLISTA", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    CNH = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DATAVALIDADECNH = table.Column<DateTime>(name: "DATA_VALIDADE_CNH", type: "TIMESTAMP(7)", nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    DATACADASTRO = table.Column<DateTime>(name: "DATA_CADASTRO", type: "TIMESTAMP(7)", nullable: false),
                    ATIVO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: true),
                    IDENDERECO = table.Column<long>(name: "ID_ENDERECO", type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_MOTOCILCISTA", x => x.IDMOTOCICLISTA);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_NF",
                columns: table => new
                {
                    IDNF = table.Column<long>(name: "ID_NF", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NUMERO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    SERIE = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: false),
                    MODELO = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    CHAVEACESSO = table.Column<string>(name: "CHAVE_ACESSO", type: "NVARCHAR2(44)", maxLength: 44, nullable: true),
                    DATAEMISSAO = table.Column<DateTime>(name: "DATA_EMISSAO", type: "TIMESTAMP(7)", nullable: false),
                    VALORTOTAL = table.Column<decimal>(name: "VALOR_TOTAL", type: "DECIMAL(18, 2)", nullable: false),
                    FORNECEDOR = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CNPJFORNECEDOR = table.Column<string>(name: "CNPJ_FORNECEDOR", type: "NVARCHAR2(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_NF", x => x.IDNF);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_PATIO",
                columns: table => new
                {
                    IDPATIO = table.Column<long>(name: "ID_PATIO", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IDFILIAL = table.Column<long>(name: "ID_FILIAL", type: "NUMBER(19)", nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    AREAM2 = table.Column<decimal>(name: "AREA_M2", type: "DECIMAL(18, 2)", nullable: true),
                    CAPACIDADE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ATIVO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_PATIO", x => x.IDPATIO);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_STATUS_MOTO",
                columns: table => new
                {
                    IDSTATUS = table.Column<long>(name: "ID_STATUS", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DISPONIVEL = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_STATUS_MOTO", x => x.IDSTATUS);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_STATUS_OPERACAO",
                columns: table => new
                {
                    IDSTATUSOPERACAO = table.Column<long>(name: "ID_STATUS_OPERACAO", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    TIPOMOVIMENTACAO = table.Column<string>(name: "TIPO_MOVIMENTACAO", type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_STATUS_OPERACAO", x => x.IDSTATUSOPERACAO);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_TIPO_MOTO",
                columns: table => new
                {
                    IDTIPO = table.Column<long>(name: "ID_TIPO", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    CATEGORIA = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_TIPO_MOTO", x => x.IDTIPO);
                });

            migrationBuilder.CreateTable(
                name: "MOTTU_ZONA_PATIO",
                columns: table => new
                {
                    IDZONA = table.Column<long>(name: "ID_ZONA", type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IDPATIO = table.Column<long>(name: "ID_PATIO", type: "NUMBER(19)", nullable: false),
                    NOMEZONA = table.Column<string>(name: "NOME_ZONA", type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    TIPOZONA = table.Column<string>(name: "TIPO_ZONA", type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    CAPACIDADE = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTTU_ZONA_PATIO", x => x.IDZONA);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOTTU_ENDERECO");

            migrationBuilder.DropTable(
                name: "MOTTU_FILIAL");

            migrationBuilder.DropTable(
                name: "MOTTU_HISTORICO_LOCALIZACAO");

            migrationBuilder.DropTable(
                name: "MOTTU_LOCALIZACAO_MOTO");

            migrationBuilder.DropTable(
                name: "MOTTU_MOTO");

            migrationBuilder.DropTable(
                name: "MOTTU_MOTOCILCISTA");

            migrationBuilder.DropTable(
                name: "MOTTU_NF");

            migrationBuilder.DropTable(
                name: "MOTTU_PATIO");

            migrationBuilder.DropTable(
                name: "MOTTU_STATUS_MOTO");

            migrationBuilder.DropTable(
                name: "MOTTU_STATUS_OPERACAO");

            migrationBuilder.DropTable(
                name: "MOTTU_TIPO_MOTO");

            migrationBuilder.DropTable(
                name: "MOTTU_ZONA_PATIO");
        }
    }
}
