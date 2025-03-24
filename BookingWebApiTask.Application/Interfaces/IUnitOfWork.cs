using BookingWebApiTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApiTask.Application.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        public IBaseRepository<ApplicationUser, string> ApplicationUser { get; }

        public IBaseRepository<Reservation, int> Reservation { get; }

        public IBaseRepository<Trip, int> Trip { get; }

        public Task<int> SaveChangesAsync();
    }
}
