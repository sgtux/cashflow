using System;
using System.Collections.Generic;
using System.Linq;
using FinanceApi.Infra;
using FinanceApi.Infra.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
  /// <summary>
  /// Pagamentos
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  public class PaymentController : BaseController
  {

    private AppDbContext _context;

    /// <summary>
    /// Construtor
    /// </summary>    
    public PaymentController(AppDbContext context) => _context = context;

    /// <summary>
    /// Obter os pagamentos usuário logado
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public List<Payment> Get()
    {
      return _context.Payment.Where(p => p.UserId == UserId).ToList();
    }

    /// <summary>
    /// Inserir um novo pagamento para o usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPost]
    public void Post([FromBody]Payment payment)
    {
      ValidatePayment(payment);
      payment.UserId = UserId;
      _context.Add(payment);
      _context.SaveChanges();
    }

    /// <summary>
    /// Atualizar um pagamento do usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPut]
    public void Put([FromBody]Payment payment)
    {
      ValidatePayment(payment);
      var paymentDb = _context.Payment.FirstOrDefault(p => p.Id == payment.Id && p.UserId == UserId);
      if (paymentDb is null)
        ThrowValidationError("Pagamento não localizado.");

      payment.UserId = UserId;
      paymentDb.Description = payment.Description;
      paymentDb.Cost = payment.Cost;
      paymentDb.CreditCardId = payment.CreditCardId;
      paymentDb.Type = payment.Type;
      paymentDb.Plots = payment.Plots;
      _context.Update(paymentDb);
      _context.SaveChanges();
    }

    /// <summary>
    /// Remove um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var payment = _context.Payment.FirstOrDefault(p => p.Id == id && p.UserId == UserId);
      if (payment is null)
        ThrowValidationError("Pagamento não localizado.");

      _context.Remove(payment);
      _context.SaveChanges();
    }

    private void ValidatePayment(Payment payment)
    {
      if (payment is null)
        ThrowValidationError("Pagamento inválido.");

      if (string.IsNullOrEmpty(payment.Description))
        ThrowValidationError("A descrição é obrigatória.");

      if (payment.Cost <= 0)
        ThrowValidationError("O valor deve ser maior que Zero.");

      if (default(DateTime) == payment.FirstPayment)
        ThrowValidationError("A data do primeiro pagamento é obrigatória.");

      if (payment.PlotsPaid > payment.Plots)
        ThrowValidationError("O quantidade parcelas pagas não pode ser maior que o número de parcelas.");

      if (payment.Plots <= 0)
        ThrowValidationError("O pagamento deve ter pelo menos 1 parcela.");

      if (payment.CreditCardId.HasValue)
      {
        var card = _context.CreditCard.FirstOrDefault(p => p.Id == payment.CreditCardId.Value && p.UserId == UserId);
        if (card is null)
          ThrowValidationError("Cartão não localizado.");
      }
    }
  }
}