//CÁC HÀM HỖ TRỢ XỬ LÝ KIỂU CHUỖI VÀ ĐỊNH DẠNG TIỀN THEO CẤU HÌNH HỆ THỐNG MANH.NGUYEN - JAN, 26 - 2011
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace vnaisoft.DataBase.Helper
{
    public static class StringFunctions
    {
        public static string NonUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
    }
    public static class StringHelper
    {
        public static string replaceSpecialCharacter(string str)
        {
            return str.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&#38;").Replace("\'", "&#39;").Replace("\"", "&#34;");
        }
        public static object GetValuePropertyObject(object value, string path)
        {
            Type currentType = value.GetType();

            foreach (string propertyName in path.Split('.'))
            {
                PropertyInfo property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }
            return value;
        }
        public static string ChuyenSo(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;
            var am = "";
            if (number.StartsWith("-"))
            {
                number = number.Replace("-", "");
                am = "Âm ";
            }

            number = number.Replace(".", "").Replace(",", "");
            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (n - j == 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];
            doc = doc.Substring(0, 1).ToUpper() + doc.Substring(1, doc.Length - 1);
            return am + doc.Trim() + " ";
        }
        public static string GetValuePropertyString(object value, string path)
        {
            Type currentType = value.GetType();
            var result = "";
            foreach (string propertyName in path.Split('.'))
            {

                if (propertyName.Contains("[") || propertyName.Contains("]"))
                {
                    var firstIndex = propertyName.IndexOf("[");
                    var lastIndex = propertyName.IndexOf("]");
                    var handle = propertyName.Substring(0, firstIndex);
                    string posString = propertyName.Substring(firstIndex, lastIndex - firstIndex);
                    int pos = int.Parse(Regex.Replace(posString, @"[^0-9]", ""));


                    List<object> list = new List<object>();
                    try
                    {
                        var objectTemp = StringHelper.GetValuePropertyObject(value, handle);
                        list = ((IEnumerable)objectTemp).Cast<object>().ToList();
                    }
                    catch (Exception e) { }
                    value = list[pos];
                    currentType = value.GetType();
                }
                else
                {
                    PropertyInfo property = currentType.GetProperty(propertyName);
                    if (property == null) return "";
                    value = property.GetValue(value, null);
                    currentType = property.PropertyType;
                }
            }
            if (value == null) return "";
            if (value.GetType() == typeof(DateTime?) || value.GetType() == typeof(DateTime))
            {
                if (value != null)
                {
                    var date = (DateTime)value;
                    if (date.Hour == 0 && date.Minute == 0)
                    {
                        result = ((DateTime)value).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        result = ((DateTime)value).ToString("dd/MM/yyyy HH:ss");
                    }


                }
            }
            else if (value.GetType() == typeof(int?)
                || value.GetType() == typeof(decimal?)
                || value.GetType() == typeof(double?)
                || value.GetType() == typeof(long?)
                || value.GetType() == typeof(int)
                || value.GetType() == typeof(decimal)
                || value.GetType() == typeof(double)
                || value.GetType() == typeof(long)
                )
            {
                result = string.Format("{0:#,##0.####}", value);
            }
            else
            {
                result = value.ToString();
            }
            return result;
        }
        public static string CalculateSHA256BitHash(string input)
        {

            // step 1, calculate MD5 hash from input



            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(inputBytes);



            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }



        /// <summary>
        /// Định dạng dấu phân cách tiền tệ
        /// </summary>
        /// <param name="number">Số cần định dạng</param>
        /// <param name="groupingSymbol">Ký hiệu phân cách</param>
        /// <param name="decimalSymbol">Ký hiệu thập phân</param>
        /// <param name="decimalPlaces">Số chữ số thập phân</param>
        /// <returns>Chuỗi đã định dạng</returns>
        public static string Format(double? number, string groupingSymbol, string decimalSymbol, int decimalPlaces)
        {
            if (number.HasValue)
            {
                if (number == 0) return "0";
                var numberFormat = new CultureInfo("vi-VN", false).NumberFormat;
                //KHÔNG LẤY GIÁ TRỊ THẬP PHÂN ĐỐI VỚI TIỀN

                //number = decimalPlaces == 0 ? Math.Floor((double)number) : number;
                {
                    numberFormat.NumberDecimalDigits = decimalPlaces;
                    numberFormat.NumberGroupSeparator = groupingSymbol;
                    numberFormat.NumberDecimalSeparator = decimalSymbol;
                }
                return ((double)(number)).ToString("N", numberFormat);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Định dạng dấu phân cách tiền tệ và có làm tròn
        /// </summary>
        /// <param name="number">Số cần định dạng</param>
        /// <param name="groupingSymbol">Ký hiệu phân cách</param>
        /// <param name="decimalSymbol">Ký hiệu thập phân</param>
        /// <param name="decimalPlaces">Số chữ số thập phân</param>
        /// <returns>Chuỗi đã định dạng</returns>
        public static string Format(this double? number, string groupingSymbol, string decimalSymbol, int decimalPlaces, bool isRound)
        {
            var CurrencyFormat = 3;
            if (number == 0) return "0";
            var numberFormat = new CultureInfo("vi-VN", false).NumberFormat;
            var value = Math.Pow(10, CurrencyFormat);
            if (number != null)
            {
                if (isRound)
                {
                    //LÀM TRÒN THEO CẤU HÌNH HỆ THỐNG
                    number /= value;
                    number = Math.Round((double)number, 0) * value;
                }

                numberFormat.NumberGroupSeparator = groupingSymbol;
                numberFormat.NumberDecimalSeparator = decimalSymbol;
                numberFormat.NumberDecimalDigits = decimalPlaces;
            }
            return number != null ? ((double)(number)).ToString("N", numberFormat) : string.Empty;
        }

        public static string Truncate(this string str, int maxLength)
        {
            if (str == null || str.Length < maxLength || str.IndexOf(" ", maxLength) == -1)
                return str;

            return str.Substring(0, str.IndexOf(" ", maxLength)) + "...";
        }

        public static string ReadNumber(string value)
        {
            //CHUYỂN 1 SỐ ST THÀNH CHUỖI
            value = value.Replace(".", "").Replace(",", "");
            var result = "";

            var temp = 0L;

            if (!long.TryParse(value, out temp))
            {
                return "";
            }
            if (long.Parse(value) == 0L)
                return "không ";
            var i = value.Length - 1;
            while (i >= 2)
            {
                var t = value.Length % 3;
                string str = null;
                if (t != 0)
                {
                    str = value.Substring(0, t);
                    value = value.Remove(0, t);
                }
                else
                {
                    str = value.Substring(0, 3);
                    value = value.Remove(0, 3);
                }
                i = value.Length - 1;
                result = result + Convert3Number(str);
                if (string.IsNullOrEmpty(value))
                {
                    break;
                }
                if (value.Length == 3 | value.Length == 12 | value.Length == 21 | value.Length == 30 | value.Length == 39)
                {
                    if (str != "000")
                    {
                        result = result + "ngàn ";
                    }

                }
                else if (value.Length == 6 | value.Length == 15 | value.Length == 24 | value.Length == 33 | value.Length == 42)
                {

                    if (str != "000")
                    {
                        result = result + "triệu ";
                    }


                }
                else if (value.Length == 9 | value.Length == 18 | value.Length == 27 | value.Length == 36 | value.Length == 45)
                {


                    result = result + "tỷ ";


                }
            }
            result = result + Convert3Number(value);
            string[] chuoiMois = result.Trim().Split(' ');
            string chuoiMoi = "";
            try
            {
                for (int j = 0; j < chuoiMois[0].Length; j++)
                {
                    if (j == 0)
                        chuoiMoi += chuoiMois[0][j].ToString().ToUpper();
                    else
                        chuoiMoi += chuoiMois[0][j];
                }
                for (int k = 1; k < chuoiMois.Length; k++)
                {
                    chuoiMoi += ' ' + chuoiMois[k];
                }
                return chuoiMoi;
            }
            catch { return result.Trim(); }
        }

        private static string Convert3Number(string str)
        {
            //chuyển nhóm 3 số thành chữ
            var tmp = str;
            var result = "";
            while (str.Length > 0)
            {
                var value = str.Substring(0, 1);
                switch (str.Length)
                {
                    case 3:
                        //var temp = 0;
                        if (int.Parse(str) != 0)
                        {
                            if (value != "0")
                            {
                                result = ConvertNumber(value) + "trăm ";
                            }
                            else
                            {
                                result = "không trăm ";
                            }
                        }
                        break;
                    case 2:
                        if (value != "0")
                        {
                            if (string.IsNullOrEmpty(result))
                            {
                                if (value == "1")
                                {
                                    result = "mười ";
                                }
                                else
                                {
                                    result = ConvertNumber(value) + "mươi ";
                                }
                            }
                            else if (value == "1")
                            {
                                result = result + "mười ";
                            }
                            else
                            {
                                result = result + ConvertNumber(value) + "mươi ";
                            }
                        }
                        break;
                    case 1:
                        switch (value)
                        {
                            case "5":
                                if (string.IsNullOrEmpty(result))
                                {
                                    //005 or 05
                                    if (tmp.Length == 3 | tmp.Length == 2)
                                    {
                                        result = "lẻ năm ";
                                        //5
                                    }
                                    else
                                    {
                                        result = ConvertNumber(value);
                                    }
                                    //125 or 25 or 105
                                }
                                else
                                {
                                    if (tmp.Substring(1, 1) == "0")
                                    {
                                        result = result + "lẻ năm ";
                                    }
                                    else
                                    {
                                        result = result + "lăm ";
                                    }
                                }
                                break;
                            case "1":
                                if (string.IsNullOrEmpty(result))
                                {
                                    //001 or 01
                                    if (tmp.Length == 3 | tmp.Length == 2)
                                    {
                                        result = "lẻ một ";
                                        //5
                                    }
                                    else
                                    {
                                        result = "một ";
                                    }
                                    //121 or 21 or 101
                                }
                                else
                                {
                                    //101  121
                                    switch (tmp.Length)
                                    {
                                        case 3:
                                            if (tmp.Substring(1, 1) == "0")
                                            {
                                                result = result + "lẻ một ";
                                            }
                                            else if (tmp.Substring(1, 1) == "1")
                                            {
                                                result = result + "một ";
                                            }
                                            else
                                            {
                                                result = result + "mốt ";
                                            }
                                            break;
                                        case 2:
                                            if (tmp.Substring(0, 1) == "0")
                                            {
                                                result = result + "lẻ một ";
                                            }
                                            else if (tmp.Substring(0, 1) == "1")
                                            {
                                                result = result + "một ";
                                            }
                                            else
                                            {
                                                result = result + "mốt ";
                                            }
                                            break;
                                    }
                                }
                                break;
                            default:
                                if (value != "0")
                                {
                                    //002
                                    if (string.IsNullOrEmpty(result))
                                    {
                                        //002 or 02
                                        if (tmp.Length == 3 | tmp.Length == 2)
                                        {
                                            result = "lẻ " + ConvertNumber(value);
                                            //2
                                        }
                                        else
                                        {
                                            result = ConvertNumber(value);
                                        }

                                        //123 or 23 or 103
                                    }
                                    else
                                    {
                                        if (tmp.Substring(1, 1) == "0")
                                        {
                                            result = result + "lẻ " + ConvertNumber(value);
                                        }
                                        else
                                        {
                                            result = result + ConvertNumber(value);
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                }
                str = str.Remove(0, 1);
            }
            return result;
        }

        private static string ConvertNumber(string str)
        {
            switch (str)
            {
                case "0":
                    return "không ";
                case "1":
                    return "một ";
                case "2":
                    return "hai ";
                case "3":
                    return "ba ";
                case "4":
                    return "bốn ";
                case "5":
                    return "năm ";
                case "6":
                    return "sáu ";
                case "7":
                    return "bảy ";
                case "8":
                    return "tám ";
                case "9":
                    return "chín ";
            }
            return "";
        }


        ///Phong.tran - 14/10/2013-- Doi Thanh Tieng Anh

        #region Doc So Bang Tieng Anh - BY Phong.tran 14/10/2013
        public static String changeNumericToWords(double numb)
        {
            String num = numb.ToString();
            return changeToWords(num, false);
        }
        public static String changeCurrencyToWords(String numb)
        {

            return changeToWords(numb, true);
        }
        public static String changeNumericToWordsNew(String numb, string groupingSymbol)
        {
            var number = numb.Split(groupingSymbol.ToCharArray()[0]); string strt = "";
            int stt = 0;
            foreach (var item in number)
            {
                stt++;
                strt += changeToWords(item, false);
                if (number.Length > 4)
                {
                    strt += "";
                }
                else
                {
                    if (number.Length == 4)
                    {
                        switch (stt)
                        {
                            case 1:
                                strt += "billion ";
                                break;
                            case 2:
                                strt += "million ";
                                break;
                            case 3:
                                strt += "thousand ";
                                break;
                            default:
                                strt += "";
                                break;
                        }
                    }
                    else if (number.Length == 3)
                    {
                        switch (stt)
                        {
                            case 1:
                                strt += "million ";
                                break;
                            case 2:
                                strt += "thousand ";
                                break;
                            default:
                                strt += "";
                                break;
                        }
                    }
                    else if (number.Length == 2)
                    {
                        switch (stt)
                        {
                            case 1:
                                strt += "thousand ";
                                break;
                            default:
                                strt += "";
                                break;
                        }
                    }

                }
            }


            return strt;
        }

        public static String changeCurrencyToWords(double numb)
        {
            return changeToWords(numb.ToString(), true);
        }
        private static String changeToWords(String numb, bool isCurrency)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = (isCurrency) ? ("only") : ("");
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (points != null)
                    {
                        andStr = (isCurrency) ? ("and") : ("point");// just to separate whole numbers from points/cents
                        endStr = (isCurrency) ? ("Cents " + endStr) : ("");
                        pointStr = translateCents(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch {; }
            return val;
        }
        private static String translateWholeNumber(String number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = number.StartsWith("0");
                    int numDigits = number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " hundred and ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " thousand ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " million ";
                            break;
                        case 10://Billions's range
                            pos = (numDigits % 10) + 1;
                            place = " billion ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));
                        //check for trailing zeros
                        if (beginsZero) word = "" + word.Trim().Replace("hundred", "");
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }

            }
            catch {; }
            return word.Trim();
        }
        private static String tens(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = null;
            switch (digt)
            {
                case 10:
                    name = "ten";
                    break;
                case 11:
                    name = "eleven";
                    break;
                case 12:
                    name = "twelve";
                    break;
                case 13:
                    name = "thirteen";
                    break;
                case 14:
                    name = "fourteen";
                    break;
                case 15:
                    name = "fifteen";
                    break;
                case 16:
                    name = "sixteen";
                    break;
                case 17:
                    name = "seventeen";
                    break;
                case 18:
                    name = "eighteen";
                    break;
                case 19:
                    name = "nineteen";
                    break;
                case 20:
                    name = "twenty -";
                    break;
                case 30:
                    name = "thirty -";
                    break;
                case 40:
                    name = "forty -";
                    break;
                case 50:
                    name = "fifty -";
                    break;
                case 60:
                    name = "sixty -";
                    break;
                case 70:
                    name = "seventy -";
                    break;
                case 80:
                    name = "eighty -";
                    break;
                case 90:
                    name = "ninety -";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ones(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";
            switch (digt)
            {
                case 1:
                    name = "one";
                    break;
                case 2:
                    name = "two";
                    break;
                case 3:
                    name = "three";
                    break;
                case 4:
                    name = "four";
                    break;
                case 5:
                    name = "five";
                    break;
                case 6:
                    name = "six";
                    break;
                case 7:
                    name = "seven";
                    break;
                case 8:
                    name = "eight";
                    break;
                case 9:
                    name = "nine";
                    break;
            }
            return name;
        }
        private static String translateCents(String cents)
        {
            String cts = "", digit = "", engOne = "";
            for (int i = 0; i < cents.Length; i++)
            {
                digit = cents[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cts += " " + engOne;
            }
            return cts;
        }

        #endregion
        /// <summary>
        /// son.le - 2011.07.01: kiểm tra chuổi có phải là số không
        /// </summary>
        /// <param name="value">Chuổi cần kiểm tra</param>
        public static bool IsNumeric(this string value)
        {
            double outNum;
            return double.TryParse(value, out outNum);
        }

        /// <summary>
        /// son.le - 2011.07.01: chuyển chuổi về kiểu Int
        /// </summary>
        /// <param name="value">Chuổi cần chuyển</param>
        /// <returns>Nếu không thành công trả về null</returns>
        public static int? ToInt32(this string value)
        {
            int outInt;
            if (int.TryParse(value, out outInt))
                return outInt;
            else
                return null;
        }

        /// <summary>
        /// Loại bỏ các ký tự đặc biệt trong mã
        /// </summary>
        public static string Remove(this string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.ToUpper().Trim();
                str = str.Replace('/', '-');

                //THAY THẾ CÁC MÃ CHỈ CHỨA CÁC KÝ TỰ TỪ A ĐẾN Z, 0 - 9
                const string pattern = "[^a-zA-Z_0-9]";

                //CÁC KÝ TỰ ĐẶC BIỆT CÒN LẠI ĐƯỢC THAY THẾ BẰNG KÝ TỰ "-"
                str = Regex.Replace(str, pattern, "-");

                return str;
            }
            else
            {
                return String.Empty;
            }
        }
        public static int GetSoHopDongMax(string soHopDong)
        {
            string[] arraySoHopDong = soHopDong.Split('-');
            string soHopDong1 = arraySoHopDong[0].Substring(arraySoHopDong[0].Length - 4);
            string nam = arraySoHopDong[1].Substring(0, 4);
            int namInt = Convert.ToInt32(nam);
            if (namInt < DateTime.Now.Year)
            {
                return 1;
            }
            return Convert.ToInt32(soHopDong1) + 1;

        }
        public static DateTime AddBusinessDays(DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("days cannot be negative", "days");
            }

            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);

        }
        public static string CheckLetter(string preString, string maxValue, int length)
        {
            string yearCurrent = DateTime.Now.Year.ToString().Substring(2, 2);
            string monthCurrent = DateTime.Now.Month.ToString(); // "4"
            //khi thang hien tai nho hon 9 thi cong them "0" vao
            if (Convert.ToInt32(monthCurrent) <= 9)
            {
                monthCurrent = "0" + monthCurrent;
            }
            //Khi tham so select o database la null khoi tao so dau tien
            if (String.IsNullOrEmpty(maxValue))
            {
                string ret = "1";
                while (ret.Length < length)
                {
                    ret = "0" + ret;
                }
                return preString + yearCurrent + monthCurrent + "-" + ret;
            }
            else
            {
                string preStringMax = string.Empty;
                string maxNumber = string.Empty;
                string monthYear = string.Empty;
                string monthDb = string.Empty;

                int pos = maxValue.IndexOf("-");

                if (pos > 4)
                {
                    preStringMax = maxValue.Substring(0, maxValue.IndexOf("-") - 4);
                    maxNumber = maxValue.Substring(maxValue.IndexOf("-") + 1);
                    monthYear = maxValue.Substring(maxValue.IndexOf("-") - 4, 4);
                    monthDb = monthYear.Substring(2, 2); //as "04"

                    string stringTemp = maxNumber;
                    //Khi thang trong gia tri max bang voi thang create thi cong len cho 1
                    if (monthDb == monthCurrent)
                    {
                        int strToInt = Convert.ToInt32(maxNumber);
                        maxNumber = Convert.ToString(strToInt + 1);
                        while (maxNumber.Length < stringTemp.Length)
                            maxNumber = "0" + maxNumber;
                    }
                    else //reset
                    {
                        maxNumber = "1";
                        while (maxNumber.Length < stringTemp.Length)
                        {
                            maxNumber = "0" + maxNumber;
                        }
                    }
                }

                return preString + yearCurrent + monthCurrent + "-" + maxNumber;
            }
        }
        public static string ArchiveStringToNewString(int length, string stringArchive)
        {
            string ret = String.Empty;

            if (String.IsNullOrEmpty(stringArchive))
                return ret;

            if (stringArchive.Length > length)
            {
                ret = stringArchive.Substring(0, length) + " <a href=\"#\" rel=\"tooltip\" title=\"" + stringArchive + "\"><strong>...</strong></a>";
            }
            else { ret = stringArchive; }

            return ret;
        }

    }
}
