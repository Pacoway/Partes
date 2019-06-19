using OSCPartes.Models;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace OSCPartes.Validation
{
    public static class DataValidation
    {
        public static bool ValidateEmail(string cadena)
        {
            return cadena != null && Regex.IsMatch(cadena, @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$");
        }
        public static bool ValidateTLF(string cadena)
        {
            return cadena != null &&
                    (
                     Regex.IsMatch(cadena, @"^\+\d{1,3}\ \d{3}\ \d{2}\ \d{2}\ \d{2}$") ||
                     Regex.IsMatch(cadena, @"^\+\d{1,3}\ \d{3}\ \d{3}\ \d{3}$") ||
                     Regex.IsMatch(cadena, @"^\d{3}\ \d{2}\ \d{2}\ \d{2}$") ||
                     Regex.IsMatch(cadena, @"^\d{3}\ \d{3}\ \d{3}$") ||
                     Regex.IsMatch(cadena, @"^\d{9}$") ||
                     Regex.IsMatch(cadena, @"^\+\d{1,3}\ \d{9}$")
                     );
        }
        public static string FormatTLF(string cadena)
        {
            if (cadena != null && cadena!="")
            {
                if (Regex.IsMatch(cadena, @"^\+\d{1,3}\ \d{3}\ \d{2}\ \d{2}\ \d{2}$")) return cadena;
                if (Regex.IsMatch(cadena, @"^\+\d{1,3}\ \d{3}\ \d{3}\ \d{3}$")) return Regex.Replace(cadena, @"^(\+\d{1,3})\ (\d{3})\ (\d{2})(\d{1})\ (\d{1})(\d{2})$", @"$1 $2 $3 $4$5 $6");
                if (Regex.IsMatch(cadena, @"^\d{3}\ \d{2}\ \d{2}\ \d{2}$")) return cadena;
                if (Regex.IsMatch(cadena, @"^\d{3}\ \d{3}\ \d{3}$")) return Regex.Replace(cadena, @"^(\d{3})\ (\d{2})(\d{1})\ (\d{1})(\d{2})$", @"$1 $2 $3$4 $5");
                if (Regex.IsMatch(cadena, @"^\d{9}$")) return Regex.Replace(cadena, @"^(\d{3})(\d{2})(\d{2})(\d{2})$", @"$1 $2 $3 $4");
                if (Regex.IsMatch(cadena, @"^\+\d{1,3}\ \d{9}$")) return Regex.Replace(cadena, @"^(\+\d{1,3})\ (\d{3})(\d{2})(\d{2})(\d{2})$", @"$1 $2 $3 $4 $5");
            }
            return cadena;
        }
        public static bool ValidateCP(string cadena)
        {
            return cadena != null && Regex.IsMatch(cadena, @"^\d{4,5}$");
        }
        public static bool ValidateCIF_NIF_DNI_NIE(string cadena)
        {
            return cadena != null && (
                                        (Regex.IsMatch(cadena, @"^[ABCDEFGHJNPQRSUVW]\d{7}(?:[A-J]|[0-9])$") && DataValidation.ValidateCIF(cadena)) ||
                                        (Regex.IsMatch(cadena, @"^[KLM]\d{7}(?:[A-Z][0-9])$") && DataValidation.ValidateNIFAntiguo(cadena)) ||
                                        (Regex.IsMatch(cadena, @"^[XYZ]\d{7}[TRWAGMYFPDXBNJZSQVHLCKE]$") && DataValidation.ValidateNIE(cadena)) ||
                                        (Regex.IsMatch(cadena, @"^\d{8}[TRWAGMYFPDXBNJZSQVHLCKE]$") && DataValidation.ValidateDNI(cadena))
                                     );
        }
        public static bool ValidateCIF(string cadena)
        {
            if (cadena == null || cadena.Length != 9) return false;

            if (cadena[0] == 'N' || cadena[0] == 'P' || cadena[0] == 'Q' ||
                cadena[0] == 'R' || cadena[0] == 'S' || cadena[0] == 'W')
            {
                if (Char.IsDigit(cadena, 8)) return false;
            }
            else if (cadena[0] == 'A' || cadena[0] == 'B' || cadena[0] == 'C' ||
                     cadena[0] == 'D' || cadena[0] == 'E' || cadena[0] == 'F' ||
                     cadena[0] == 'G' || cadena[0] == 'H' || cadena[0] == 'J' ||
                     cadena[0] == 'U' || cadena[0] == 'V')
            {
                if (!Char.IsDigit(cadena, 8)) return false;
            }
            else
            {
                return false;
            }

            int sumaA = (int)Char.GetNumericValue(cadena[2]) + (int)Char.GetNumericValue(cadena[4]) + (int)Char.GetNumericValue(cadena[6]);
            string impar1 = ((int)Char.GetNumericValue(cadena[1]) * 2).ToString("00");
            string impar2 = ((int)Char.GetNumericValue(cadena[3]) * 2).ToString("00");
            string impar3 = ((int)Char.GetNumericValue(cadena[5]) * 2).ToString("00");
            string impar4 = ((int)Char.GetNumericValue(cadena[7]) * 2).ToString("00");
            int sumaB = (int)Char.GetNumericValue(impar1[0]) + (int)Char.GetNumericValue(impar1[1]) +
                        (int)Char.GetNumericValue(impar2[0]) + (int)Char.GetNumericValue(impar2[1]) +
                        (int)Char.GetNumericValue(impar3[0]) + (int)Char.GetNumericValue(impar3[1]) +
                        (int)Char.GetNumericValue(impar4[0]) + (int)Char.GetNumericValue(impar4[1]);
            int sumaC = sumaA + sumaB;
            int control = 10 - (sumaC % 10);
            if (control == 10) control = 0;

            if (cadena[0] == 'N' || cadena[0] == 'P' || cadena[0] == 'Q' ||
                cadena[0] == 'R' || cadena[0] == 'S' || cadena[0] == 'W')
            {
                switch (control)
                {
                    case 1: return cadena[8] == 'A';
                    case 2: return cadena[8] == 'B';
                    case 3: return cadena[8] == 'C';
                    case 4: return cadena[8] == 'D';
                    case 5: return cadena[8] == 'E';
                    case 6: return cadena[8] == 'F';
                    case 7: return cadena[8] == 'G';
                    case 8: return cadena[8] == 'H';
                    case 9: return cadena[8] == 'I';
                    case 0: return cadena[8] == 'J';
                    default: return false;
                }
            }
            else if (cadena[0] == 'A' || cadena[0] == 'B' || cadena[0] == 'C' ||
                     cadena[0] == 'D' || cadena[0] == 'E' || cadena[0] == 'F' ||
                     cadena[0] == 'G' || cadena[0] == 'H' || cadena[0] == 'J' ||
                     cadena[0] == 'U' || cadena[0] == 'V')
            {
                return (int)Char.GetNumericValue(cadena[8]) == control;
            }
            else
            {
                return false;
            }
        }
        public static bool ValidateNIFAntiguo(string cadena)
        {
            if (cadena == null || cadena.Length != 9) return false;

            if (cadena[0] != 'K' && cadena[0] != 'L' && cadena[0] != 'M' ) return false;

            int sumaA = (int)Char.GetNumericValue(cadena[2]) + (int)Char.GetNumericValue(cadena[4]) + (int)Char.GetNumericValue(cadena[6]);
            string impar1 = ((int)Char.GetNumericValue(cadena[1]) * 2).ToString("00");
            string impar2 = ((int)Char.GetNumericValue(cadena[3]) * 2).ToString("00");
            string impar3 = ((int)Char.GetNumericValue(cadena[5]) * 2).ToString("00");
            string impar4 = ((int)Char.GetNumericValue(cadena[7]) * 2).ToString("00");
            int sumaB = (int)Char.GetNumericValue(impar1[0]) + (int)Char.GetNumericValue(impar1[1]) +
                        (int)Char.GetNumericValue(impar2[0]) + (int)Char.GetNumericValue(impar2[1]) +
                        (int)Char.GetNumericValue(impar3[0]) + (int)Char.GetNumericValue(impar3[1]) +
                        (int)Char.GetNumericValue(impar4[0]) + (int)Char.GetNumericValue(impar4[1]);
            int sumaC = sumaA + sumaB;
            int control = 10 - (sumaC % 10);
			if (control == 10) control = 0;

            if (!Char.IsDigit(cadena[8]))
            {
                switch (control)
                {
                    case 1: return cadena[8] == 'A';
                    case 2: return cadena[8] == 'B';
                    case 3: return cadena[8] == 'C';
                    case 4: return cadena[8] == 'D';
                    case 5: return cadena[8] == 'E';
                    case 6: return cadena[8] == 'F';
                    case 7: return cadena[8] == 'G';
                    case 8: return cadena[8] == 'H';
                    case 9: return cadena[8] == 'I';
                    case 0: return cadena[8] == 'J';
                    default: return false;
                }
            }
            else
            {
                return (int)Char.GetNumericValue(cadena[8]) == control;
            }
        }
        public static bool ValidateDNI(string cadena)
        {
            if (cadena == null || cadena.Length != 9) return false;

            string cadena_numero = cadena.Substring(0, 8);
            ulong numero = Convert.ToUInt64(cadena_numero) % 23;

            switch (numero)
            {
                case 0: return cadena[8] == 'T';
                case 1: return cadena[8] == 'R';
                case 2: return cadena[8] == 'W';
                case 3: return cadena[8] == 'A';
                case 4: return cadena[8] == 'G';
                case 5: return cadena[8] == 'M';
                case 6: return cadena[8] == 'Y';
                case 7: return cadena[8] == 'F';
                case 8: return cadena[8] == 'P';
                case 9: return cadena[8] == 'D';
                case 10: return cadena[8] == 'X';
                case 11: return cadena[8] == 'B';
                case 12: return cadena[8] == 'N';
                case 13: return cadena[8] == 'J';
                case 14: return cadena[8] == 'Z';
                case 15: return cadena[8] == 'S';
                case 16: return cadena[8] == 'Q';
                case 17: return cadena[8] == 'V';
                case 18: return cadena[8] == 'H';
                case 19: return cadena[8] == 'L';
                case 20: return cadena[8] == 'C';
                case 21: return cadena[8] == 'K';
                case 22: return cadena[8] == 'E';
                default: return false;
            }

        }
        public static bool ValidateNIE(string cadena)
        {
            if (cadena == null || cadena.Length != 9) return false;

            string cadena_numero = cadena.Substring(1, 7);

            switch (cadena[0])
            {
                case 'X': cadena_numero = '0' + cadena_numero; break;
                case 'Y': cadena_numero = '1' + cadena_numero; break;
                case 'Z': cadena_numero = '2' + cadena_numero; break;
                default: return false;
            }


            ulong numero = Convert.ToUInt64(cadena_numero) % 23;

            switch (numero)
            {
                case 0: return cadena[8] == 'T';
                case 1: return cadena[8] == 'R';
                case 2: return cadena[8] == 'W';
                case 3: return cadena[8] == 'A';
                case 4: return cadena[8] == 'G';
                case 5: return cadena[8] == 'M';
                case 6: return cadena[8] == 'Y';
                case 7: return cadena[8] == 'F';
                case 8: return cadena[8] == 'P';
                case 9: return cadena[8] == 'D';
                case 10: return cadena[8] == 'X';
                case 11: return cadena[8] == 'B';
                case 12: return cadena[8] == 'N';
                case 13: return cadena[8] == 'J';
                case 14: return cadena[8] == 'Z';
                case 15: return cadena[8] == 'S';
                case 16: return cadena[8] == 'Q';
                case 17: return cadena[8] == 'V';
                case 18: return cadena[8] == 'H';
                case 19: return cadena[8] == 'L';
                case 20: return cadena[8] == 'C';
                case 21: return cadena[8] == 'K';
                case 22: return cadena[8] == 'E';
                default: return false;
            }
        }

        public static bool ValidateMatricula(string cadena)
        {
            return cadena != null && Regex.IsMatch(cadena, @"\d{4}[A-Z]{3}");
        }
        public static bool ValidateHora(string cadena)
        {
            return cadena != null && Regex.IsMatch(cadena, @"\d{2}:\d{2}");
        }

    }

}
