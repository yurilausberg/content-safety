﻿
using DotNetEnv;

namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class ContentSafetySampleAnalyzeText
    {
        public static void AnalyzeText()
        {
            // Create Azure AI ContentSafety Client
            Env.Load();

            string endpoint = Environment.GetEnvironmentVariable("CONTENT_SAFETY_ENDPOINT") ?? throw new InvalidOperationException("Environment variable 'CONTENT_SAFETY_ENDPOINT' is not set.");
            string key = Environment.GetEnvironmentVariable("CONTENT_SAFETY_KEY") ?? throw new InvalidOperationException("Environment variable 'CONTENT_SAFETY_KEY' is not set.");

            ContentSafetyClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

            // Example: analyze text without blocklist

            string text = Console.ReadLine() ?? string.Empty;

            var request = new AnalyzeTextOptions(text);

            Response<AnalyzeTextResult> response;
            try
            {
                response = client.AnalyzeText(request);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Analyze text failed.\nStatus code: {0}, Error code: {1}, Error message: {2}", ex.Status, ex.ErrorCode, ex.Message);
                throw;
            }

            Console.WriteLine("\nAnalyze text succeeded:");
            Console.WriteLine("Hate severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Hate)?.Severity ?? 0);
            Console.WriteLine("SelfHarm severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.SelfHarm)?.Severity ?? 0);
            Console.WriteLine("Sexual severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Sexual)?.Severity ?? 0);
            Console.WriteLine("Violence severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Violence)?.Severity ?? 0);
        }

        static void Main()
        {
            AnalyzeText();
        }
    }
}
