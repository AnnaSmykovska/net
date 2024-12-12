using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankConsoleApp
{
    class Program
    {
        static List<Account> accounts = new List<Account>
        {
            new Account("1234567891234567", "1234", 1000, "UAH"),
            new Account("9876543219876543", "5678", 2000, "UAH")
        };
        static void Main(string[] args)
        {
            Console.WriteLine("Вiтаємо у системi банкомата!");
            Account currentAccount = Authenticate();

            if (currentAccount != null)
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nОберiть дiю:");
                    Console.WriteLine("1. Перегляд балансу");
                    Console.WriteLine("2. Поповнити рахунок");
                    Console.WriteLine("3. Зняти кошти");
                    Console.WriteLine("4. Переказ коштiв на iншу картку");
                    Console.WriteLine("5. Вихiд");
                    Console.Write("Ваш вибiр: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            ViewBalance(currentAccount);
                            break;
                        case "2":
                            Deposit(currentAccount);
                            break;
                        case "3":
                            Withdraw(currentAccount);
                            break;
                        case "4":
                            Transfer(currentAccount);
                            break;
                        case "5":
                            exit = true;
                            Console.WriteLine("Дякуємо за використання системи банкомата!");
                            break;
                        default:
                            Console.WriteLine("Неправильний вибiр. Спробуйте ще раз.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Не вдалося виконати аутентифiкацiю.");
            }
        }

        static Account Authenticate()
        {
            Console.Write("Введiть номер картки: ");
            string cardNumber = Console.ReadLine();
            Console.Write("Введiть PIN-код: ");
            string pinCode = Console.ReadLine();

            foreach (var account in accounts)
            {
                if (account.CardNumber == cardNumber && account.PinCode == pinCode)
                {
                    Console.WriteLine("Аутентифiкацiя успiшна!");
                    return account;
                }
            }

            Console.WriteLine("Невiрний номер картки або PIN-код.");
            return null;
        }

        static void ViewBalance(Account account)
        {
            Console.WriteLine($"Ваш баланс: {account.Balance} {account.Currency}");
        }

        static void Deposit(Account account)
        {
            Console.Write("Введiть суму для поповнення: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                account.Deposit(amount);
                Console.WriteLine("Кошти успiшно зарахованi. Новий баланс: {account.Balance} {account.Currency}");
            }
            else
            {
                Console.WriteLine("Некоректна сума.");
            }
        }

        static void Withdraw(Account account)
        {
            Console.Write("Введiть суму для зняття: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (account.Withdraw(amount))
                {
                    Console.WriteLine("Кошти успiшно знято. Новий баланс: {account.Balance} {account.Currency}");
                }
                else
                {
                    Console.WriteLine("Недостатньо коштiв.");
                }
            }
            else
            {
                Console.WriteLine("Некоректна сума.");
            }
        }

        static void Transfer(Account senderAccount)
        {
            Console.Write("Введiть номер картки отримувача: ");
            string recipientCardNumber = Console.ReadLine();
            Account recipientAccount = accounts.Find(a => a.CardNumber == recipientCardNumber);

            if (recipientAccount == null)
            {
                Console.WriteLine("Картку отримувача не знайдено.");
                return;
            }

            Console.Write("Введiть суму для переказу: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (senderAccount.Withdraw(amount))
                {
                    recipientAccount.Deposit(amount);
                    Console.WriteLine("Кошти успiшно переказанi. Ваш новий баланс: {senderAccount.Balance} {senderAccount.Currency}");
                }
                else
                {
                    Console.WriteLine("Недостатньо коштiв для переказу.");
                }
            }
            else
            {
                Console.WriteLine("Некоректна сума.");
            }
        }
    }

    class Account
    {
        public string CardNumber { get; set; }
        public string PinCode { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }

        public Account(string cardNumber, string pinCode, decimal initialBalance, string currency)
        {
            CardNumber = cardNumber;
            PinCode = pinCode;
            Balance = initialBalance;
            Currency = currency;
        }

        public void Deposit(decimal amount) => Balance += amount;

        public bool Withdraw(decimal amount)
        {
            if (amount > Balance) return false;
            Balance -= amount;
            return true;
        }
    }
}