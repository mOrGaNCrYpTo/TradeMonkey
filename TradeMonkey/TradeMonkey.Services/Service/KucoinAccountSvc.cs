using Kucoin.Net.Clients.SpotApi;
using Kucoin.Net.Objects.Models;

using KucoinAccount = Kucoin.Net.Objects.Models.Spot.KucoinAccount;

namespace TradeMonkey.Trader.Services
{
    [RegisterService]
    public sealed class KucoinAccountSvc
    {
        [InjectService]
        public KucoinClientSpotApiAccount KuCoinSpotClient { get; private set; }

        [InjectService]
        public KuCoinDbRepository DbRepo { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="repository"> </param>
        /// <exception cref="ArgumentNullException"> </exception>
        public KucoinAccountSvc(KucoinClientSpotApiAccount kucoinClientSpotApiAccount, KuCoinDbRepository kuCoinDbRepository)
        {
            KuCoinSpotClient = kucoinClientSpotApiAccount ??
                throw new ArgumentNullException(nameof(kucoinClientSpotApiAccount));

            DbRepo = kuCoinDbRepository ?? throw new ArgumentNullException(nameof(kuCoinDbRepository));
        }

        /// <summary>
        /// Gets a transferable balance of a specified account.
        /// </summary>
        /// <param name="asset">       </param>
        /// <param name="accountType"> </param>
        /// <param name="ct">          </param>
        /// <returns> </returns>
        public async Task<KucoinTransferableAccount> GetTransferableAsync(string asset, AccountType accountType, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetTransferableAsync(asset, accountType, ct);
            return result.Data;
        }

        /// <summary>
        /// Transfers assets between accounts
        /// </summary>
        /// <param name="asset">         </param>
        /// <param name="from">          </param>
        /// <param name="to">            </param>
        /// <param name="quantity">      </param>
        /// <param name="fromTag">       </param>
        /// <param name="toTag">         </param>
        /// <param name="clientOrderId"> </param>
        /// <param name="ct">            </param>
        /// <returns> </returns>
        public async Task<KucoinInnerTransfer> InnerTransferAsync(string asset, AccountType from, AccountType to,
            decimal quantity, string? fromTag = default, string? toTag = default, string? clientOrderId = default, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.InnerTransferAsync(asset, from, to, quantity, fromTag, toTag, clientOrderId, ct);
            return result.Data;
        }

        /// <summary>
        /// Withdraw an asset to an address, such as MetaMask or KuCoin wallet
        /// </summary>
        /// <param name="asset">         </param>
        /// <param name="toAddress">     </param>
        /// <param name="quantity">      </param>
        /// <param name="memo">          </param>
        /// <param name="isInner">       </param>
        /// <param name="remark">        </param>
        /// <param name="chain">         </param>
        /// <param name="feeDeductType"> </param>
        /// <param name="ct">            </param>
        /// <returns> </returns>
        public async Task<KucoinNewWithdrawal> WithdrawAsync(string asset, string toAddress, decimal quantity,
            string? memo, bool isInner, string? remark, string? chain = default, FeeDeductType?
            feeDeductType = default, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.WithdrawAsync(asset, toAddress, quantity, memo, isInner, remark, chain,
                feeDeductType, ct);
            return result.Data;
        }

        /// <summary>
        /// Get isolated margin account info
        /// </summary>
        /// <param name="symbol"> </param>
        /// <param name="ct">     </param>
        /// <returns> </returns>
        public async Task<KucoinIsolatedMarginAccount> GetIsolatedMarginAccountAsync(string symbol,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetIsolatedMarginAccountAsync(symbol, ct);
            return result.Data;
        }

        /// <summary>
        /// Gets all isolated margin accounts info
        /// </summary>
        /// <param name="ct"> </param>
        /// <returns> </returns>
        public async Task<KucoinIsolatedMarginAccountsInfo> GetIsolatedMarginAccountsAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetIsolatedMarginAccountsAsync(ct);
            return result.Data;
        }

        public async Task<KucoinMarginAccount> GetMarginAccountAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetMarginAccountAsync(ct);
            return result.Data;
        }

        /// <summary>
        /// Gets a list of accounts
        /// </summary>
        /// <param name="asset">       Optional: Get the accounts for a specific asset </param>
        /// <param name="accountType"> Optional: Filter on type of account </param>
        /// <param name="ct">          Optional: Cancellation token </param>
        /// <returns> </returns>
        public async Task<IEnumerable<KucoinAccount>> GetAccountsAsync(string? asset = default,
            AccountType? accountType = default, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var accounts = await KuCoinSpotClient.GetAccountsAsync(asset, accountType, ct);
            await DbRepo.InsertManyAsync(accounts.Data, ct);
            return accounts.Data;
        }

        /// <summary>
        /// Get a specific account and balance
        /// </summary>
        /// <param name="accountId"> </param>
        /// <param name="ct">        </param>
        /// <returns> </returns>
        public async Task<KucoinAccountSingle> GetAccountAsync(string accountId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var account = await KuCoinSpotClient.GetAccountAsync(accountId, ct);
            await DbRepo.InsertOneAsync(account.Data, ct);
            return account.Data;
        }

        /// <summary>
        /// Get the basic user fees
        /// </summary>
        /// <param name="ct"> </param>
        /// <returns> </returns>
        public async Task<KucoinUserFee> GetBasicUserFeeAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var fee = await KuCoinSpotClient.GetBasicUserFeeAsync(ct);
            return fee.Data;
        }

        /// <summary>
        /// Gets the deposit address for an asset
        /// </summary>
        /// <param name="asset">   The asset to get the address for </param>
        /// <param name="network"> Optional: The network to get the address for </param>
        /// <param name="ct">      Cancellation token </param>
        /// <returns> </returns>
        public async Task<KucoinDepositAddress> GetDepositAddressAsync(string asset, string? network = default,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var address = await KuCoinSpotClient.GetDepositAddressAsync(asset, network, ct);
            return address.Data;
        }

        /// <summary>
        /// Gets the deposit addresses for an asset
        /// </summary>
        /// <param name="asset"> </param>
        /// <param name="ct">    </param>
        /// <returns> </returns>
        public async Task<IEnumerable<KucoinDepositAddress>> GetDepositAddressesAsync(string asset, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var add = await KuCoinSpotClient.GetDepositAddressesAsync(asset, ct);
            return add.Data;
        }

        /// <summary>
        /// Gets a list of account activity
        /// </summary>
        /// <param name="asset">       </param>
        /// <param name="direction">   </param>
        /// <param name="bizType">     </param>
        /// <param name="startTime">   </param>
        /// <param name="endTime">     </param>
        /// <param name="currentPage"> </param>
        /// <param name="pageSize">    </param>
        /// <param name="ct">          </param>
        /// <returns> </returns>
        public async Task<KucoinPaginated<KucoinAccountActivity>> GetAccountLedgersAsync(string? asset = default,
            AccountDirection? direction = default, BizType? bizType = default, DateTime? startTime = default, DateTime?
            endTime = default, int? currentPage = default, int? pageSize = default, CancellationToken ct = default)
        {
            var acct = await KuCoinSpotClient.GetAccountLedgersAsync(asset, direction, bizType, startTime, endTime,
                currentPage, pageSize, ct);
            return acct.Data;
        }

        /// <summary>
        /// Gets a list of deposits
        /// </summary>
        /// <param name="asset">       </param>
        /// <param name="startTime">   </param>
        /// <param name="endTime">     </param>
        /// <param name="status">      </param>
        /// <param name="currentPage"> </param>
        /// <param name="pageSize">    </param>
        /// <param name="ct">          </param>
        /// <returns> </returns>
        public async Task<KucoinPaginated<KucoinDeposit>> GetDepositsAsync(string? asset = default,
            DateTime? startTime = default, DateTime? endTime = default, DepositStatus? status = default,
            int? currentPage = default, int? pageSize = default, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetDepositsAsync(asset, startTime, endTime, status, currentPage, pageSize, ct);
            return result.Data;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">  </param>
        /// <param name="asset"> </param>
        /// <param name="ct">    </param>
        /// <returns> </returns>
        public async Task<KucoinNewAccount> CreateAccountAsync(AccountType type, string asset, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.CreateAccountAsync(type, asset, ct);
            return result.Data;
        }

        /// <summary>
        /// </summary>
        /// <param name="asset">   </param>
        /// <param name="network"> </param>
        /// <param name="ct">      </param>
        /// <returns> </returns>
        public async Task<KucoinDepositAddress> CreateDepositAddressAsync(string asset, string? network = default, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.CreateDepositAddressAsync(asset, network, ct);
            return result.Data;
        }

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="withdrawalId"> </param>
        /// <param name="ct">           </param>
        /// <returns> </returns>
        public async Task<object> CancelWithdrawalAsync(string withdrawalId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.CancelWithdrawalAsync(withdrawalId, ct);
            return result.Data;
        }

        /// <summary>
        /// Get cross margin risk limit
        /// </summary>
        /// <param name="ct"> </param>
        /// <returns> </returns>
        public async Task<IEnumerable<KucoinRiskLimitCrossMargin>> GetRiskLimitCrossMarginAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetRiskLimitCrossMarginAsync(ct);
            return result.Data;
        }

        /// <summary>
        /// Get isolated margin risk limit
        /// </summary>
        /// <param name="ct"> </param>
        /// <returns> </returns>
        public async Task<IEnumerable<KucoinRiskLimitIsolatedMargin>> GetRiskLimitIsolatedMarginAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetRiskLimitIsolatedMarginAsync(ct);
            return result.Data;
        }

        /// <summary>
        /// Get the trading fee for a symbol
        /// </summary>
        /// <param name="symbol"> </param>
        /// <param name="ct">     </param>
        /// <returns> </returns>

        public async Task<IEnumerable<KucoinTradeFee>> GetSymbolTradingFeesAsync(string symbol, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetSymbolTradingFeesAsync(symbol, ct);
            return result.Data;
        }

        /// <summary>
        /// Get the trading fees for symbols
        /// </summary>
        /// <param name="symbol"> </param>
        /// <param name="ct">     </param>
        /// <returns> </returns>

        public async Task<IEnumerable<KucoinTradeFee>> GetSymbolTradingFeesAsync(IEnumerable<string> symbols,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetSymbolTradingFeesAsync(symbols, ct);
            return result.Data;
        }

        /// <summary>
        /// Gets a list of withdrawals
        /// </summary>
        /// <param name="asset">       </param>
        /// <param name="startTime">   </param>
        /// <param name="endTime">     </param>
        /// <param name="status">      </param>
        /// <param name="currentPage"> </param>
        /// <param name="pageSize">    </param>
        /// <param name="ct">          </param>
        /// <returns> </returns>
        public async Task<KucoinPaginated<KucoinWithdrawal>> GetWithdrawalsAsync(string? asset = default,
            DateTime? startTime = default, DateTime? endTime = default, WithdrawalStatus? status = default,
            int? currentPage = default, int? pageSize = default, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await KuCoinSpotClient.GetWithdrawalsAsync(asset, startTime, endTime, status, currentPage, pageSize, ct);
            return result.Data;
        }
    }
}