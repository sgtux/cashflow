using System;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Services
{
    public abstract class BaseService
    {
        public DateTime CurrentDate => Utils.CurrentDate;
    }
}