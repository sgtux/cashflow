using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Cashflow.Api.Models;
using Cashflow.Tests.Config;
using Cashflow.Tests.Mocks;
using Cashflow.Tests.TestModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests.Tests
{
    [TestClass]
    public abstract class BaseControllerTest
    {
        private static WebApplicationFactory<TestStartup> _factory;

        public static void Init()
        {
            if (_factory == null)
                _factory = new WebApplicationTest();
        }

        protected void TestErrors(object inputModel, ApiResultModel model, string error = null)
        {
            bool ok = true;

            if (model.Errors == null)
                model.Errors = new List<string>();

            if ((string.IsNullOrEmpty(error) && model.Errors.Any())
                || !string.IsNullOrEmpty(error) && !model.Errors.Contains(error))
                ok = false;

            var obtained = string.Join(';', model.Errors);

            Assert.IsTrue(ok, $"Expected: '{error}' - Obtained: '{obtained}' - Input: '{JsonSerializer.Serialize(inputModel)}'");
        }

        protected async Task<ApiResultDataModel<T>> Post<T>(string url, object body, int? userId = null)
        {
            var content = await Send(url, HttpMethod.Post, body, userId);
            return JsonSerializer.Deserialize<ApiResultDataModel<T>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
        }

        protected async Task<ApiResultModel> Post(string url, object body, int? userId = null)
        {
            var content = await Send(url, HttpMethod.Post, body, userId);
            return JsonSerializer.Deserialize<ApiResultModel>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
        }

        protected async Task<ApiResultDataModel<T>> Put<T>(string url, object body, int? userId = null)
        {
            var content = await Send(url, HttpMethod.Put, body, userId);
            return JsonSerializer.Deserialize<ApiResultDataModel<T>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
        }

        protected async Task<ApiResultModel> Put(string url, object body, int? userId = null)
        {
            var content = await Send(url, HttpMethod.Put, body, userId);
            return JsonSerializer.Deserialize<ApiResultModel>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        protected async Task<ApiResultDataModel<T>> Delete<T>(string url, int? userId = null)
        {
            var content = await Send(url, HttpMethod.Delete, null, userId);
            return JsonSerializer.Deserialize<ApiResultDataModel<T>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
        }

        protected async Task<ApiResultModel> Delete(string url, int? userId = null)
        {
            var content = await Send(url, HttpMethod.Delete, null, userId);
            return JsonSerializer.Deserialize<ApiResultModel>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); ;
        }

        protected async Task<ApiResultDataModel<T>> Get<T>(string url, int? userId = null)
        {
            var content = await Send(url, HttpMethod.Get, null, userId);
            return JsonSerializer.Deserialize<ApiResultDataModel<T>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        private async Task<string> Send(string url, HttpMethod method, object body, int? userId = null)
        {
            var client = _factory.CreateClient();

            string token = string.Empty;
            if (userId.HasValue)
                token = await GetToken(userId.Value);

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage response = null;
            StringContent requestContent = body == null ? null : new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            if (method == HttpMethod.Post)
                response = await client.PostAsync(url, requestContent);
            else if (method == HttpMethod.Put)
                response = await client.PutAsync(url, requestContent);
            else if (method == HttpMethod.Delete)
                response = await client.DeleteAsync(url);
            else
                response = await client.GetAsync(url);

            Trace.WriteLine($"Method: {method} - Url: {url} - StatusCode: {response.StatusCode}");

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new Exception(JsonSerializer.Serialize(response));

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetToken(int userId)
        {
            var client = _factory.CreateClient();

            var user = new AccountModel()
            {
                NickName = $"User{userId}",
                Password = "123"
            };

            StringContent requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/token", requestContent);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Invalid UserId {userId}");

            var accountData = JsonSerializer.Deserialize<ApiResultDataModel<AccountResultModel>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return accountData.Data.Token;
        }
    }
}