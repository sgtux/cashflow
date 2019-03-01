using System;
using FinanceApi.Shared;
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
        Assert.AreEqual(null, message);
      }
      catch (ValidateException ex)
      {
        if (message == null)
          throw ex;
        Assert.AreEqual(message, ex.Message);
      }
    }
  }
}