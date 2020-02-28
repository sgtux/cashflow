using System;
using System.Linq;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
  [TestClass]
  public abstract class BaseTest
  {
    protected void AssertExceptionMessage(Action action, string message = null)
    {
      try
      {
        action.Invoke();
        Assert.AreEqual(message, null);
      }
      catch (ValidateException ex)
      {
        if (message == null)
          throw ex;
        Assert.AreEqual(message, ex.Message);
      }
    }

    protected void HasNotifications(ResultModel model, params string[] notifications)
    {
      bool has = true;
      var expected = "";

      if (notifications == null || notifications.Length == 0)
      {
        has = !model.Notifications.Any();
      }
      else
      {
        expected = string.Join(',', notifications);
        foreach (var i in notifications)
          if (!model.Notifications.Contains(i))
            has = false;
      }

      var obtained = string.Join(',', model.Notifications);

      Assert.IsTrue(has, $"Expected: {expected} - Obtained: {obtained}");
    }
  }
}