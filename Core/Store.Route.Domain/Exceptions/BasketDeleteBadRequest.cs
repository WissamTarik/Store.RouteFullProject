using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Domain.Exceptions
{
    public class BasketDeleteBadRequest():BadRequestException($"Invalid operation when delete basket")
    {
    }
}
