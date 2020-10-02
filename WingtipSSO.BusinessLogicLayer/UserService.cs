using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.POCOS;

namespace WingtipSSO.BusinessLogicLayer
{
    public interface IUserService
    {
        string Create(UserPoco poco);
        UserPoco Get(string userId);
        IList<UserPoco> Get();
        UserPoco Authenticate(string userId, string password);
    }
    public class UserService : IUserService
    {
        private const int saltLengthLimit = 10;
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public UserPoco Authenticate(string userId, string password)
        {
            var poco = _usersRepository.Find(userId);
            if (poco == null)
            {
                throw new LoginException("Invalid username or password");
            }
            if (VerifyHash(password, poco.PasswordHash))
            {
                if (!poco.IsActivated)
                {
                    throw new LoginException("User activation rquired.");
                }else if (poco.IsLocked)
                {
                    throw new LoginException("Account is locked.");
                }
                return poco;
            }
            else
            {
                throw new LoginException("Invalid username or password");
            }
        }
        public string Create(UserPoco poco)
        {
            poco.Id = poco.Email;
            poco.IsActivated = true;
            poco.IsLocked = false;
            poco.Created = DateTime.Now;
            poco.LastUpdated = DateTime.Now;
            poco.IsAdmin = false;
            poco.IsUser = true;

            // make password hash 
            poco.PasswordHash = ComputeHash(poco.Password, null);
            poco.Password = null;

            poco.Roles = new List<Role> { Role.Admin };

            _usersRepository.Create(poco);
            return poco.Id;
        }
        public UserPoco Get(string userId)
        {
            return _usersRepository.Find(userId);
        }
        public IList<UserPoco> Get()
        {
            return _usersRepository.Read();
        }
        private static byte[] GetSalt()
        {
            return GetSalt(saltLengthLimit);
        }

        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }
        private string ComputeHash(string plainText, byte[] saltBytes)
        {
            if (saltBytes == null)
            {
                saltBytes = GetSalt();
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            }

            HashAlgorithm hash = new SHA512Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashWithSaltBytes[i] = hashBytes[i];
            }
            for (int i = 0; i < saltBytes.Length; i++)
            {
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
            }

            return Convert.ToBase64String(hashWithSaltBytes);
        }
        private bool VerifyHash(string plainText, string hashValue)
        {
            const int hashSizeInBytes = 64;

            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];
            string expectedHashString = ComputeHash(plainText, saltBytes);
            return (hashValue == expectedHashString);
        }
    }
}
