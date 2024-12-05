using System;

namespace Test
{
    internal class Program
    {
        private static object user;

        struct User
        {
            public bool IsLoggedIn { get; set; }
            public bool HasPermissions { get; set; }
            public bool IsActive { get; set; }

        }

        void ValidateOrder() { }
        void SaveOrderToDatabase() { }
        static void ShowDashboard() { }

        //Код до рефакторингу
        void ProcessOrderBefore()
        {
            Console.WriteLine("Processing order...");
            ValidateOrder();
            SaveOrderToDatabase();
            Console.WriteLine("Order processed successfully!");
        }

        //Код після рефакторингу
        void ProcessOrder()
        {
            LogStart();
            ValidateOrder();
            SaveOrderToDatabase();
            LogEnd();
        }
        void LogStart()
        {
            Console.WriteLine("Processing order...");
        }
        void LogEnd()
        {
            Console.WriteLine("Order processed successfully!");
        }


        static void Main(string[] args)
        {
            //Код до рефакторингу
            int x = 100;
            int y = 200;
            int z = x + y;
            Console.WriteLine("Sum: " + z);

            //Код після рефакторингу
            int itemPrice = 100;
            int itemTax = 200;
            int totalPrice = itemPrice + itemTax;
            Console.WriteLine("Price: " + totalPrice);

            User user = new User();


            //Код до рефакторингу
            if (user.IsLoggedIn && user.HasPermissions && user.IsActive)
            {
                ShowDashboard();
            }

            //Код після рефакторингу
            if (CanAccessDashboard(user))
            {
                ShowDashboard();
            }
        }
        static bool CanAccessDashboard(User user)
        {
            return user.IsLoggedIn && user.HasPermissions && user.IsActive;
        }
    }
}
