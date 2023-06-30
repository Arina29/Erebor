using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace IdentityServer.Migrations.Extensions;

public static class DataSeed
{
    const string InitialUserId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";

    public static void SeedAuthDbContext(this ModelBuilder modelBuilder)
    {
        var initialUser = new EreborUser
        { Id = InitialUserId, UserName = "alice", Email = "AliceSmith@email.com", EmailConfirmed = true };

        var passHasher = new PasswordHasher<EreborUser>();
        initialUser.PasswordHash = passHasher.HashPassword(initialUser, "Pass123$");

        modelBuilder.Entity<EreborUser>()
            .HasData(initialUser);
    }
}
