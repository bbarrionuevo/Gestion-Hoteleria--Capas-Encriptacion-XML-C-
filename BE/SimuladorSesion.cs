using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SimuladorSesion

    {

        //simulo que la session del usuario, si se loguea o desloguea y habilito menu

        static SimuladorSesion _sesion;
        BEUsuario oBEusuario;

        public static SimuladorSesion GetInstance
        {
            get
            {
                if (_sesion == null) _sesion = new SimuladorSesion();
                return _sesion;
            }
        }
        public bool IsLoggedIn()
        {
            return oBEusuario != null;
        }

        bool isInRole(BEComponente c, BETipoPermiso permiso, bool existe)
        {


            if (c.Permiso.Equals(permiso))
                existe = true;
            else
            {
                foreach (var item in c.Hijos)
                {
                    existe = isInRole(item, permiso, existe);
                    if (existe) return true;
                }



            }

            return existe;
        }

        public bool IsInRole(BETipoPermiso permiso)
        {
            bool existe = false;

            // Comprobar si el usuario tiene un rol antes de intentar acceder a sus hijos
            if (oBEusuario.Rol != null)
            {
                foreach (var item in oBEusuario.Rol.Hijos)
                {
                    if (item.Permiso.Equals(permiso))
                        return true;
                    
                }
            }

            return existe;
        }

        public void Logout()
        {
            _sesion.oBEusuario = null;
        }


        public void Login(BEUsuario u)
        {
            _sesion.oBEusuario = u;

        }

        private SimuladorSesion()
        {

        }
    }
}
