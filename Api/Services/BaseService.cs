using System;

namespace Cashflow.Api.Services
{
    public abstract class BaseService
    {
        public DateTime CurrentDate => DateTime.Now;
    }
}