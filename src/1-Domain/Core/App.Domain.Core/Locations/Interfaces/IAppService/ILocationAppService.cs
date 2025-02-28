using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Locations.Interfaces.IAppService
{
    public interface ILocationAppService
    {
        Task<List<Province>> GetAllProvincesAsync(CancellationToken cancellationToken);
        Task<List<City>> GetAllCitiesAsync(CancellationToken cancellationToken);
    }
}
