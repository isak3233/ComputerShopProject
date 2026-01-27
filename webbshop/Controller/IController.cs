using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Controller
{
    public interface IController
    {
        public Task<IController> ActivateController();
    }
}
