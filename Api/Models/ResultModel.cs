using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Cashflow.Api.Models
{
    public class ResultModel
    {
        public List<string> Notifications { get; private set; }

        public ResultModel() => Notifications = new List<string>();

        public void AddNotification(string notification) => Notifications.Add(notification);

        public void AddNotification(List<string> notifications) => Notifications.AddRange(notifications);

        public void AddNotification(IList<ValidationFailure> errors) => Notifications.AddRange(errors.Select(p => p.ErrorMessage));

        public bool IsValid => !Notifications.Any();
    }
}