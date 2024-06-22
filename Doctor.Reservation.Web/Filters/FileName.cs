using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Doctor.Reservation.Domain.Exceptions;

namespace Doctor.Reservation.Web.Filters;
public class GlobalExceptionFilters : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled) return;

        var exception = context.Exception;

        int statusCode;

        switch (true)
        {
            case bool _ when exception is DomainException:
                statusCode = (int)HttpStatusCode.BadRequest;
                break;


            case bool _ when exception is InvalidOperationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                break;


            case bool _ when exception is KeyNotFoundException:
                statusCode = (int)HttpStatusCode.BadRequest;
                break;


            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Result = new ObjectResult(exception.Message) { StatusCode = statusCode };
    }
}