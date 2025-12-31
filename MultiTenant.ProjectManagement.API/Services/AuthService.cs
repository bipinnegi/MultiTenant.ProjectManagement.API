using Microsoft.EntityFrameworkCore;
using MultiTenant.ProjectManagement.API.Data;
using MultiTenant.ProjectManagement.API.DTOs;
using MultiTenant.ProjectManagement.API.Models;

namespace MultiTenant.ProjectManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResponse> RegisterTenantAsync(RegisterTenantRequest request)
        {
            //Check if email already exist
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.OwnerEmail);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }
            //create tenant
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = request.TenantName

            };

            await _context.Tenants.AddAsync(tenant);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.OwnerName,
                Email = request.OwnerEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Owner",
                TenantId = tenant.Id
            };
            await _context.Users.AddAsync(user);

            // Save changes
            await _context.SaveChangesAsync();

            // Return response
            return new AuthResponse
            {
                TenantId = tenant.Id,
                Role = user.Role,
                Token = "TEMP_TOKEN",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            //Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            // Verify password
            var passwordValid = BCrypt.Net.BCrypt.Verify( request.Password, user.PasswordHash);

            if (!passwordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            // Return response (JWT comes later)
            return new AuthResponse
            {
                TenantId = user.TenantId,
                Role = user.Role,
                Token = "TEMP_TOKEN",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
