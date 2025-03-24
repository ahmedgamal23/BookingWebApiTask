using BookingWebApiTask.Application.Interfaces;
using BookingWebApiTask.Domain.Data;
using BookingWebApiTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApiTask.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<ApplicationUser, string> ApplicationUser { get; private set; }

        public IBaseRepository<Reservation, int> Reservation { get; private set; }

        public IBaseRepository<Trip, int> Trip { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ApplicationUser = new BaseRepository<ApplicationUser, string>(_context);
            Reservation = new BaseRepository<Reservation, int>(_context);
            Trip = new BaseRepository<Trip, int>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
