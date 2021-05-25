using System;

namespace HelperLibrary
{
    /// <summary>
    /// Implements the Password class.
    /// </summary>
    ///
    public static class PasswordHelper
    {


        /// <summary>
        /// Generates a random password.
        /// </summary>
        ///
        public static string GenerateRandomPassword(string baseString)
        {

            try
            {

                Random randomNumberGenerator;
                randomNumberGenerator = new Random();

                decimal randomNumber;


                // If the base string less than six characters in length:
                if (baseString.Length < 6)
                {
                    // generate a random six digit number,
                    randomNumber = randomNumberGenerator.Next(100000, 999999);
                    randomNumber = Math.Round(randomNumber);

                    // append the random number to the base string,
                    baseString += randomNumber.ToString();

                    // only take the first six character of the base string.
                    baseString = baseString.Substring(0, 6);
                }


                // If the base string greater than 22 characters in length:
                if (baseString.Length > 22)
                {
                    // only take the first 22 character of the base string.
                    baseString = baseString.Substring(0, 22);
                }


                // Initialise the random password to the base string,
                string randomPassword;
                randomPassword = baseString;

                // generate a random number,
                randomNumber = randomNumberGenerator.Next(10, 99);
                randomNumber = Math.Round(randomNumber);

                // append the random number to the password,
                randomPassword += randomNumber.ToString();

                // generate another random number,
                randomNumber = randomNumberGenerator.Next(10, 99);
                randomNumber = Math.Round(randomNumber);

                // append the random number to the password.
                randomPassword += randomNumber.ToString();


                // Return the random password.
                return randomPassword;

            }


            // If an exception occurs:
            catch (Exception e)
            {
                // throw the exception encountered.
                throw e;
            }

        }




        /// <summary>
        /// Encrypts the specified clear-text password.
        /// </summary>
        ///
        public static string Encrypt(string clearPassword)
        {

            // Encrypt the specified clear-text password:
            try
            {
                // instantiate the random number generator class,
                Random randomNumberGenerator;
                randomNumberGenerator = new Random();

                // initialise the encrypted password to "blank",
                string encryptedPassword;
                encryptedPassword = "";

                // for each character in the clear-text password:
                for (int index = 0; (index < clearPassword.Length); index++)
                {
                    // generate a random number,
                    int randomNumber;
                    randomNumber =
                        GenerateRandomNumber(randomNumberGenerator);

                    // obtain the ascii value of the currect character in the
                    // clear-text password,
                    int clearPasswordCharacterValue;
                    clearPasswordCharacterValue =
                        (int)clearPassword[index];

                    // calculate the encrypted character value for the current
                    // character in the clear-text password,
                    long encryptedPasswordCharacterValue;
                    encryptedPasswordCharacterValue =
                        clearPasswordCharacterValue * randomNumber;

                    // format the random number by padding it with zero's to a
                    // maximum length of six characters, then append it to the
                    // encrypted password,
                    encryptedPassword +=
                        randomNumber.ToString().PadLeft(2, '0');

                    // format the encrypted character value by padding it with
                    // zero's to a maximum length of eight characters, then
                    // append it to the encrypted password.
                    encryptedPassword +=
                        encryptedPasswordCharacterValue.ToString().PadLeft(4, '0');
                }

                // return the encrypted password.
                return encryptedPassword;
            }


            // If an exception occurs:
            catch (Exception e)
            {
                // throw the exception encountered.
                throw e;
            }

        }




        /// <summary>
        /// Decrypts the specified encrypted password.
        /// </summary>
        ///
        public static string Decrypt(string encryptedPassword)
        {

            // Decrypt the specified encrypted password:
            try
            {
                // initialise the clear-text password to "blank",
                string clearPassword = "";

                // while the encrypted password's length is greater than zero:
                while (encryptedPassword.Length > 0)
                {

                    // If a dubious number of characters remain in the
                    // encrypted password:
                    if (encryptedPassword.Length < 6)
                    {
                        // pad the encrypted password with zeros to ensure the
                        // required minimum number of characters are available.
                        encryptedPassword = encryptedPassword.PadRight(6, '0');
                    }


                    // Obtain the random number from the encrypted password:

                    // obtain the first six characters from the the encrypted
                    // password, as the random number string,
                    string randomNumberString;
                    randomNumberString = encryptedPassword.Substring(0, 2);

                    int randomNumber;

                    // if the random number string cannot be converted into an
                    // (integer) number:
                    if (Int32.TryParse(randomNumberString, out randomNumber) ==
                        false)
                    {
                        // throw a exception,
                        throw new Exception("Unable to decrypt password.");
                    }

                    // obtain the remainder of the encrypted password.
                    encryptedPassword = encryptedPassword.Substring(2);


                    // Obtain the encrypted character from the encrypted
                    // password:

                    // obtain the first eight characters from the the encrypted
                    // password, as the encrypted character string,
                    string encryptedPasswordCharacterString;
                    encryptedPasswordCharacterString =
                        encryptedPassword.Substring(0, 4);

                    int encryptedPasswordCharacterValue;

                    // if the encrypted character string cannot be converted
                    // into an (integer) number:
                    if (Int32.TryParse(encryptedPasswordCharacterString,
                        out encryptedPasswordCharacterValue) == false)
                    {
                        // throw a exception,
                        throw new Exception("Unable to decrypt password.");
                    }

                    // obtain the remainder of the encrypted password.
                    encryptedPassword = encryptedPassword.Substring(4);


                    // Decrypt the encrypted character value.
                    char decryptedPasswordCharacter;
                    decryptedPasswordCharacter =
                        (char)(encryptedPasswordCharacterValue / randomNumber);

                    // Append the decrypted character to the clear-text
                    // password.
                    clearPassword += decryptedPasswordCharacter;

                }

                // Return the clear-text password.
                return clearPassword;
            }

            // If an exception occurs:
            catch (Exception e)
            {
                // throw the exception encountered.
                throw e;
            }

        }




        /// <summary>
        /// Validates the specified password, i.e.:
        ///  - Ensures the password in at least seven characters in length.
        ///  - Searches the specified password for both alphabetic and numeric
        ///    characters.
        /// </summary>
        ///
        public static void Validate(string password)
        {

            // Validate the specified password:
            try
            {

                // If the specified password is less than seven characters in
                // length:
                if ((password.Length < 7))
                {
                    // throw the corresponding exception.
                    throw new Exception("The specified password cannot be less than seven characters in length.");
                }


                // If the specified password is greater than twenty-six
                // characters in length:
                if ((password.Length > 26))
                {
                    // throw the corresponding exception.
                    throw new Exception("The specified password cannot be greater than twenty-six characters in length.");
                }


                // Search the specified password for alphabetic characters:

                bool hasAlphabeticCharacters = false;

                int index = 0;

                // Whilst an alphabetic character has not been found in the
                // specified password, and the index has not reached the end
                // of the specified password:
                while ((hasAlphabeticCharacters == false) &&
                       (index < password.Length))
                {
                    // if the currect character is an alphabetic character:
                    if ((password[index] >= 'A') &&
                        (password[index] <= 'z'))
                    {
                        // set hasAlphabeticCharacters to true to indicate that
                        // an alphabetic character has been found in the
                        // specified password.
                        hasAlphabeticCharacters = true;
                    }

                    index++;
                }


                // Search the specified password for numeric characters:

                bool hasNumericCharacters = false;

                index = 0;

                // Whilst a numeric character has not been found in the
                // specified password, and the index has not reached the end
                // of the specified password:
                while ((hasNumericCharacters == false) &&
                       (index < password.Length))
                {
                    // if the currect character in the specified password is a
                    // numeric character:
                    if ((password[index] >= '0') &&
                        (password[index] <= '9'))
                    {
                        // set hasNumericCharacters to true to indicate that a
                        // numeric character has been found in the specified
                        // password.
                        hasNumericCharacters = true;
                    }

                    index++;
                }


                // If the specified password does not contain both alphabetic
                // and numeric characters:
                if ((hasAlphabeticCharacters == false) ||
                    (hasNumericCharacters == false))
                {
                    // throw and exception to indicate that the specified
                    // password does not contain both alphabetic and numeric
                    // characters.
                    throw new Exception("The specified password must contain both alphabetic and numeric characters.");
                }

            }


            // If an exception occurs:
            catch (Exception e)
            {
                // throw the exception encountered.
                throw e;
            }

        }




        /// <summary>
        /// Generates a "random" number, between 48 and 57.
        /// </summary>
        ///
        private static int GenerateRandomNumber(Random randomNumberGenerator)
        {

            // Generate a "random" number, between 48 and 57:

            try
            {
                // generate a random number and cast it to a string,
                string randomNumberString;
                randomNumberString = randomNumberGenerator.Next().ToString();

                // obtain the ascii value of the last character of the random
                // number string,
                int randomNumber;
                randomNumber = (int)randomNumberString[(randomNumberString.Length - 1)];

                // return the "random" number.
                return randomNumber;
            }

            // If an exception occurs:
            catch (Exception e)
            {
                // throw the exception encountered.
                throw e;
            }

        }
    }
}

