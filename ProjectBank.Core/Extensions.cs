namespace ProjectBank.Core
{
    public static class Extensions
    {
        public static async Task<HashSet<T>> ToSetAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            var results = new HashSet<T>();
            await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                results.Add(item);
            }

            return results;
        }
    }
}
