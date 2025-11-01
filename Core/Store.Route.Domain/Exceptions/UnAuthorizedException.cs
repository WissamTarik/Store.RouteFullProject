using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Domain.Exceptions
{
    public class UnAuthorizedException():Exception("Invalid email or password !!")
    {
    }
}
