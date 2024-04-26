using ApiCubosExamenAzureMacarena.Data;
using ApiCubosExamenAzureMacarena.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosExamenAzureMacarena.Repositories
{
    public class RepositoryCubos
    {
        public CubosContext context;

        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            List<Cubo> cubos =  await this.context.Cubos.ToListAsync();
            string urlStorage = "https://storageaccountmacarena.blob.core.windows.net/imagenesexamen/";
            foreach (var item in cubos)
            {
                item.Imagen = urlStorage + item.Imagen;
            }
            return cubos;

        }

        public async Task<List<Cubo>> FindCuboAsync(string marca)
        {
            return await this.context.Cubos
                .Where(x => x.Marca == marca) 
                .ToListAsync();
        }

        public async Task<List<CompraCubos>> PedidosUsuario(UsuarioCubo usuario)
        {
            return await this.context.CompraCubos
                .Where(x => x.IdUsuario  == usuario.IdUsuario)
                .ToListAsync();
        }

        public async Task<UsuarioCubo> LogInUsuarioAsync(string email, string password)
        {
            return await this.context.UsuarioCubos
                .Where(x => x.Email == email
                && x.Password == password)
                .FirstOrDefaultAsync();
        }

    }
}
