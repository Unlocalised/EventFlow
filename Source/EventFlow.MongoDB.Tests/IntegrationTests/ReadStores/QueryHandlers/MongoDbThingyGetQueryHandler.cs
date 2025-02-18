// The MIT License (MIT)
// 
// Copyright (c) 2015-2024 Rasmus Mikkelsen
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.MongoDB.ReadStores;
using EventFlow.MongoDB.Tests.IntegrationTests.ReadStores.ReadModels;
using EventFlow.Queries;
using EventFlow.TestHelpers.Aggregates;
using EventFlow.TestHelpers.Aggregates.Queries;
using MongoDB.Driver;

namespace EventFlow.MongoDB.Tests.IntegrationTests.ReadStores.QueryHandlers
{
    public class MongoDbThingyGetQueryHandler : IQueryHandler<ThingyGetQuery, Thingy>
    {
        private readonly IMongoDbReadModelStore<MongoDbThingyReadModel> _readStore;
        public MongoDbThingyGetQueryHandler(
            IMongoDbReadModelStore<MongoDbThingyReadModel> mongeReadStore)
        {
            _readStore = mongeReadStore;
        }

        public async Task<Thingy> ExecuteQueryAsync(ThingyGetQuery query, CancellationToken cancellationToken)
        {
            var thingyId = query.ThingyId.ToString();
            var asyncCursor = await _readStore.FindAsync(f => string.Equals(f.Id, thingyId), cancellationToken: cancellationToken).ConfigureAwait(false);
            var thingyReadModel = await asyncCursor.FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            return thingyReadModel?.ToThingy();
        }
    }
}