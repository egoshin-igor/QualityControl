using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace lab1
{
    class Program
    {
        static int Main( string[] args )
        {
            Console.OutputEncoding = Encoding.UTF8;

            if ( args.Length != 3 )
            {
                Console.WriteLine( "Некорректные данные\nПравильный формат:\nlab1.exe <number> <number> <number>\n<number> > 0" );
                return 1;
            }

            List<double> triangleValues = GetDoubleList( args );
            if ( triangleValues == null )
            {
                Console.WriteLine( "Некорректные данные\nПравильный формат:\nlab1.exe <number> <number> <number>\n<number> > 0" );
                return 1;
            }

            if ( !ValidateFigureLength( triangleValues ) )
            {
                Console.WriteLine( "Некорректные данные\nПравильный формат:\nlab1.exe <number> <number> <number>\n<number> > 0" );
                return 1;
            }

            string triangleType = GetTriangleType( triangleValues[ 0 ], triangleValues[ 1 ], triangleValues[ 2 ] );
            Console.WriteLine( triangleType );
            return 0;
        }

        private static string GetTriangleType( double a, double b, double c )
        {
            if ( ( ( a + b ).CompareTo( c ) <= 0 ) ||
                 ( ( a + c ).CompareTo( b ) <= 0 ) ||
                 ( ( b + c ).CompareTo( a ) <= 0 ) )
            {
                return "не треугольник";
            }

            if ( a.CompareTo( b ) == 0 && b.CompareTo( c ) == 0 )
            {
                return "равносторонний";
            }

            if ( a.CompareTo( b ) == 0 && b.CompareTo( c ) != 0 ||
                 a.CompareTo( c ) == 0 && c.CompareTo( b ) != 0 ||
                 b.CompareTo( c ) == 0 && c.CompareTo( a ) != 0 )
            {
                return "равнобедренный";
            }

            return "обычный";
        }

        private static List<double> GetDoubleList( string[] args )
        {
            List<double> doubleList = new List<double>();
            foreach ( var arg in args )
            {
                if ( !double.TryParse( arg, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo( "ru" ), out double value ) &&
                     !double.TryParse( arg, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo( "en" ), out value ) )
                {
                    return null;
                }
                doubleList.Add( value );
            }

            return doubleList;
        }

        private static bool ValidateFigureLength( List<double> doubleList )
        {
            foreach ( var value in doubleList )
            {
                if ( value.CompareTo( 0 ) <= 0 )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
