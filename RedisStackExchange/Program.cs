using System;
using System.Threading;
using RedisHelp;
using StackExchange.Redis;

namespace RedisStackExchange
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var redis = new RedisHelper(1);

            #region String

            var str = "123";
            var demo = new DemoModel
            {
                Id = 1,
                Name = "123"
            };
            //var resukt = redis.StringSet("redis_string_test", str);
            //var str1 = redis.StringGet("redis_string_test");
            //redis.StringSet("redis_string_model", demo);
            //var model = redis.StringGet<Demo>("redis_string_model");

            //for (int i = 0; i < 10; i++)
            //{
            //    var dd=redis.StringIncrement("StringIncrement", 2);
            //}
            //for (int i = 0; i < 10; i++)
            //{
            //    redis.StringDecrement("StringIncrement");
            //}
            //redis.StringSet("redis_string_model1", demo, TimeSpan.FromSeconds(10));

            #endregion String

            #region Hash

            redis.HashSet("user", "u1", "123");
            redis.HashSet("user", "u2", "1234");
            redis.HashSet("user", "u3", "1235");
            var news = redis.HashGet<string>("user", "u2");

            #endregion

            #region List

            for (var i = 0; i < 10; i++)
            {
                redis.ListRightPush("list", i);
            }

            for (var i = 10; i < 20; i++)
            {
                redis.ListLeftPush("list", i);
            }
            var length = redis.ListLength("list");

            var leftpop = redis.ListLeftPop<string>("list");
            var rightPop = redis.ListRightPop<string>("list");

            var list = redis.ListRange<int>("list");

            #endregion

            #region Transaction

            var tran = redis.CreateTransaction();

            tran.StringSetAsync("tran_string", "test1");
            tran.StringSetAsync("tran_string1", "test2");
            var committed = tran.Execute();

            #endregion

            #region Lock

            var db = redis.GetDatabase();
            RedisValue token = Environment.MachineName;
            if (db.LockTake("lock_test", token, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    //TODO:do you want to do
                    Thread.Sleep(5000);
                }
                finally
                {
                    db.LockRelease("lock_test", token);
                }
            }

            #endregion Lock

            Console.ReadKey();
        }
    }

    public class DemoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}