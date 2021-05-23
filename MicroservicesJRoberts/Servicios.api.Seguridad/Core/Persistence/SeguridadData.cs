﻿using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Seguridad.Core.Persistence
{
    public class SeguridadData
    {
        public static async Task InsertarUsuario(SeguridadContexto context, UserManager<Usuario> usuarioManager)
        {
            if(!usuarioManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    Nombre = "Jonathan",
                    Apellido = "Roberts",
                    Direccion = "Av. La Madrid 369",
                    UserName = "jroberts",
                    Email = "jonathanroberts992@gmail.com"
                };

                await usuarioManager.CreateAsync(usuario, "Password1234$");
            }
        }
    }
}
