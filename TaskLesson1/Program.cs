using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLesson1
{
    class Program
    {
        private static readonly bool isLongTermOperation;

        static void Main(string[] args)
        {
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            //MakeLongWork();
            //Console.WriteLine("UI поток закончил");
            //Console.ReadLine();
            //1 способ создания (не запуска)
            //Task<int> task = new Task<int>(() => {
            //    Thread.Sleep(15000);
            //    return 1;
            //});
            //Task actionTask = new Task(() => Console.WriteLine());
            //Запуск задачи
            //task.Start();
            //Получение результата
            //while(!task.IsCompleted)
            //{
            //    Console.WriteLine("Идет работа...");
            //    Thread.Sleep(300);
            //}
            //var result = task.Result;
            //2 спосооб (тонкая настройка)
            Task.Factory.StartNew(() => Console.WriteLine(), TaskCreationOptions.LongRunning);
            //3 способ (запуск CPU bound операции - всегда)
            Task.Run(() => 1);
            //Отмена операции в процессе выполнения
            var cancelationTokenSource = new CancellationTokenSource();
            var cancelationToken = cancelationTokenSource.Token;
            //bekzat ne prochtet eto potomushto on ne smozhet eto prochest i on etogo ne uvidit ya eto tochno znayu
            var task = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Первая порция");
                if(cancelationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Отмена операции");
                    return string.Empty;
                }
                Thread.Sleep(1000);
                Console.WriteLine("Вторая порция");
                Thread.Sleep(1000);
                Console.WriteLine("Получен результат");
                return "Ответ";
            }, cancelationToken);
            cancelationTokenSource.CancelAfter(1500);
            var res = task.Result;
            var longTask = LongOperation();
            var task1 = new Task(() => Console.WriteLine(1));
            var task3 = new Task(() => Console.WriteLine(3));
            var task4 = new Task(() => Console.WriteLine(4));
            var task5 = new Task(() => Console.WriteLine(5));
            var task6 = new Task(() => Console.WriteLine(6));
            task1.ContinueWith(resultTask => Console.WriteLine(2)).ContinueWith(resultTask => Console.WriteLine(3)).ContinueWith(resultTask => Console.WriteLine(4));
            Console.ReadLine();
            var task2 = new Task<int>(() => 1);
            task2.ContinueWith(resultTask => Console.WriteLine(resultTask.Result));
        }
        private static Task LongOperation()
        {
            if(isLongTermOperation)
            {
                return Task.Run(() => Thread.Sleep(20000));
            }
            else
            {
                int x = 15 + 10;
                Console.WriteLine(x);
                return Task.CompletedTask;
                //Task.FromCanceled и Task.FromCanceled<результат>
                //если нужен результат, то return Task.FromResult(результат);
            }
        }
        //private static Task MakeLongWork()
        //{
        //    return new Task(() => Console.WriteLine(Thread.CurrentThread.ManagedThreadId));
        //}
    }
}
