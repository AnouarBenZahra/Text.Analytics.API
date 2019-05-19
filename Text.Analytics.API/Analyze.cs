using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace Text.Analytics.API
{
    public class Analyze
    {
        private const string SubscriptionKey = "...";

        public static async Task<List<string>> Result(MultiLanguageBatchInput multiLanguageBatchInput)
        {
            var credentials = new ApiKeyService(SubscriptionKey);
            var client = new TextAnalyticsClient(credentials)
            {
                //Replace 'westus' with the correct region for your Text Analytics subscription.
                Endpoint = "https://westus.api.cognitive.microsoft.com"
            };

            return await sentimentAnalysis(client, multiLanguageBatchInput);

        }
        private static async Task<List<string>> sentimentAnalysis(TextAnalyticsClient client, MultiLanguageBatchInput multiLanguageBatchInput)
        {
            List<string> results = new List<string>();
            var sentimentBatch = await client.SentimentAsync(false, multiLanguageBatchInput);

            // Printing sentiment results
            foreach (var document in sentimentBatch.Documents)
            {
                results.Add($"ID: {document.Id} , Sentiment Score: {document.Score:0.00}");
            }
            return results;
        }
    }
    class ApiKeyService : ServiceClientCredentials
    {
        private readonly string subscriptionKey;
        public ApiKeyService(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey;
        }
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            request.Headers.Add("Ocp-Apim-Subscription-Key", this.subscriptionKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
