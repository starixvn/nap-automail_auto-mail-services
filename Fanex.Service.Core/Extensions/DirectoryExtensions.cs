// -----------------------------------------------------------------------
// <copyright file="DirectoryExtensions.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Extensions
{
    using System.IO;
    using System.Net;
    using Fanex.Common;
    using Fanex.Service.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DirectoryExtensions
    {
        private static readonly int _MaxNumberOfDirectories = 3;

        public static void CheckAndCreateDirectory(string rootPath, string path)
        {
            if (AppSettingBase.IsAuthenticate)
            {
                CheckAndCreateDirectoryWithAuthenticate(rootPath, path);
            }
            else
            {
                CheckAndCreateDirectoryWithNoneAuthenticate(path);
            }
        }

        private static void CheckAndCreateDirectoryWithNoneAuthenticate(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void CheckAndCreateDirectoryWithAuthenticate(string rootPath, string path)
        {
            string password = AppSettingBase.Password;
            string username = AppSettingBase.UserName;

            if (AppSettingBase.IsEncryptPassword)
            {
                password = Cryptography.CfsDeCrypt(password);
            }

            NetworkCredential networkCredential = new NetworkCredential(username, password);

            using (new NetworkConnection(rootPath, networkCredential))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path of directory.</param>
        public static void DeleteDirectory(string rootPath, string path)
        {
            if (AppSettingBase.IsAuthenticate)
            {
                DeleteDirectoryWithAuthenticate(rootPath, path);
            }
            else
            {
                DeleteDirectoryWithNoneAuthenticate(path);
            }
        }

        private static void DeleteDirectoryWithNoneAuthenticate(string path)
        {
            var directories = Directory.GetDirectories(path);
            var directoryLength = directories.Length;
            if (directoryLength > _MaxNumberOfDirectories)
            {
                //// Delete old data. Remain 3 lasted month data.
                for (int i = 0; i < directoryLength - _MaxNumberOfDirectories; i++)
                {
                    var directory = directories[i];
                    Directory.Delete(directory, true);
                }
            }
        }

        private static void DeleteDirectoryWithAuthenticate(string rootPath, string path)
        {
            string password = AppSettingBase.Password;
            string username = AppSettingBase.UserName;

            if (AppSettingBase.IsEncryptPassword)
            {
                password = Cryptography.CfsDeCrypt(password);
            }

            NetworkCredential networkCredential = new NetworkCredential(username, password);

            using (new NetworkConnection(rootPath, networkCredential))
            {
                var directories = Directory.GetDirectories(path);
                var directoryLength = directories.Length;
                if (directoryLength > _MaxNumberOfDirectories)
                {
                    //// Delete old data. Remain 3 lasted month data.
                    for (int i = 0; i < directoryLength - _MaxNumberOfDirectories; i++)
                    {
                        var directory = directories[i];
                        Directory.Delete(directory, true);
                    }
                }
            }
        }
    }
}