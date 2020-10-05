using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.DynamoDBAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.DynamoDBAccess
{
    public class DynamoUsersRepository : DynamoRepositoryBase, IUsersRepository
    {
        public DynamoUsersRepository(DynamoDBContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public void Create(UserPoco poco)
        {
            var conditions = new List<ScanCondition>();
            List<User> list = _context.ScanAsync<User>(conditions).GetRemainingAsync().Result;

            conditions.Add(new ScanCondition("Email", ScanOperator.Equal, new object[] { "mrgocat3@ggg.com" }));
            list = _context.ScanAsync<User>(conditions).GetRemainingAsync().Result;


            poco.Roles = new List<Role> { Role.Admin, Role.Moderator };
            User entity = _mapper.Map<User>(poco);
        //    UserPoco pp = _mapper.Map<UserPoco>(entity);
            _context.SaveAsync(entity);

        //    User user = _context.LoadAsync<User>(entity.Id).Result;

        }
        public UserPoco Find(string id)
        {
            User entity = _context.LoadAsync<User>(id).Result;
            if (entity == null) return null;
            return _mapper.Map<UserPoco>(entity);
        }
        public Boolean CheckIdExists(string userId)
        {
            User user = _context.LoadAsync<User>(userId).Result;
            if (user == null) return false;
            return true;
        }
        public bool LockUser(string userId)
        {

            User user = _context.LoadAsync<User>(userId).Result;
            if (user == null)
            {
                return false;
            }
            user.IsLocked = true;
            user.LastUpdated = DateTime.Now;

            _context.SaveAsync(user);
            return true;
        }

        public bool Patch<T>(string userId, string key, T value)
        {
            User user = _context.LoadAsync<User>(userId).Result;
            if (user == null)
            {
                return false;
            }
            PropertyInfo propInfo = user.GetType().GetProperty(key);
            if(propInfo == null)
            {
                return false;
            }
            propInfo.SetValue(user, value);
            user.LastUpdated = DateTime.Now;

            _context.SaveAsync(user);
            return true;
        }

        public IList<UserPoco> Read()
        {
            var conditions = new List<ScanCondition>();
            List<User> list = _context.ScanAsync<User>(conditions).GetRemainingAsync().Result;

        //    conditions.Add(new ScanCondition("FirstName", ScanOperator.Equal, new object[] { "Ray" }));
        //    list = _context.ScanAsync<User>(conditions).GetRemainingAsync().Result;

            return _mapper.Map<List<UserPoco>>(list);
        }

        public bool Update(UserPoco poco)
        {
            User user = _context.LoadAsync<User>(poco.Id).Result;
            if (user == null)
            {
                return false;
            }
            user.LastName = poco.LastName;
            user.FirstName = poco.FirstName;
            user.Email = poco.Email;
            user.Nationality = poco.Nationality;
            user.Telephone = poco.Telephone;
            user.DateOfBirth = poco.DateOfBirth;
            user.LastUpdated = DateTime.Now;

            _context.SaveAsync(user);
            return true;
        }

        public bool UpdatePassword(string id, string passwordHash)
        {
            User user = _context.LoadAsync<User>(id).Result;
            if (user == null)
            {
                return false;
            }

            user.PasswordHash = passwordHash;
            user.LastUpdated = DateTime.Now;

            _context.SaveAsync(user);
            return true;
        }
    }
}
