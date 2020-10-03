using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.POCOS;

namespace WingtipSSO.BusinessLogicLayer
{

    public class UserService : IUserService
    {
        private const int MaxLoginAttempt = 5;
        private const int saltLengthLimit = 10;
        private readonly IUsersRepository _usersRepository;
        private readonly IUserHistoriesRepository _userHistoriesRepository;
        private readonly IUserLoginLogsRepository _userLoginLogsRepository;
        private readonly IUserWrongPasswordRepository _userWrongPasswordRepository;

        public UserService(IUsersRepository usersRepository, 
            IUserHistoriesRepository userHistoriesRepository, 
            IUserLoginLogsRepository userLoginLogsRepository,
            IUserWrongPasswordRepository userWrongPasswordRepository)
        {
            _usersRepository = usersRepository;
            _userHistoriesRepository = userHistoriesRepository;
            _userLoginLogsRepository = userLoginLogsRepository;
            _userWrongPasswordRepository = userWrongPasswordRepository;
        }
        public UserPoco Authenticate(string userId, string password, string loginIP = null)
        {
            var poco = _usersRepository.Find(userId);
            if (poco == null)
            {
                CreateLoginLog(userId, loginIP, true, "Cannot find user.");
                throw new LoginException("Invalid username or password");
            }
            if (VerifyHash(password, poco.PasswordHash))
            {
                if (!poco.IsActivated)
                {
                    CreateLoginLog(userId, loginIP, true, "User activation rquired.");
                    throw new LoginException("User activation rquired.");
                }else if (poco.IsLocked)
                {
                    CreateLoginLog(userId, loginIP, true, "Account is locked.");
                    throw new LoginException("Account is locked.");
                }
                CreateLoginLog(userId, loginIP, false, "Success.");
                _userWrongPasswordRepository.Delete(userId);
                return poco;
            }
            else
            {
                CreateLoginLog(userId, loginIP, true, "Password verification failed.");
                if (poco.IsLocked)
                {
                    throw new LoginException("Your account is locked.");
                } 
                else 
                { 
                    UserWrongPasswordPoco wpPoco = _userWrongPasswordRepository.Find(userId);
                    if (wpPoco != null && wpPoco.FailedCount >= MaxLoginAttempt)
                    {
                        _userWrongPasswordRepository.Delete(userId);
                        _usersRepository.LockUser(userId);
                        _userHistoriesRepository.Create(userId, userId, "Max login attempt reached.");
                        throw new LoginException("Invalid username or password. Your account is locked.");
                    }
                    else
                    {
                        _userWrongPasswordRepository.IncreaseFailedCount(userId);
                        throw new LoginException("Invalid username or password");
                    }
                }
            }
        }
        private void CreateLoginLog(string userId, string loginIP, bool failed, string failedReason)
        {
            UserLoginLogPoco poco = new UserLoginLogPoco
            {
                UserId = userId,
                LoginIP = loginIP,
                LoginDate = DateTime.Now,
                Failed = failed,
                FailedReason = failedReason
            };
            _userLoginLogsRepository.Create(poco);
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

            //poco.Roles = new List<Role> { Role.Admin };

            _usersRepository.Create(poco);

            /*UserHistoryPoco uhPoco = new UserHistoryPoco
            {
                UserId = poco.Id,
                Updated = poco.Created,
                SnapShot = poco,
                UpdateUserId = poco.Id
            };*/

            _userHistoriesRepository.Create(poco.Id, poco.Id, "Create New");

            return poco.Id;
        }
        public void UpdatePasswrod(string userId, string oldPassword, string newPassword)
        {
            var poco = _usersRepository.Find(userId);
            if (poco == null)
            {
                throw new UpdateException("User not found");
            }
            if (!VerifyHash(oldPassword, poco.PasswordHash))
            {
                throw new UpdateException("Password does not match");
            }
            string newPasswordHash = ComputeHash(newPassword, null);
            _usersRepository.UpdatePassword(userId, newPasswordHash);
            _userHistoriesRepository.Create(userId, userId, "Password update");
        }
        public void Update(UserPoco poco)
        {
            _usersRepository.Update(poco);
            _userHistoriesRepository.Create(poco.Id, poco.Id, "Update Info");
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
