using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace WebApplicationPMRO2.Models
{
    public class MenuItemBD
    {
          public int RolId { get; set; }

        public int MenuId { get; set; }

        public int PuedeVer { get; set; }

        public int PuedeCrear { get; set; }

        public int PuedeEditar { get; set; }

        public int PuedeEliminar { get; set; }

        public int ModuloId { get; set; }

        public string Titulo { get; set; }

        public string Url { get; set; }

        public string Icono { get; set; }

        public int Orden { get; set; }
    }
}