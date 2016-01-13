using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace OCR.Prediction
{
    public interface IPredict
    {
        IPredictImage PredictModel(IScanImage scanImage, IAlphabet alphabet, PredictOptions options);
    }

    public class Predict : IPredict
    {
        private PredictOptions _options;

        public IPredictImage PredictModel(IScanImage scanImage, IAlphabet alphabet, PredictOptions options)
        {
            _options = options;
            var blobs = new List<IBlob>();

            // Get the model params.
            var thetas = GetModelParamsFromFile();

            // Loop thru all blobs and predict.
            foreach (var blob in scanImage.Blobs)
            {
                // Get the blob pixels.                
                Vector xs = blob.GetPixels(scanImage.Image, options.ExtractedBackColor, options.ExportSize);

                // Get the model value (this is what to be used in the Sigmoid function).
                var v = (thetas * xs);

                // This is for finding the maximum value of all letters predictions (1-vs-all), so
                // we know what letter to choose.
                var max = new double[3];
                int[] maxIndex = { -1, -1, -1 };

                // Loop thru the values
                for (int i = 0; i < v.Count; i++)
                {
                    // Get the final model prediction (Sigmoid).
                    v[i] = SpecialFunctions.Logistic(v[i].Real);

                    // Check if this prediction is in the "top 3".
                    for (int j = 0; j < max.Length; j++)
                    {
                        if (v[i].Real > max[j])
                        {
                            max[j] = v[i].Real;
                            maxIndex[j] = i;

                            // We want to kepp max array sorted, so once we found a value
                            // it is bigger than we stop.
                            break;
                        }
                    }
                }
                
                var b = (IBlob)blob.Clone();
                b.Title = alphabet.Data[maxIndex[0]];

                blobs.Add(b);
            }

            var collection = blobs.OrderBy(b => b.RowIndex)
                .ThenBy(b => b.Left)
                .GroupBy(g => g.RowIndex)
                .ToDictionary(g => g.Key, s => s.Select(g=>g).ToList());

            return new PredictImage(scanImage.Image, collection);
        }

        private Matrix GetModelParamsFromFile()
        {
            // The model thetas (this is an intermidiate dictionary before we convert it to matrix).
            var allThetas = new Dictionary<int, List<double>>();

            // Open the model file.
            using (var sr = new StringReader(_options.Model))
            {
                int rowIndex = 0;

                // Read the first line and loop till the end.
                string line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    // Create a new row in the dictionary.
                    allThetas.Add(rowIndex, new List<double>());

                    // Split the values.
                    string[] thetas = line.TrimStart().Split(' ');

                    // Loop thru all values and add them.
                    foreach (string currTheta in thetas)
                    {
                        // This Parse is a potential exception if there is not valid number there, ok for now...
                        double theta = double.Parse(currTheta, CultureInfo.InvariantCulture);
                        allThetas[rowIndex].Add(theta);
                    }

                    // Get the next line and move to the next row index.
                    line = sr.ReadLine();
                    ++rowIndex;
                }
            }

            // Create the thetas matrix.
            Matrix thetasM = new DenseMatrix(allThetas.Keys.Count, allThetas[0].Count);

            // Loop thru all dictionary vales (ordered by rows) and add it to the matrix.
            foreach (int row in allThetas.Keys.OrderBy(key => key))
            {
                for (int i = 0; i < allThetas[row].Count; i++)
                {
                    thetasM[row, i] = allThetas[row][i];
                }
            }

            return thetasM;
        }       
    }
}
