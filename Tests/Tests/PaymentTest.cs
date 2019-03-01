using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cashflow.Api;
using FinanceApi.Infra;
using FinanceApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Cashflow.Tests
{
  [TestClass]
  public class PaymentTest : BaseTest
  {
    [TestMethod]
    public void GetFuturePayments()
    {
      // var response = Client.GetAsync("/api/Payment/FuturePayments").Result;
      // var statusCode = (int)response.StatusCode;
      // Assert.AreEqual(200, statusCode);
    }
  }
}