using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraccion;

namespace BE
{
    public class BEClsCliente : BEClsPersona
    {
       
        
        public BEClsCliente() 
        {
          
        }
        public BEClsCliente(int id)
        {
            ID=id;
        }

    }
}

