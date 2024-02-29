using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPFBase.Api.Extensions
{
    public class SystemBase
    {
        public SystemBase()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        //新增生成随机字符串方法(使用RNGCryptoServiceProvider 做种)
        private static readonly int defaultLength = 8;

        private static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        private static string BuildRndCodeAll(int strLen)
        {
            System.Random RandomObj = new System.Random(GetNewSeed());
            string buildRndCodeReturn = null;
            for (int i = 0; i < strLen; i++)
            {
                buildRndCodeReturn += (char)RandomObj.Next(33, 125);
            }
            return buildRndCodeReturn;
        }

        public static string GetRndStrOfAll()
        {
            return BuildRndCodeAll(defaultLength);
        }

        public static string GetRndStrOfAll(int LenOf)
        {
            return BuildRndCodeAll(LenOf);
        }

        private static string sCharLow = "abcdefghijklmnopqrstuvwxyz";
        private static string sCharUpp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string sNumber = "0123456789";

        private static string BuildRndCodeOnly(string StrOf, int strLen, bool addLine)
        {
            System.Random RandomObj = new System.Random(GetNewSeed());
            string buildRndCodeReturn = null;
            string buildRndCodeReturnAddLine = null;
            for (int i = 0; i < strLen; i++)
            {
                string rndstr = StrOf.Substring(RandomObj.Next(0, StrOf.Length - 1), 1);
                buildRndCodeReturn += rndstr;
                buildRndCodeReturnAddLine += rndstr;
                //addline为每5个字符串加一个-做分割；
                if (buildRndCodeReturn.Length % 5 == 0 && (i + 1) < strLen)
                {
                    buildRndCodeReturnAddLine += "-";
                }
            }
            if (addLine)
            {
                return buildRndCodeReturnAddLine;
            }
            else
            {
                return buildRndCodeReturn;
            }
        }

        public static string GetRndStrOnlyFor(bool addLine)
        {
            return BuildRndCodeOnly(sCharUpp + sNumber, defaultLength, addLine);
        }

        public static string GetRndStrOnlyFor(int LenOf, bool addLine)
        {
            return BuildRndCodeOnly(sCharUpp + sNumber, LenOf, addLine);
        }

        public static string GetRndStrOnlyFor(bool bUseLow, bool bUseNumber, bool addLine)
        {
            string strTmp = sCharUpp;
            if (bUseLow) strTmp += sCharLow;
            if (bUseNumber) strTmp += sNumber;

            return BuildRndCodeOnly(strTmp, defaultLength, addLine);
        }

        public static string GetRndStrOnlyFor(int LenOf, bool bUseLow, bool bUseNumber, bool addLine)
        {
            string strTmp = sCharUpp;
            if (bUseLow) strTmp += sCharLow;
            if (bUseNumber) strTmp += sNumber;

            return BuildRndCodeOnly(strTmp, LenOf, addLine);
        }

        //原有生成随机字符串方法（以时间做种）
        static Random rnd = new Random(unchecked((int)System.DateTime.Now.Ticks));

        public static string GenRndString(int StringLength)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            string strRndString = string.Empty;

            for (int i = 0; i < StringLength; i++)
            {
                strRndString += chars[rnd.Next(0, chars.Length - 1)].ToString();
            }

            return strRndString;
        }

        public static string GenRndIntNo(int intlength)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string strRndNo = string.Empty;
            for (int i = 0; i < intlength; i++)
            {
                strRndNo += chars[rnd.Next(0, chars.Length - 1)].ToString();
            }
            return strRndNo;

        }

        public static string FormatDataTime(System.DateTime dtIn, int Style)
        {
            string strResult = "";
            if (dtIn == null)
                return "";
            strResult = dtIn.Month.ToString() + "月" + dtIn.Day.ToString();
            return strResult;
        }

        public static string GenDateRndString(int RndType, int StringLength)
        {
            System.DateTime dtNow = System.DateTime.Now;
            string strRndString = string.Empty;
            string strYear = dtNow.Year.ToString();
            string strMonth = dtNow.Month.ToString();
            string strDay = dtNow.Day.ToString();
            string strHour = dtNow.Hour.ToString();
            string strMinute = dtNow.Minute.ToString();
            string strSecond = dtNow.Second.ToString();

            if (strMonth.Length < 2)
                strMonth = "0" + strMonth;
            if (strDay.Length < 2)
                strDay = "0" + strDay;
            if (strHour.Length < 2)
                strHour = "0" + strHour;
            if (strMinute.Length < 2)
                strMinute = "0" + strMinute;
            if (strSecond.Length < 2)
                strSecond = "0" + strSecond;


            switch (RndType)
            {
                case 1:
                    strRndString = strYear;
                    break;
                case 2:
                    strRndString = strYear + strMonth;
                    break;
                case 4:
                    strRndString = strYear + strMonth + strDay + strHour + strMinute + strSecond;
                    break;
                default:
                    strRndString = strYear + strMonth + strDay;
                    break;
            }
            if (strRndString.Length >= StringLength)
            {
                strRndString = strRndString.Substring(0, StringLength);
            }
            else
            {
                strRndString = strRndString + GenRndString(StringLength - strRndString.Length);
            }

            return strRndString;
        }
 
    }
}
