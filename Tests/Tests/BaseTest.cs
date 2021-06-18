using System.Linq;
using Cashflow.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        protected void HasNotifications(ResultModel model, params string[] notifications)
        {
            bool has = true;
            var expected = "";

            if (notifications?.Any() ?? false)
            {
                expected = string.Join(';', notifications);
                foreach (var i in notifications)
                    if (!model.Notifications.Contains(i))
                        has = false;
            }
            else
                has = !model.Notifications.Any();

            var obtained = string.Join(';', model.Notifications);

            Assert.IsTrue(has, $"Expected: '{expected}' - Obtained: '{obtained}'");
        }
    }
}