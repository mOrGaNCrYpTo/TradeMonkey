namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // remove recommended periods
    /// <include file='../../_common/Results/info.xml' path='info/type[@name="Prune"]/*' />
    ///
    public static IEnumerable<AlmaResult> RemoveWarmupPeriods(
        this IEnumerable<AlmaResult> results)
    {
        int removePeriods = results
            .ToList()
            .FindIndex(x => x.Alma != null);

        return results.Remove(removePeriods);
    }
}
