using System;

namespace Cashflow.Api.Models
{
  public class ResultDataModel<T> : ResultModel
  {
    public T Data { get; set; }

    public ResultDataModel()
    {
      Data = (T)Activator.CreateInstance(typeof(T));
    }

    public ResultDataModel(T data)
    {
      Data = data;
    }
  }
}