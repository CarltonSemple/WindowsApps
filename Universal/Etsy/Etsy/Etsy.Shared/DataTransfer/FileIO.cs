using Etsy.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Etsy.DataTransfer
{
    public class FileIO
    {
        /// <summary>
        /// Encrypt a string and save it to the specified file
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="fileName"></param>
        public static async Task EncryptAndSave(string variable, string fileName)
        {
            IBuffer buffer = await DataEncryption.ProtectString(variable, "LOCAL=user");    // encrypt

            await WriteBufferToFile(buffer, fileName);        // save to file

            return;
        }

        /// <summary>
        /// Write a buffer to a the specified file
        /// </summary>
        private static async Task WriteBufferToFile(IBuffer buffer, string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, 
                                                                    CreationCollisionOption.ReplaceExisting);

            await Windows.Storage.FileIO.WriteBufferAsync(file, buffer);

            return;
        }

        /// <summary>
        /// Read an encrypted buffer from the specified file.
        /// Return a plain-text string after decrypting the buffer
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> ReadEncryptedFileAsString(string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);    // open file

            IBuffer buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);            // read to buffer

            string result = await DataEncryption.DecryptData(buffer);                       // decrypt

            return result;
        }

        /// <summary>
        /// Read a file as text
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> ReadStringFromFile(string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);    // open file
            string result = await Windows.Storage.FileIO.ReadTextAsync(file);

            return result;
        }

        /// <summary>
        /// Serialize an object (JsonSerialize), encrypt it, and save to the specified file
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task SerializeAndSave<X>(X obj, string fileName)
        {
            var serializer = new DataContractJsonSerializer(typeof(X));         // create a serializer of the object's type
            
            MemoryStream memStream = new MemoryStream();                        // open up a new memory stream
            serializer.WriteObject(memStream, obj);                             // Write object to memory stream
            memStream.Seek(0, SeekOrigin.Begin);                            // return to the beginning of the stream

            try
            {
                var storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting); // open file
                var file = await storageFile.OpenAsync(FileAccessMode.ReadWrite);       // get access to the file

                using (var fileStream = file.GetOutputStreamAt(0)) // start at the file's beginning
                {
                    await DataEncryption.ProtectStreamToStream(memStream.AsInputStream(), fileStream, "LOCAL=user");
                    await fileStream.FlushAsync();
                    fileStream.Dispose();
                    file.Dispose();
                }
            }
            catch(Exception e)
            {
                string message = e.Message;
            }
        }

        
        public static async Task<X> DeserializeFromFile<X>(string fileName)
        {
            var serializer = new DataContractJsonSerializer(typeof(X));         // create a serializer of the object's type

            MemoryStream memStream = new MemoryStream();                        // open up a new memory stream

            var storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName); // get file  
            
            try
            {
                var inStream = await storageFile.OpenStreamForReadAsync(); //var inStream = await storageFile.OpenAsync(FileAccessMode.Read))

                await DataEncryption.DecryptStream(inStream.AsInputStream(),
                                                    memStream.AsOutputStream(),
                                                    "LOCAL=user");
            }
            catch (Exception e)
            {
                int i = 0;
            }

            memStream.Seek(0, SeekOrigin.Begin);        // move to stream beginning for reading
            

            return (X)serializer.ReadObject(memStream); // read the object and return it
        }

        /// <summary>
        /// Delete the specified file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task DeleteFile(string fileName)
        {
            var storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            await storageFile.DeleteAsync();

            return;
        }
    }
}
