using System;
using Cashflow.Api.Utils;

namespace Cashflow.Api.Services
{
    public abstract class BaseService
    {
        public DateTime CurrentDate => DateTimeUtils.CurrentDate;
    }
}