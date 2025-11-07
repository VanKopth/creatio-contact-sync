using System;
using System.Linq;

namespace  ItForge.Salmon.JsonPlaceholder
{
    public class PhoneGenerator : IPhoneGenerator
    {
        private readonly Random _random;

        public PhoneGenerator()
        {
            _random = new Random();
        }

        public string GeneratePhilippineNumber()
        {
            var digits = string.Concat(Enumerable.Range(0, 9).Select(d => _random.Next(0, 10)));

            return $"+63{digits}";
        }
    }
}
