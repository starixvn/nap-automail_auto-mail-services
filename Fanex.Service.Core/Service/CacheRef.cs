// -----------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service
{
    using Fanex.Common;
    using Fanex.Service.Core.Constants;
    using RemotingRef;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CacheRef
    {
        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(
                Converter.ToString2(leagueId),
                GetSimpleLanguage(LanguageTypeConstants.EnUS),
                string.Empty);
        }

        private static string GetSimpleLanguage(string lang)
        {
            switch (lang)
            {
                case LanguageTypeConstants.EnUS:
                    return LanguageTypeConstants.En;

                case LanguageTypeConstants.ZhTW:
                    return LanguageTypeConstants.Ch; // Traditional
                case LanguageTypeConstants.ZhCn:
                    return LanguageTypeConstants.Cs; // Simplified
                case LanguageTypeConstants.JaJp:
                    return LanguageTypeConstants.Jp;

                case LanguageTypeConstants.ThTh:
                    return LanguageTypeConstants.Th; // Thai
                default:
                    return LanguageTypeConstants.En;
            }
        }
    }
}