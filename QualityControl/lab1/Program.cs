using System;
using System.Collections.Generic;

namespace lab1
{
    enum TriangleType
    {
        IsoscelesTriangle = 0,
        EquilateralTriangle = 1,
        SimpleTriangle = 2,
        NotTriangle = 3
    }

    class Program
    {
        static int Main( string[] args )
        {
            if ( args.Length != 3 )
            {
                Console.WriteLine( "Incorrect data" );
                return 1;
            }

            List<double> triangleValues = GetDoubleList( args );
            if ( triangleValues == null )
            {
                Console.WriteLine( "Incorrect data" );
                return 1;
            }

            var triangleType = GetTriangleType( triangleValues[ 0 ], triangleValues[ 1 ], triangleValues[ 2 ] );
            Console.WriteLine( triangleType.ToString() );
            return 0;
        }

        private static TriangleType GetTriangleType( double a, double b, double c )
        {
            if ( ( ( a + b ).CompareTo( c ) <= 0 ) ||
                 ( ( a + c ).CompareTo( b ) <= 0 ) ||
                 ( ( b + c ).CompareTo( a ) <= 0 ) )
            {
                return TriangleType.NotTriangle;
            }

            if ( a.CompareTo(b) == 0 && b.CompareTo( c ) == 0 )
            {
                return TriangleType.EquilateralTriangle;
            }

            if ( a.CompareTo( b ) == 0 && b.CompareTo( c ) != 0 ||
                 a.CompareTo( c ) == 0 && c.CompareTo( b ) != 0 ||
                 b.CompareTo( c ) == 0 && c.CompareTo( a ) != 0 )
            {
                return TriangleType.IsoscelesTriangle;
            }

            return TriangleType.SimpleTriangle;
        }

        private static List<double> GetDoubleList( string[] args )
        {
            List<double> doubleList = new List<double>();
            foreach ( var arg in args )
            {
                if ( !double.TryParse( arg, out double value ) )
                {
                    return null;
                }
                doubleList.Add( value );
            }
            return doubleList;
        }
    }
}
