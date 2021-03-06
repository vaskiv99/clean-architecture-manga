namespace WebApi.UseCases.V1.Register
{
    using System.Collections.Generic;
    using Application.Boundaries.Register;
    using Microsoft.AspNetCore.Mvc;
    using WebApi.ViewModels;

    public sealed class RegisterPresenter : IOutputPort
    {
        public IActionResult ViewModel { get; private set; }

        public void CustomerAlreadyRegistered(string message)
        {
            ViewModel = new BadRequestObjectResult(message);
        }

        public void Standard(RegisterOutput output)
        {
            var transactions = new List<TransactionModel>();

            foreach (var item in output.Account.Transactions)
            {
                var transaction = new TransactionModel(
                    item.Amount,
                    item.Description,
                    item.TransactionDate);

                transactions.Add(transaction);
            }

            var account = new AccountDetailsModel(
                output.Account.AccountId,
                output.Account.CurrentBalance,
                transactions);

            var accounts = new List<AccountDetailsModel>();
            accounts.Add(account);

            var registerResponse = new RegisterResponse(
                output.Customer.CustomerId,
                output.Customer.SSN,
                output.Customer.Name,
                accounts);

            ViewModel = new CreatedAtRouteResult(
                "GetCustomer",
                new
                {
                    customerId = registerResponse.CustomerId,
                    version = "1.0",
                },
                registerResponse);
        }
    }
}