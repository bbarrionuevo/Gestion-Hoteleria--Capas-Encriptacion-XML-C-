using Abstraccion;
using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class BLLUsuario: IGestor<BEUsuario>
    {
        DALUsuario DALUsuario;
        public BLLUsuario()
        {
            DALUsuario = new DALUsuario();
        }
        public bool ValidarCredenciales(BEUsuario BEUsu)
        {
            if (DALUsuario.ValidarCredenciales(BEUsu)) { return true; }
            else { return false; }
        }

        public bool Baja(BEUsuario Objeto)
        {
            throw new NotImplementedException();
        }

        public bool Guardar(BEUsuario Objeto)
        {
            return DALUsuario.Guardar(Objeto);
        }

        public BEUsuario ListarObjeto(BEUsuario Objeto)
        {
            return DALUsuario.ListarObjeto(Objeto);
        }

        public List<BEUsuario> ListarTodo()
        {
           
            return DALUsuario.ListarTodo();
        }
        public bool GuardarPermisos(BEUsuario oBEUsu)
        {
            
            return DALUsuario.GuardarPermisos(oBEUsu);
        }
        public bool BorrarPermisos(BEUsuario oBEUsu)
        {

            return DALUsuario.BorrarPermisos(oBEUsu);
        }
    }
}
