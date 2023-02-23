using Mapster;

using System.Threading.Tasks;

namespace TradeMonkey.DataCollector.Helpers
{
    [RegisterService]
    public class KucoinTickerDataSvc
    {
        private readonly TmDBContext _dbContext;

        [InjectService]
        public ApiRepository ApiRepository { get; private set; }

        public KucoinTickerDataSvc(TmDBContext dBContext, ApiRepository apiRepository)
        {
            _dbContext = dBContext;
            ApiRepository = apiRepository;
        }

        public async Task ProcessTick(KucoinStreamTick streamtick, CancellationToken token)
        {
            var tick = streamtick.Adapt<Data.Entity.KucoinTick>();

            _ = await _dbContext.KucoinTicks.AddAsync(tick, token);
            _ = await _dbContext.SaveChangesAsync(token);

            Parallel.Invoke(() => DoSomeWork(), () => DoSomeOtherWork());
        }
    }
}