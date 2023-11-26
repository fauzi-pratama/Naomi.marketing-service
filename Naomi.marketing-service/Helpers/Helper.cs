namespace Naomi.marketing_service.Helpers
{
    public class Helper
    {
        private const string cstrKey = "AcEpAsTiBiSa";
        private static DateTime cstrDate = Convert.ToDateTime("12/12/2012");
        public static string fEncrypt(string strInput, string sKey, DateTime dDate)
        {
            string[] s = new string[0];
            string[] sK = new string[0];
            string[] sR = new string[0];
            string[] sD = new string[0];
            int intLenKey;
            int intLimit;
            int intLenInput;
            string strOut;
            string strDate;
            string temp;
            try
            {
                if (sKey == "") sKey = cstrKey;
                if (dDate == Convert.ToDateTime("1/1/1900")) dDate = cstrDate;

                strDate = Convert.ToDateTime(dDate).ToString("ddMMyyyy");
                intLenInput = strInput.Length;
                intLenKey = sKey.Length;

                if (intLenInput > intLenKey)
                {
                    sKey = mfAddX(sKey, intLenInput - intLenKey);
                    intLimit = intLenInput;
                }
                else if (intLenKey > intLenInput)
                {
                    sKey = sKey.Substring(sKey.Length - intLenInput, intLenInput);
                    intLimit = intLenInput;
                }
                else
                {
                    intLimit = intLenKey;
                }

                strDate = mfFitStr(strDate, intLimit);

                Array.Resize(ref sK, intLimit);
                Array.Resize(ref sD, intLimit);
                Array.Resize(ref s, intLimit);
                Array.Resize(ref sR, intLimit);

                for (int i = 0; i <= intLimit - 1; i++)
                {
                    sK[i] = sKey.Substring(i, 1);
                }

                for (int i = 0; i <= intLimit - 1; i++)
                {
                    sD[i] = strDate.Substring(i, 1);
                }

                for (int i = 0; i <= intLimit - 1; i++)
                {
                    s[i] = strInput.Substring(i, 1);
                }

                for (int i = 0; i <= intLimit - 1; i++)
                {
                    temp = "00" + String.Format("{0:X}", (Convert.ToInt32(Convert.ToChar(s[i])) ^ Convert.ToInt32(Convert.ToChar(sK[i])) ^ Convert.ToInt32(Convert.ToChar(sD[i]))));
                    sR[i] = temp.Substring(temp.Length - 2, 2);
                }

                strOut = "";

                for (int i = 0; i <= intLimit - 1; i++)
                {
                    strOut = strOut + sR[i];
                }

                return strOut;
            }
            catch (Exception ex)
            {
                strOut = ex.Message;
                return strOut;
            }
        }
        public static string fDecrypt(string sInput, string sKey, DateTime dDate)
        {
            string[] s = new string[0];
            string[] sK = new string[0];
            string[] sR = new string[0];
            string[] sD = new string[0];
            int intLenInputan;
            int intLenKey;
            string strOut;
            string strDate;
            try
            {
                if (sKey == "") sKey = cstrKey;
                if (dDate == Convert.ToDateTime("1/1/1900")) dDate = cstrDate;

                strDate = Convert.ToDateTime(dDate).ToString("ddMMyyyy");

                intLenInputan = sInput.Length / 2;
                intLenKey = sKey.Length;

                strDate = mfFitStr(strDate, intLenInputan);

                if (intLenKey > intLenInputan)
                    sKey = sKey.Substring(sKey.Length - intLenInputan, intLenInputan);

                if (intLenKey < intLenInputan)
                    sKey = mfAddX(sKey, intLenInputan - intLenKey);

                Array.Resize(ref sK, intLenInputan);
                Array.Resize(ref sD, intLenInputan);
                Array.Resize(ref s, intLenInputan);
                Array.Resize(ref sR, intLenInputan);

                for (int i = 0; i <= intLenInputan - 1; i++)
                {
                    sK[i] = sKey.Substring(i, 1);
                }

                for (int i = 0; i <= intLenInputan - 1; i++)
                {
                    sD[i] = strDate.Substring(i, 1);
                }

                for (int i = 0; i <= intLenInputan - 1; i++)
                {
                    s[i] = sInput.Substring(i * 2, 2);
                }

                for (int i = 0; i <= intLenInputan - 1; i++)
                {
                    sR[i] = (int.Parse(s[i], System.Globalization.NumberStyles.HexNumber) ^ Convert.ToInt32(Convert.ToChar(sK[i])) ^ Convert.ToInt32(Convert.ToChar(sD[i]))).ToString();
                }

                strOut = "";

                for (int i = 0; i <= intLenInputan - 1; i++)
                {
                    strOut += char.ConvertFromUtf32(Convert.ToInt32(sR[i])).ToString();
                }

                return strOut;
            }
            catch (Exception ex)
            {
                strOut = ex.Message;
                return strOut;
            }
        }
        public static string mfFitStr(string strInput, int intLength)
        {
            if (strInput.Length > intLength)
            {
                strInput = strInput.Substring(0, intLength);
            }
            else
            {
                strInput = mfAddX(strInput, intLength - strInput.Length);
            }
            return strInput;
        }
        public static string mfAddX(string strInput, int intAdd)
        {
            int x = 0;
            int y = 0;

            x = strInput.Length;
            y = strInput.Length + intAdd - 1;

            for (int i = x; i <= y; i++)
            {
                strInput += "x";
            }
            return strInput;
        }
    }
}
