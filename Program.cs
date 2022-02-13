using System;

namespace stateless_state_machine
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Stateless.StateMachine<Car.State, Car.Action>(Car.State.Stopped);


            //arrange
            car.Configure(Car.State.Stopped)
                .Permit(Car.Action.Start, Car.State.Started);

            car.Configure(Car.State.Started)
                .Permit(Car.Action.Accelarate, Car.State.Running)
                .PermitReentry(Car.Action.Start)
                .Permit(Car.Action.Stop, Car.State.Stopped)
                .OnEntry(s => Console.WriteLine($"Entry: {s.Source} -> {s.Destination}"))
                .OnExit(s => Console.WriteLine($"Exit: {s.Source} -> {s.Destination}"));

            var triggersWithParam = car.SetTriggerParameters<int>(Car.Action.Accelarate);

            car.Configure(Car.State.Running)
                .SubstateOf(Car.State.Started)
                .Permit(Car.Action.Stop, Car.State.Stopped)
                .OnEntryFrom(triggersWithParam, speed => Console.WriteLine($"Speed: {speed}"))
                .InternalTransition(Car.Action.Start, () => Console.WriteLine("Started called while in Running state"));

            Console.WriteLine($"Current state: {car.State}");

            //act
            car.Fire(Car.Action.Start);
            Console.WriteLine($"Current state: {car.State}");

            car.Fire(triggersWithParam, 50);
            Console.WriteLine($"Current state: {car.State}");

            car.Fire(Car.Action.Accelarate);
            Console.WriteLine($"Current state: {car.State}");

            car.Fire(Car.Action.Start);

            car.Fire(Car.Action.Stop);
            Console.WriteLine($"Current state: {car.State}");

        }
    }
}
