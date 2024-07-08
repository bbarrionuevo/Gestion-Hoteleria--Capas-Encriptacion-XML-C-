using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BE;

namespace BLL
{
    public class BLLSesion
    {

        //instancio el objeto Mapper de permisos
        DALPermiso oDALPermiso;
        public BLLSesion()
        {
            oDALPermiso = new DALPermiso();
        }


        public void Login(BEUsuario oBEUsu)
        {

            
            SimuladorSesion.GetInstance.Login(oBEUsu);

        }

        public void Logout()
        {
            SimuladorSesion.GetInstance.Logout();
        }
    }
}
