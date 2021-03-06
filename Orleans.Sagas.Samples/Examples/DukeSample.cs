﻿using Orleans.Sagas.Samples.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    public class DukeSample : Sample
    {
        public override async Task Execute()
        {
            await new List<ISagaGrain>
            {
                await ExecuteNormally(),
                await ExecuteAndAbort(),
                await AbortWithoutExecution(),
                await AbortThenExecute()
            }.Wait();
        }

        private async Task<ISagaGrain> ExecuteNormally()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            return await sagaBuilder.ExecuteSaga();
        }

        public async Task<ISagaGrain> ExecuteAndAbort()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            var saga = await sagaBuilder.ExecuteSaga();

            await saga.RequestAbort();

            return saga;
        }

        public async Task<ISagaGrain> AbortWithoutExecution()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.RequestAbort();

            return saga;
        }

        public async Task<ISagaGrain> AbortThenExecute()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.RequestAbort();

            AddActivities(sagaBuilder);

            await sagaBuilder.ExecuteSaga();

            return saga;
        }

        private static void AddActivities(ISagaBuilder sagaBuilder)
        {
            sagaBuilder
                .AddActivity(new KickAssActivity { Config = new KickAssConfig { KickAssCount = 7 } })
                .AddActivity(new ChewBubblegumActivity());
        }
    }
}
