using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.DynamoDBAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.DynamoDBAccess
{
    public class DynamoUserWrongPasswordRepository : DynamoRepositoryBase, IUserWrongPasswordRepository
    {
        public DynamoUserWrongPasswordRepository(DynamoDBContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public string Create(UserWrongPasswordPoco poco)
        {
            UserWrongPassword entity = _mapper.Map<UserWrongPassword>(poco);
            _context.SaveAsync(entity);

            return entity.Id;
        }
        public void IncreaseFailedCount(string userId)
        {
            UserWrongPassword entity = _context.LoadAsync<UserWrongPassword>(userId).Result;
            if (entity == null)
            {
                entity = new UserWrongPassword
                {
                    Id = userId,
                    FailedCount = 1
                };
                _context.SaveAsync(entity);
            }
            else
            {
                entity.FailedCount++;
                _context.SaveAsync(entity);
            }
        }
        public void Delete(string userId)
        {
            _context.DeleteAsync<UserWrongPassword>(userId);
        }

        public UserWrongPasswordPoco Find(string userId)
        {
            UserWrongPassword entity = _context.LoadAsync<UserWrongPassword>(userId).Result;
            if (entity == null) return null;
            return _mapper.Map<UserWrongPasswordPoco>(entity);

        }
    }
}
