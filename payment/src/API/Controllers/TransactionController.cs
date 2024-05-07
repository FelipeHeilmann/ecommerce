using Application.Transactions.MakePaymentRequest;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController : APIBaseController
{
    public TransactionController(ISender _sender) : base(_sender) { }

    
}
