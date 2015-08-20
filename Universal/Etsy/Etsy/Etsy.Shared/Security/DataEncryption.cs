// Using the DataProtectionProvider class: http://msdn.microsoft.com/en-us/library/windows/apps/windows.security.cryptography.dataprotection.dataprotectionprovider.aspx
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;
using System.Threading.Tasks;

namespace Etsy.Security
{
    public sealed class DataEncryption
    {
        /// <summary>
        /// Encrypt a string and limit access to the LOCAL=user
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userDescriptor"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<IBuffer> ProtectString(string message, string userDescriptor)
        {
            // Create a DataProtectionProvider object for the specified descriptor.
            DataProtectionProvider Provider = new DataProtectionProvider(userDescriptor);

            // Encode the plaintext input message to a buffer.
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;  // same as the decrypting function
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(message, encoding);

            // Encrypt the message.
            IBuffer buffProtected = await Provider.ProtectAsync(buffMsg);   // wait

            return buffProtected;   // Return encrypted message
        }

        /// <summary>
        /// Encrypt an input stream and output to another stream
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        /// <param name="userDescriptor"></param>
        /// <returns></returns>
        public static async Task ProtectStreamToStream(IInputStream inStream, IOutputStream outStream, string userDescriptor)
        {
            // Create a DataProtectionProvider object for the specified descriptor.
            DataProtectionProvider Provider = new DataProtectionProvider(userDescriptor);

            await Provider.ProtectStreamAsync(inStream, outStream);

        }

        /// <summary>
        /// Decrypt and return the protected data as a plain-text string
        /// </summary>
        /// <param name="buffProtected"></param>
        /// <param name="encoding"></param>
        /// <returns>plain-text string of the encrypted data buffer</returns>
        public static async Task<String> DecryptData(IBuffer buffProtected)
        {
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;  // same as the encrypting function

            // Create a DataProtectionProvider object.
            DataProtectionProvider Provider = new DataProtectionProvider();

            // Decrypt the protected message specified on input.
            IBuffer buffUnprotected = await Provider.UnprotectAsync(buffProtected);

            // Execution of the SampleUnprotectData method resumes here
            // after the awaited task (Provider.UnprotectAsync) completes
            // Convert the unprotected message from an IBuffer object to a string.
            String strClearText = CryptographicBuffer.ConvertBinaryToString(encoding, buffUnprotected);

            // Return the plaintext string.
            return strClearText;
        }

        /// <summary>
        /// Decrypt an input stream and output to another stream
        /// </summary>
        /// <param name="readStream"></param>
        /// <param name="outStream"></param>
        /// <returns></returns>
        public static async Task DecryptStream(IInputStream readStream, IOutputStream outStream, string userDescriptor)
        {
            // Create a DataProtectionProvider object for the specified descriptor.
            DataProtectionProvider Provider = new DataProtectionProvider(userDescriptor);

            await Provider.UnprotectStreamAsync(readStream, outStream);     // decrypt and output

            return;
        }
    }
}
