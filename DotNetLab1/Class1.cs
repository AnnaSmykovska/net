using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public class Class1
    {
        public string CardNumber { get; set; }
        public string PinCode { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } // Поле для валюти

        public Class1(string cardNumber, string pinCode, decimal initialBalance, string currency = "UAH")
        {
            CardNumber = cardNumber;
            PinCode = pinCode;
            Balance = initialBalance;
            Currency = currency;
        }

        public bool Authenticate(string pinCode) => PinCode == pinCode;

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума повинна бути більше нуля.");

            Balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > Balance || amount <= 0)
                return false;

            Balance -= amount;
            return true;
        }
    }

    // Клас для банкоматів
    public class AutomatedTellerMachine
    {
        public string ATMId { get; set; }
        public string Location { get; set; }
        public decimal CashAvailable { get; private set; }

        public AutomatedTellerMachine(string atmId, string location, decimal initialCash)
        {
            ATMId = atmId;
            Location = location;
            CashAvailable = initialCash;
        }

        public bool DispenseCash(decimal amount)
        {
            if (amount > CashAvailable || amount <= 0)
                return false;

            CashAvailable -= amount;
            return true;
        }

        public void AddCash(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума повинна бути більше нуля.");

            CashAvailable += amount;
        }
    }

    // Клас для банку
    public class Bank
    {
        public string Name { get; set; }
        public List<AutomatedTellerMachine> ATMs { get; private set; }

        public Bank(string name)
        {
            Name = name;
            ATMs = new List<AutomatedTellerMachine>();
        }

        public void AddATM(AutomatedTellerMachine atm)
        {
            if (atm == null)
                throw new ArgumentNullException(nameof(atm));

            ATMs.Add(atm);
        }

        public AutomatedTellerMachine FindATMById(string atmId)
        {
            return ATMs.Find(atm => atm.ATMId == atmId);
        }
    }
}