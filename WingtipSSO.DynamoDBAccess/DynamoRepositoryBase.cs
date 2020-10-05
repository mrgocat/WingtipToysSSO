using Amazon.DynamoDBv2.DataModel;
using AutoMapper;

namespace WingtipSSO.DynamoDBAccess
{
    public abstract class  DynamoRepositoryBase
    {
        protected readonly IMapper _mapper;
        protected readonly DynamoDBContext _context;

        public DynamoRepositoryBase(DynamoDBContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
