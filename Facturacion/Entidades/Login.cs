using System;

namespace Entidades
{
    public class Login
    {
        public string CodigoUsuario { get; set; }

        public string Contraseña { get; set; }

        public string Rol { get; set; }

        public DateTime MyProperty { get; set; }

        public Login()
        {
        }

        public Login(string codigoUsuario, string contraseña, string rol)
        {
            CodigoUsuario = codigoUsuario;
            Contraseña = contraseña;
            Rol = rol;
        }
    }
}
