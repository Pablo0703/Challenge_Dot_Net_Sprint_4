using Microsoft.ML;
using Microsoft.ML.Data;

namespace Challenge_1.Infrastructure.ML
{
    public class ModeloInput
    {
        [LoadColumn(0)] public float MotoId { get; set; }
        [LoadColumn(1)] public float UsuarioId { get; set; }
    }

    public class ModeloOutput
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }

    public class ModeloRecomendacao
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _modelo;
        private readonly PredictionEngine<ModeloInput, ModeloOutput> _predEngine;

        public ModeloRecomendacao()
        {
            _mlContext = new MLContext();

            string modeloPath = Path.Combine(AppContext.BaseDirectory, "Infrastructure", "ML", "modelo_recomendacao.zip");

            if (!File.Exists(modeloPath))
                throw new FileNotFoundException($"❌ Modelo ML.NET não encontrado em: {modeloPath}");

            // Carrega o modelo
            DataViewSchema schema;
            _modelo = _mlContext.Model.Load(modeloPath, out schema);
            _predEngine = _mlContext.Model.CreatePredictionEngine<ModeloInput, ModeloOutput>(_modelo);
        }

        public float RecomendarMoto(int usuarioId, int motoId)
        {
            var input = new ModeloInput
            {
                UsuarioId = usuarioId,
                MotoId = motoId
            };

            var prediction = _predEngine.Predict(input);
            return prediction.Score;
        }
    }
}
