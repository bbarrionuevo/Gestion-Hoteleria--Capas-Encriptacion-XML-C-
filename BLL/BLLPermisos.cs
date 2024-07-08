using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
namespace BLL
{
    public class BLLPermisos
    {

        //instancio el objeto Mapper de permisos
        DALPermiso oDALPermiso;
        public BLLPermisos()
        {
            oDALPermiso = new DALPermiso();
        }

        //método para sabner si existe

        public bool Existe(BEComponente c, int id)
        {
            bool existe = false;

            if (c.ID.Equals(id))
                existe = true;
            else

                foreach (var item in c.Hijos)
                {

                    existe = Existe(item, id);
                    if (existe) return true;
                }

            return existe;

        }

        //método para buscar todos los permisos
        public Array GetAllPermission()
        {
            return oDALPermiso.GetAllPermission();
        }


        //método para guardar  los permisosm en la familia
        public BEComponente GuardarComponente(BEComponente p, bool esfamilia)
        {
            return oDALPermiso.GuardarComponente(p, esfamilia);
        }

        //método para guardar  Familias
        public void GuardarFamilia(BEFamilia c)
        {
            oDALPermiso.GuardarFamilia(c);
        }

        //método para taer todas las patentes
        public IList<BEPatente> GetAllPatentes()
        {
            return oDALPermiso.GetAllPatentes();
        }

        //método para taer todas las familias
        public IList<BEFamilia> GetAllFamilias()
        {
            return oDALPermiso.GetAllFamilias();
        }

        //método para taer todas las patentes
        public IList<BEComponente> GetAll(BEFamilia oFamilia)
        {
            return oDALPermiso.GetAll(oFamilia);

        }


        //método para taer los permisos de los suaurios
        public void FillUserComponents(BEUsuario e)
        {
            oDALPermiso.FillUserComponents(e);

        }

        //método para taer todos las familias con sus permisos
        public void FillFamilyComponents(BEFamilia familia)
        {
            oDALPermiso.FillFamilyComponents(familia);
        }


    }
}
