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

using System;
using EventFlow.Configuration.Cancellation;

namespace EventFlow.Configuration
{
    public class EventFlowConfiguration : IEventFlowConfiguration, ICancellationConfiguration
    {
        public int LoadReadModelEventPageSize { get; set; }
        public int PopulateReadModelEventPageSize { get; set; }
        public int NumberOfRetriesOnOptimisticConcurrencyExceptions { get; set; }
        public TimeSpan DelayBeforeRetryOnOptimisticConcurrencyExceptions { get; set; }
        public bool ThrowSubscriberExceptions { get; set; }
        public bool IsAsynchronousSubscribersEnabled { get; set; }
        public CancellationBoundary CancellationBoundary { get; set; }
        public bool ForwardOptimisticConcurrencyExceptions { get; set; }

        internal EventFlowConfiguration()
        {
            LoadReadModelEventPageSize = 200;
            PopulateReadModelEventPageSize = 10000;
            NumberOfRetriesOnOptimisticConcurrencyExceptions = 4;
            DelayBeforeRetryOnOptimisticConcurrencyExceptions = TimeSpan.FromMilliseconds(100);
            ThrowSubscriberExceptions = false;
            IsAsynchronousSubscribersEnabled = false;
            CancellationBoundary = CancellationBoundary.BeforeCommittingEvents;
            ForwardOptimisticConcurrencyExceptions = false;
        }
    }
}