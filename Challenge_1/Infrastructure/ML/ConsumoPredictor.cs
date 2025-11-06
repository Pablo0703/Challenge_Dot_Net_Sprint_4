using Microsoft.ML;
using Microsoft.ML.Data;

namespace Challenge_1.Infrastructure.ML
{
    public class ConsumoInput
    {
        [LoadColumn(0)] public float Cilindrada { get; set; }
        [LoadColumn(1)] public float Peso { get; set; }
        [LoadColumn(2)] public float VelocidadeMedia { get; set; }
        [LoadColumn(3)] public float Consumo { get; set; } // Valor real para treino
    }

    public class ConsumoOutput
    {
        [ColumnName("Score")]
        public float ConsumoEstimado { get; set; }
    }

    public class ConsumoPredictor
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private List<ConsumoInput> _data; // Dados de treino dinâmicos

        public ConsumoPredictor()
        {
            _mlContext = new MLContext();

            // Dados iniciais simulados
            _data = new List<ConsumoInput>
            {
                new() { Cilindrada = 150, Peso = 130, VelocidadeMedia = 60, Consumo = 35 },
                new() { Cilindrada = 250, Peso = 180, VelocidadeMedia = 100, Consumo = 28 },
                new() { Cilindrada = 125, Peso = 120, VelocidadeMedia = 50, Consumo = 38 },
                new() { Cilindrada = 300, Peso = 200, VelocidadeMedia = 110, Consumo = 25 },
                new() { Cilindrada = 160, Peso = 140, VelocidadeMedia = 80, Consumo = 30 },
            };

            _model = TreinarModelo();
        }

        // Método para treinar o modelo a partir dos dados atuais
        private ITransformer TreinarModelo()
        {
            var trainDataView = _mlContext.Data.LoadFromEnumerable(_data);

            var pipeline = _mlContext.Transforms.Concatenate("Features",
                    nameof(ConsumoInput.Cilindrada),
                    nameof(ConsumoInput.Peso),
                    nameof(ConsumoInput.VelocidadeMedia))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Consumo", maximumNumberOfIterations: 100));

            return pipeline.Fit(trainDataView);
        }

        // 🔹 Faz uma predição
        public float PreverConsumo(float cilindrada, float peso, float velocidadeMedia)
        {
            var predEngine = _mlContext.Model.CreatePredictionEngine<ConsumoInput, ConsumoOutput>(_model);

            var input = new ConsumoInput
            {
                Cilindrada = cilindrada,
                Peso = peso,
                VelocidadeMedia = velocidadeMedia
            };

            return predEngine.Predict(input).ConsumoEstimado;
        }

        // 🔹 Adiciona novos dados e re-treina
        public void AdicionarDadosTreino(ConsumoInput novoDado)
        {
            _data.Add(novoDado);
            _model = TreinarModelo();
        }
    }
}
