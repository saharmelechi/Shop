using Microsoft.AspNetCore.Http;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsStore
{
     static public class Globals
    {
        public const string  ADMIN_SESSION_KEY = "Admin";
        public const string  USER_SESSION_KEY = "User";
        public const string  CART_SESSION_KEY = "Cart";
        private const string STOREMLFILE = "storeModelML.txt";


        public static PredictionEngine<ProductEntry, Copurchase_prediction> predictionengine { get; private set; }

        static public string getConnectedUser(ISession session) {
            return session.GetString(USER_SESSION_KEY);
        }

        static public bool isUserConnected(ISession session)
        {
            return !String.IsNullOrEmpty(getConnectedUser(session));
        }
         
        static public bool isAdminConnected(ISession session)
        {
            return !String.IsNullOrEmpty(session.GetString(ADMIN_SESSION_KEY));
        }

        static public void InitiateML()
        {
            // EXAMPLE FROM HERE: https://github.com/dotnet/machinelearning-samples/tree/master/samples/csharp/getting-started/MatrixFactorization_ProductRecommendation

            //STEP 1: Create MLContext to be shared across the model creation workflow objects 
            MLContext MLContext = new MLContext();

            ITransformer model;

            // Check if we need to retrain
            if (File.Exists(STOREMLFILE))
            {
                // When you load the model, it's a transformer.
                using (var stream = File.OpenRead(STOREMLFILE))
                    model = MLContext.Model.Load(stream);
            }
            // We need to traind all the data again
            else
            {


            //STEP 2: Create a reader by defining the schema for reading the product co-purchase dataset
            //        Do remember to replace amazon0302.txt with dataset from 
            https://snap.stanford.edu/data/amazon0302.html
                var reader = MLContext.Data.CreateTextReader(new TextLoader.Arguments()
                {
                    Separator = "tab",
                    HasHeader = true,
                    Column = new[]
                    {
                new TextLoader.Column("Label", DataKind.R4, 0),
                new TextLoader.Column("ProductID", DataKind.U4, new [] { new TextLoader.Range(0) }, new KeyRange(0, 262110)),
                new TextLoader.Column("CoPurchaseProductID", DataKind.U4, new [] { new TextLoader.Range(1) }, new KeyRange(0, 262110))
            }
                });

                //STEP 3: Read the training data which will be used to train the movie recommendation model
                var traindata = reader.Read(new MultiFileSource("Amazon0302.txt"));


                //STEP 4: Your data is already encoded so all you need to do is call the MatrixFactorization Trainer with a few extra hyperparameters:
                //        LossFunction, Alpa, Lambda and a few others like K and C as shown below. 
                var est = MLContext.Recommendation().Trainers.MatrixFactorization("ProductID", "CoPurchaseProductID",
                                             labelColumn: "Label",
                                             advancedSettings: s =>
                                             {
                                                 s.LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass;
                                                 s.Alpha = 0.01;
                                                 s.Lambda = 0.025;
                                                 // For better results use the following parameters
                                                 //s.K = 100;
                                                 //s.C = 0.00001;
                                             });

                //STEP 5: Train the model fitting to the DataSet
                //Please add Amazon0302.txt dataset from https://snap.stanford.edu/data/amazon0302.html to Data folder if FileNotFoundException is thrown.
                model = est.Fit(traindata);

                using (var stream = File.Create(STOREMLFILE))
                {
                    // Saving and loading happens to 'dynamic' models.
                    MLContext.Model.Save(model, stream);
                }

            }

            //STEP 6: Create prediction engine and predict the score for Product 63 being co-purchased with Product 3.
            //        The higher the score the higher the probability for this particular productID being co-purchased 
            predictionengine = model.CreatePredictionEngine<ProductEntry, Copurchase_prediction>(MLContext);

            // MOVE THIS TO Prosuct
            var prediction = predictionengine.Predict(
                new ProductEntry()
                {
                    ProductID = 3,
                    CoPurchaseProductID = 63
                });
        }


    }


    public class Copurchase_prediction
    {
        public float Score { get; set; }
    }

    public class ProductEntry
    {
        [KeyType(Contiguous = true, Count = 262111, Min = 0)]
        public uint ProductID { get; set; }

        [KeyType(Contiguous = true, Count = 262111, Min = 0)]
        public uint CoPurchaseProductID { get; set; }
    }
}
