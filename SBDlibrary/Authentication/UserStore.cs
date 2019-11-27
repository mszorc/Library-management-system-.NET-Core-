using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using SBDlibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SBDlibrary.Authentication
{
    public class UserStore : IUserStore<Uzytkownicy>, IUserPasswordStore<Uzytkownicy>, IUserRoleStore<Uzytkownicy>, IUserEmailStore<Uzytkownicy>
    {
        private readonly LibraryDbContext _libraryDbContext;

        public UserStore(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _libraryDbContext?.Dispose();
            }
        }
        public Task<string> GetUserIdAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.id_uzytkownika.ToString());
        }

        public Task<string> GetUserNameAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.id_uzytkownika.ToString());
        }

        public Task SetUserNameAsync(Uzytkownicy user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(SetUserNameAsync));
        }

        public Task<string> GetNormalizedUserNameAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(GetNormalizedUserNameAsync));
        }

        public Task SetNormalizedUserNameAsync(Uzytkownicy user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

        public async Task<IdentityResult> CreateAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            _libraryDbContext.Add(user);

            await _libraryDbContext.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> UpdateAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException(nameof(UpdateAsync));
            await _libraryDbContext.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            _libraryDbContext.Remove(user);

            int i = await _libraryDbContext.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<Uzytkownicy> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (int.TryParse(userId, out int id))
            {
                return await _libraryDbContext.Uzytkownicy.FindAsync(id);
            }
            else
            {
                return await Task.FromResult((Uzytkownicy)null);
            }
        }

        public async Task<Uzytkownicy> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _libraryDbContext.Uzytkownicy
                           .AsAsyncEnumerable()
                           .SingleOrDefault(p => p.id_uzytkownika.ToString().Equals(normalizedUserName, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public Task SetPasswordHashAsync(Uzytkownicy user, string passwordHash, CancellationToken cancellationToken)
        {
            user.haslo = passwordHash;

            return Task.FromResult((object)null);
        }

        public Task<string> GetPasswordHashAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.haslo);
        }

        public Task<bool> HasPasswordAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.haslo));
        }









        public async Task AddToRoleAsync(Uzytkownicy user, string roleName, CancellationToken cancellationToken)
        {
            var role = _libraryDbContext.Role.FirstOrDefault(m => m.nazwa == roleName);
            if (role != null)
            {
                var user_role = new Uzytkownicy_role()
                {
                    id_uzytkownika = user.id_uzytkownika,
                    id_roli = role.id_roli
                };

                _libraryDbContext.Add(user_role);
                await _libraryDbContext.SaveChangesAsync(cancellationToken);
            }          
        }

        public async Task RemoveFromRoleAsync(Uzytkownicy user, string roleName, CancellationToken cancellationToken)
        {
            var role = _libraryDbContext.Role.FirstOrDefault(m => m.nazwa == roleName);

            if (role == null)
            {
                return;
            }
            var user_role = _libraryDbContext.Uzytkownicy_role.FirstOrDefaultAsync(m => m.id_uzytkownika == user.id_uzytkownika && m.id_roli == role.id_roli);
            if (user_role == null)
            {
                return;
            }
            _libraryDbContext.Remove(user_role);
            await _libraryDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            var roles = _libraryDbContext.Uzytkownicy_role.Where(m => m.id_uzytkownika == user.id_uzytkownika);

            List<string> roles_string = new List<string>();

            foreach(Uzytkownicy_role x in roles)
            {
                var role = await _libraryDbContext.Role.FirstOrDefaultAsync(m => m.id_roli == x.id_roli);
                roles_string.Add(role.nazwa);
            }

            return await Task.FromResult(roles_string);
        }


        public Task<bool> IsInRoleAsync(Uzytkownicy user, string roleName, CancellationToken cancellationToken)
        {
            var user_roles = _libraryDbContext.Uzytkownicy_role.Where(m => m.id_uzytkownika == user.id_uzytkownika).ToList();
            var role = _libraryDbContext.Role.FirstOrDefault(m => m.nazwa == roleName);

            var x = user_roles.Find(m => m.id_roli == role.id_roli);
            if (x != null) return Task.FromResult(true);
            else return Task.FromResult(false);
        }






        public Task<IList<Uzytkownicy>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(Uzytkownicy user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            var email = user.email;
            return Task.FromResult(email);
        }

        public Task<bool> GetEmailConfirmedAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(Uzytkownicy user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Uzytkownicy> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var user = await _libraryDbContext.Uzytkownicy.FirstOrDefaultAsync(m => m.email == normalizedEmail);

            return user;
        }

        public Task<string> GetNormalizedEmailAsync(Uzytkownicy user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(Uzytkownicy user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.email = normalizedEmail.ToUpper();
            _libraryDbContext.Update(user);
            return Task.CompletedTask;
        }
    }
}
