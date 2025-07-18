﻿using Microsoft.EntityFrameworkCore;
using VerstaTest.Domain;

namespace VerstaTest.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; set; }
    DbSet<Order> Orders { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
