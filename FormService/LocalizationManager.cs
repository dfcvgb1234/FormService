using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormService
{
    static class LocalizationManager
    {

        static string[] daymonthyear =
        {
            "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy", "d-M-yyyy", "dd-MM-y", "dd-MM-yy", "d-MM-y", "dd-M-y", "d-M-y", "dd-MM-yy", "d-MM-yy", "dd-M-yy", "d-M-yy",
            "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "dd/MM/y", "dd/MM/yy", "d/MM/y", "dd/M/y", "d/M/y", "dd/MM/yy", "d/MM/yy", "dd/M/yy", "d/M/yy",
            "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy", "d.M.yyyy", "dd.MM.y", "dd.MM.yy", "d.MM.y", "dd.M.y", "d.M.y", "dd.MM.yy", "d.MM.yy", "dd.M.yy", "d.M.yy",
            "dd_MM_yyyy", "d_MM_yyyy", "dd_M_yyyy", "d_M_yyyy", "dd_MM_y", "dd_MM_yy", "d_MM_y", "dd_M_y", "d_M_y", "dd_MM_yy", "d_MM_yy", "dd_M_yy", "d_M_yy",
            "ddMMyyyy", "dMMyyyy", "ddMyyyy", "dMyyyy", "ddMMy", "ddMMyy", "dMMy", "ddMy", "dMy", "ddMMyy", "dMMyy", "ddMyy", "dMyy",
            "dd MM yyyy", "d MM yyyy", "dd M yyyy", "d M yyyy", "dd MM y", "dd MM yy", "d MM y", "dd M y", "d M y", "dd MM yy", "d MM yy", "dd M yy", "d M yy",
            "dd:MM:yyyy", "d:MM:yyyy", "dd:M:yyyy", "d:M:yyyy", "dd:MM:y", "dd:MM:yy", "d:MM:y", "dd:M:y", "d:M:y", "dd:MM:yy", "d:MM:yy", "dd:M:yy", "d:M:yy"
        };
        static string[] monthdayyear =
        {
            "MM-dd-yyyy", "M-dd-yyyy", "MM-d-yyyy", "M-d-yyyy", "MM-dd-y", "MM-dd-yy", "M-dd-y", "MM-d-y", "M-d-y", "MM-d-yy", "M-d-yy", "MM-d-yy", "M-d-yy",
            "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy", "MM/dd/y", "MM/dd/yy", "M/dd/y", "MM/d/y", "M/d/y", "MM/d/yy", "M/d/yy", "MM/d/yy", "M/d/yy",
            "MM.dd.yyyy", "M.dd.yyyy", "MM.d.yyyy", "M.d.yyyy", "MM.dd.y", "MM.dd.yy", "M.dd.y", "MM.d.y", "M.d.y", "MM.d.yy", "M.d.yy", "MM.d.yy", "M.d.yy",
            "MM_dd_yyyy", "M_dd_yyyy", "MM_d_yyyy", "M_d_yyyy", "MM_dd_y", "MM_dd_yy", "M_dd_y", "MM_d_y", "M_d_y", "MM_d_yy", "M_d_yy", "MM_d_yy", "M_d_yy",
            "MMddyyyy", "Mddyyyy", "MMdyyyy", "Mdyyyy", "MMddy", "MMddyy", "Mddy", "MMdy", "Mdy", "MMdyy", "Mdyy", "MMdyy", "Mdyy",
            "MM dd yyyy", "M dd yyyy", "MM d yyyy", "M d yyyy", "MM dd y", "MM dd yy", "M dd y", "MM d y", "M d y", "MM d yy", "M d yy", "MM d yy", "M d yy",
            "MM:dd:yyyy", "M:dd:yyyy", "MM:d:yyyy", "M:d:yyyy", "MM:dd:y", "MM:dd:yy", "M:dd:y", "MM:d:y", "M:d:y", "MM:d:yy", "M:d:yy", "MM:d:yy", "M:d:yy"
        };
        static string[] yearmonthday =
        {
            "yyyy-MM-dd", "yyyy-MM-d", "yyyy-M-dd", "yyyy-M-d", "y-MM-d", "yy-MM-dd", "y-MM-d", "y-M-d", "y-M-d", "yy-MM-dd", "yy-MM-d", "yy-M-dd", "yy-M-d",
            "yyyy/MM/dd", "yyyy/MM/d", "yyyy/M/dd", "yyyy/M/d", "y/MM/d", "yy/MM/dd", "y/MM/d", "y/M/d", "y/M/d", "yy/MM/dd", "yy/MM/d", "yy/M/dd", "yy/M/d",
            "yyyy.MM.dd", "yyyy.MM.d", "yyyy.M.dd", "yyyy.M.d", "y.MM.d", "yy.MM.dd", "y.MM.d", "y.M.d", "y.M.d", "yy.MM.dd", "yy.MM.d", "yy.M.dd", "yy.M.d",
            "yyyy_MM_dd", "yyyy_MM_d", "yyyy_M_dd", "yyyy_M_d", "y_MM_d", "yy_MM_dd", "y_MM_d", "y_M_d", "y_M_d", "yy_MM_dd", "yy_MM_d", "yy_M_dd", "yy_M_d",
            "yyyyMMdd", "yyyyMMd", "yyyyMdd", "yyyyMd", "yMMd", "yyMMdd", "yMMd", "yMd", "yMd", "yyMMdd", "yyMMd", "yyMdd", "yyMd",
            "yyyy MM dd", "yyyy MM d", "yyyy M dd", "yyyy M d", "y MM d", "yy MM dd", "y MM d", "y M d", "y M d", "yy MM dd", "yy MM d", "yy M dd", "yy M d",
            "yyyy:MM:dd", "yyyy:MM:d", "yyyy:M:dd", "yyyy:M:d", "y:MM:d", "yy:MM:dd", "y:MM:d", "y:M:d", "y:M:d", "yy:MM:dd", "yy:MM:d", "yy:M:dd", "yy:M:d"
        };
        static string[] yeardaymonth =
        {
            "yyyy-dd-MM", "yyyy-d-MM", "yyyy-dd-M", "yyyy-d-M", "y-d-MM", "yy-dd-MM", "y-d-MM", "y-d-M", "y-d-M", "yy-dd-MM", "yy-d-MM", "yy-dd-MM", "yy-d-M",
            "yyyy/dd/MM", "yyyy/d/MM", "yyyy/dd/M", "yyyy/d/M", "y/d/MM", "yy/dd/MM", "y/d/MM", "y/d/M", "y/d/M", "yy/dd/MM", "yy/d/MM", "yy/dd/MM", "yy/d/M",
            "yyyy.dd.MM", "yyyy.d.MM", "yyyy.dd.M", "yyyy.d.M", "y.d.MM", "yy.dd.MM", "y.d.MM", "y.d.M", "y.d.M", "yy.dd.MM", "yy.d.MM", "yy.dd.MM", "yy.d.M",
            "yyyy_dd_MM", "yyyy_d_MM", "yyyy_dd_M", "yyyy_d_M", "y_d_MM", "yy_dd_MM", "y_d_MM", "y_d_M", "y_d_M", "yy_dd_MM", "yy_d_MM", "yy_dd_MM", "yy_d_M",
            "yyyyddMM", "yyyydMM", "yyyyddM", "yyyydM", "ydMM", "yyddMM", "ydMM", "ydM", "ydM", "yyddMM", "yydMM", "yyddMM", "yydM",
            "yyyy dd MM", "yyyy d MM", "yyyy dd M", "yyyy d M", "y d MM", "yy dd MM", "y d MM", "y d M", "y d M", "yy dd MM", "yy d MM", "yy dd MM", "yy d M",
            "yyyy:dd:MM", "yyyy:d:MM", "yyyy:dd:M", "yyyy:d:M", "y:d:MM", "yy:dd:MM", "y:d:MM", "y:d:M", "y:d:M", "yy:dd:MM", "yy:d:MM", "yy:dd:MM", "yy:d:M"
        };


        public static string GetCurrentLocalization()
        {
            return CultureInfo.CurrentCulture.Name;
        }
        public static string GetCurrentShortDateFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }
        public static string GetCurrentLongDateFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
        }
        public static string GetCurrentShortTimeFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        }
        public static string GetCurrentLongTimeFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
        }
        public static DateTime ParseDateTimeFromCurrentFormat(string timeString)
        {
            try
            {
                return DateTime.ParseExact(timeString, GetCurrentShortDateFormat(), CultureInfo.CurrentCulture);
            }
            catch(Exception ex)
            {
                if (XMLSettings.g_bUseWaterfallFormatter)
                {
                    return ParseDateTimeInWaterfall(timeString);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
        public static DateTime ParseDateTimeFromFormat(string timeString, string format)
        {
            return DateTime.ParseExact(timeString, format, CultureInfo.CurrentCulture);
        }
        public static string ConvertDateToSpecificFormat(DateTime time, string format)
        {
            return time.ToString(format);
        }
        public static string ConvertTimeToSpecificFormat(DateTime time, string format)
        {
            return time.ToString(format);
        }
        public static DateTime ParseDateTimeInWaterfall(string strInputDate)
        {
            DateTime dtDate;
            int daysDistance;
            if (DateTime.TryParseExact(strInputDate, daymonthyear, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDate))
            {
                if (XMLSettings.g_bUseDistanceRule)
                {
                    daysDistance = (int)(DateTime.Now.Date - dtDate.Date).TotalDays;
                    if (XMLSettings.g_bDistanceRuleFuture) { if (daysDistance < 0) { daysDistance *= -1; } }
                    if (daysDistance <= XMLSettings.g_iDistanceRuleDays && daysDistance >= 0)
                    {
                        return dtDate;
                    }
                }
                else
                {
                    return dtDate;
                }
            }
            if (DateTime.TryParseExact(strInputDate, monthdayyear, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDate))
            {
                if (XMLSettings.g_bUseDistanceRule)
                {
                    daysDistance = (int)(DateTime.Now.Date - dtDate.Date).TotalDays;
                    if (XMLSettings.g_bDistanceRuleFuture) { if (daysDistance < 0) { daysDistance *= -1; } }
                    if (daysDistance <= XMLSettings.g_iDistanceRuleDays && daysDistance >= 0)
                    {
                        return dtDate;
                    }
                }
                else
                {
                    return dtDate;
                }
            }
            if (DateTime.TryParseExact(strInputDate, yearmonthday, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDate))
            {
                if (XMLSettings.g_bUseDistanceRule)
                {
                    daysDistance = (int)(DateTime.Now.Date - dtDate.Date).TotalDays;
                    if (XMLSettings.g_bDistanceRuleFuture) { if (daysDistance < 0) { daysDistance *= -1; } }
                    if (daysDistance <= XMLSettings.g_iDistanceRuleDays && daysDistance >= 0)
                    {
                        return dtDate;
                    }
                }
                else
                {
                    return dtDate;
                }
            }
            if (DateTime.TryParseExact(strInputDate, yeardaymonth, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDate))
            {
                if (XMLSettings.g_bUseDistanceRule)
                {
                    daysDistance = (int)(DateTime.Now.Date - dtDate.Date).TotalDays;
                    if (XMLSettings.g_bDistanceRuleFuture) { if (daysDistance < 0) { daysDistance *= -1; } }
                    if (daysDistance <= XMLSettings.g_iDistanceRuleDays && daysDistance >= 0)
                    {
                        return dtDate;
                    }
                }
                else
                {
                    return dtDate;
                }
            }
            return DateTime.Now;
        }

    }
}
