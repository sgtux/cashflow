using System;
using Cashflow.Api.Utils;

namespace Cashflow.Api.Services
{
    public abstract class BaseService
    {
        public BaseService() => CurrentDate = DateTimeUtils.CurrentDate;

        public DateTime CurrentDate { get; }
    }
}