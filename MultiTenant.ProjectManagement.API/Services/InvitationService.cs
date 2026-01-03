using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Data;
using MultiTenant.ProjectManagement.API.Helpers;
using MultiTenant.ProjectManagement.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace MultiTenant.ProjectManagement.API.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly AppDbContext _context;
        private readonly TenantContext _tenantContext;
        private readonly PasswordHasher _passwordHasher;

        public InvitationService(
            AppDbContext context,
            TenantContext tenantContext,
            PasswordHasher passwordHasher)
        {
            _context = context;
            _tenantContext = tenantContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<Invitation> CreateInvitationAsync(string email, string role)
        {
            var tenantId = _tenantContext.GetTenantId();

            // Prevent duplicate users in same tenant
            var userExists = await _context.Users
                .AnyAsync(u => u.Email == email && u.TenantId == tenantId);

            if (userExists)
            {
                throw new Exception("User already exists in this tenant");
            }

            var invitation = new Invitation
            {
                Id = Guid.NewGuid(),
                Email = email,
                Role = role,
                TenantId = tenantId,
                Token = GenerateSecureToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(2),
                IsAccepted = false
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();

            return invitation;
        }

        public async Task AcceptInvitationAsync(string token, string fullName, string password)

        {
            var invitation = await _context.Invitations
                .FirstOrDefaultAsync(i => i.Token == token);

            if (invitation == null)
                throw new Exception("Invalid invitation token");

            if (invitation.IsAccepted)
                throw new Exception("Invitation already used");

            if (invitation.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invitation expired");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = invitation.Email,
                FullName = fullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = invitation.Role,
                TenantId = invitation.TenantId
            };

            _context.Users.Add(user);

            invitation.IsAccepted = true;
            await _context.SaveChangesAsync();
        }

        private string GenerateSecureToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }
    }
}
