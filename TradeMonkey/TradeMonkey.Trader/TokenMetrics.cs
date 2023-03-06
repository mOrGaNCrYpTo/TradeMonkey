using Mapster;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using TradeMonkey.Data.Context;
using TradeMonkey.Data.Entity;

namespace TradeMonkey.DataCollector
{
    internal class TokenMetrics
    {
        private readonly string _apiKey = "tm-2244d8cf-af79-429c-9691-254076155698";
        private readonly string _connectionString = "Server=HP\\MFSQL;Database=TradeMonkey;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=false";
        private readonly Uri BaseUri = new Uri("https://api.tokenmetrics.com/v1");
        private HttpStatusCode _statusCode = HttpStatusCode.OK;

        public async Task UpsertTokensAsync(CancellationToken token = default)
        {
            try
            {
                string Url = $"{BaseUri}/Tokens";

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("api_key", _apiKey);

                    using (var request = new HttpRequestMessage(HttpMethod.Get, Url))
                    {
                        using (var response = httpClient.Send(request))
                        {
                            _statusCode = response.StatusCode;
                            response.EnsureSuccessStatusCode();

                            var content = await response.Content.ReadAsStringAsync();
                            if (content != null)
                            {
                                JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                };

                                var result = JsonSerializer.Deserialize<TokenResponse>(content, jsonOptions);

                                var options = new DbContextOptionsBuilder<TmDBContext>()
                                    .UseSqlServer(_connectionString)
                                    .Options;
                                try
                                {
                                    if (result.Data.Any())
                                        using (var dbContext = new TmDBContext(options))
                                        {
                                            dbContext.BulkInsert(result.Data, options =>
                                            {
                                                options.InsertIfNotExists = true;
                                                options.ColumnPrimaryKeyExpression = t => t.Token_Id;
                                            });
                                        }
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCorrelationDataAsync returned {_statusCode} with error: {ex.Message}");
            }
        }
    }
}