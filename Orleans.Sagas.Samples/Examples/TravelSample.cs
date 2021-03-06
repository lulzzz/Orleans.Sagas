﻿using Orleans.Sagas.Samples.Activities;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    public class TravelSample : Sample
    {
        public override async Task Execute()
        {
            var saga = await GrainFactory.CreateSaga()
                .AddActivity(new BookHireCarActivity { Config = new BookHireCarConfig() })
                .AddActivity(new BookHotelActivity())
                .AddActivity(new BookPlaneActivity())
                .ExecuteSaga();

            await saga.Wait();
        }
    }
}
