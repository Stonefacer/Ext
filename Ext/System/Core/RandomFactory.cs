using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core {
    public static class RandomFactory {

        private static string _allSymbols;
        private static string _alphanumericSymbols;
        private static Random _rnd;

        static RandomFactory() {
            _rnd = new Random((int)DateTime.Now.Ticks);
            StringBuilder sbAll = new StringBuilder();
            StringBuilder sbAlpha = new StringBuilder();
            for(int i = 32; i < 127; i++) {
                char ch = (char)i;
                sbAll.Append(ch);
                if(char.IsLetterOrDigit(ch))
                    sbAlpha.Append(ch);
            }
            _allSymbols = sbAll.ToString();
            _alphanumericSymbols = sbAlpha.ToString();
        }

        public static string GetRandomString(int length, bool alphaNumericOnly = true) {
            string sourceSet = _allSymbols;
            if(alphaNumericOnly)
                sourceSet = _alphanumericSymbols;
            StringBuilder res = new StringBuilder();
            for(int i = 0; i < length; i++) {
                res.Append(sourceSet[_rnd.Next(sourceSet.Length)]);
            }
            return res.ToString();
        }

        public static Random GetRandom() {
            return _rnd;
        }

        public static string RandomSymbolsOrder(string str, int iterations = 0) {
            if(str.Length < 2)
                return str;
            if(iterations == 0)
                iterations = str.Length >> 1 + 1;
            StringBuilder sb = new StringBuilder(str);
            while(iterations-- > 0) {
                for(int j = 0; j < sb.Length; j++) {
                    var buf = sb[j];
                    var id = _rnd.Next(sb.Length);
                    sb[j] = sb[id];
                    sb[id] = buf;
                }
            }
            return sb.ToString();
        }

        private static bool IsAscendingOrder(StringBuilder sb) {
            for(int i = 1; i < sb.Length; i++) {
                if(sb[i - 1] > sb[i])
                    return false;
            }
            return true;
        }

        private static void SortStringBuilder(StringBuilder sb) {
            for(int i = 0; i < sb.Length - 1; i++) {
                for(int j = i + 1; j < sb.Length; j++) {
                    if(sb[j] < sb[i]) {
                        var buf = sb[j];
                        sb[j] = sb[i];
                        sb[i] = buf;
                    }
                }
            }
        }

        public static void GetAllPossibleOrders(string source, Action<StringBuilder> OnNewOrder) {
            StringBuilder sbAsc = new StringBuilder(source);
            SortStringBuilder(sbAsc);
            StringBuilder sbDesc = new StringBuilder(sbAsc.ToString());
            sbDesc.Reverse();
            OnNewOrder(sbAsc);
            OnNewOrder(sbDesc);
            do {
                for(int i = sbAsc.Length - 1; i > 0; i--) {
                    var buf = sbAsc[i];
                    sbAsc[i] = sbAsc[i - 1];
                    sbAsc[i - 1] = buf;
                    OnNewOrder(sbAsc);
                    buf = sbDesc[i];
                    sbDesc[i] = sbDesc[i - 1];
                    sbDesc[i - 1] = buf;
                    OnNewOrder(sbDesc);
                }
            } while(!IsAscendingOrder(sbDesc));
        }

    }
}
