using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Domain.Exceptions
{
    public class BasketNotFoundException(string id):
        NotFoundException($"Basket with {id} is not found")
    {
    }
}
