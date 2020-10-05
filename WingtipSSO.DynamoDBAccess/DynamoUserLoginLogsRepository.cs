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
    public class DynamoUserLoginLogsRepository : DynamoRepositoryBase, IUserLoginLogsRepository
    {
        public DynamoUserLoginLogsRepository(DynamoDBContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public string Create(UserLoginLogPoco poco)
        {
            UserLoginLog entity = _mapper.Map<UserLoginLog>(poco);
            entity.Id = Guid.NewGuid().ToString();
            _context.SaveAsync(entity);

            return entity.Id;
        }
    }
}
