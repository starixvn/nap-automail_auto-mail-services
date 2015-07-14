// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.TurnoverWatcher.Business
{
    using System;
    using System.Configuration;
    using Fanex.Service.Core.Utilities;
    using Fanex.Service.Core.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings : AppSettingBase, ISettings
    {
        public static string DataDirectory { get; set; }

        public static new string ExportDataDirectoryWithYearMonthDay
        {
            get { return ExportDataDirectoryWithYearMonthDay(DataDirectory); }
        }

        public static string ExcelPasswordProtected
        {
            get;
            set;
        }

        public static int FunctionId
        {
            get;
            set;
        }

        public static bool IsTest
        {
            get;
            set;
        }
    }
}