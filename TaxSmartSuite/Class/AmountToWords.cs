using System;

namespace TaxSmartSuite.Class
{
    public class AmountToWords
    {
        string englishTranslation;

        long numb;

        int numbDec;

        public string convertToWords(string amount)
        {
            string mainString = Convert.ToString(amount);
            string[] Split = mainString.Split(new Char[] { '.' });

            //////"numb" holds the naria part of the amount as integer.
            numb = Convert.ToInt64(Split[0]);

            /////"num" holds the kobo part as string so that a 2 decimal place (.00k) can be created using SUBSTRING.                        
            string num = Convert.ToString(Split[1]);

            /////"numbDec" holds the kobo part in 2 decimal places.
            numbDec = Convert.ToInt32(num.Substring(0, 2));

            ///////for amount that does not have a kobo part
            if (numbDec == 0)
            {
                englishTranslation = NumberToWords(numb) + " Naira Only";
            }
            ///////for amount that does not have a naira part.
            if (numb == 0)
            {
                englishTranslation = "NIL";
            }
            ///////for amount that has a kobo and naira part..
            if (numbDec != 0)
            {
                englishTranslation = NumberToWords(numb).ToString() + " Naira " + NumberToWords(numbDec).ToString() + " Kobo Only.";
            }

            return englishTranslation;
        }

        public string NumberToWords(long number)
        {
            //////////////Applying the Zero Rule
            // Zero rule
            if (number == 0)
            {
                return _smallNumbers[0];
            }
            else
            {
                ///////////////Separating the Number into Three-Digit Groups

                // Array to hold four three-digit groups
                long[] digitGroups = new long[5];

                // Ensure a positive number to extract from
                long positive = Math.Abs(number);

                // Extract the three-digit groups
                for (int i = 0; i < 5; i++)
                {
                    digitGroups[i] = positive % 1000;
                    positive /= 1000;
                }

                //////////////Converting a Three-Digit Group
                // Convert each three-digit group to words
                string[] groupText = new string[5];

                for (int i = 0; i < 5; i++)
                {
                    groupText[i] = ThreeDigitGroupToWords(digitGroups[i]);
                }



                ////////////////Recombining the Three-Digit Groups

                // Recombine the three-digit groups
                string combined = groupText[0];
                bool appendAnd;

                // Determine whether an 'and' is needed
                appendAnd = (digitGroups[0] > 0) && (digitGroups[0] < 100);

                // Process the remaining groups in turn, smallest to largest
                for (int i = 1; i < 5; i++)
                {
                    // Only add non-zero items
                    if (digitGroups[i] != 0)
                    {
                        // Build the string to add as a prefix
                        string prefix = groupText[i] + " " + _scaleNumbers[i];

                        if (combined.Length != 0)
                            prefix += appendAnd ? " and " : ", ";

                        // Opportunity to add 'and' is ended
                        appendAnd = false;

                        // Add the three-digit group to the combined string
                        combined = prefix + combined;
                    }
                }


                /////////////////Applying the Negative Rule

                // Negative rule
                if (number < 0)
                    combined = "Negative " + combined;

                return combined;
            }
        }

        // Single-digit and small number names
        private string[] _smallNumbers = new string[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        // Tens number names from twenty upwards
        private string[] _tens = new string[] { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        // Scale number names for use during recombination
        private string[] _scaleNumbers = new string[] { "", "Thousand", "Million", "Billion", "Trillion" };

        // Converts a three-digit group into English words
        private string ThreeDigitGroupToWords(long threeDigits)
        {
            /////////////////Applying The Hundreds Rules
            // Initialise the return text
            string groupText = "";

            // Determine the hundreds and the remainder
            int hundreds = Convert.ToInt32(threeDigits / 100);

            int tensUnits = Convert.ToInt32(threeDigits % 100);

            // Hundreds rules
            if (hundreds != 0)
            {
                groupText += _smallNumbers[hundreds] + " Hundred";

                if (tensUnits != 0)
                {
                    groupText += " and ";
                }
            }

            ////////////////////Applying The Tens Rules

            // Determine the tens and units
            int tens = tensUnits / 10;
            int units = tensUnits % 10;

            // Tens rules
            if (tens >= 2)
            {
                groupText += _tens[tens];
                if (units != 0)
                {
                    groupText += " " + _smallNumbers[units];
                }
            }
            else if (tensUnits != 0)
            {
                groupText += _smallNumbers[tensUnits];
            }

            return groupText;
            //////Recombining the Three-Digit Groups
        }

    }
}
