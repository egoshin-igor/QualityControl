using System;
using System.Collections.Generic;
using System.Text;

namespace lab5
{
    public class LongNumber
    {
        // число в представлении массива байт
        private List<byte> _discharges = new List<byte>();
        //флаг + -
        private bool _isNegative = false;
        // различные конструкторы
        public LongNumber()
        {
        }

        public LongNumber( LongNumber ln )
        {
            _isNegative = ln._isNegative;
            _discharges = ln._discharges;
        }

        private LongNumber( IEnumerable<byte> bytes )
        {
            _discharges.AddRange( bytes );
        }

        private LongNumber( IEnumerable<byte> bytes, bool isNegative )
        {
            _discharges.AddRange( bytes );
            _isNegative = isNegative;
        }

        // широкую на широкую! умножаем
        public static LongNumber operator *( LongNumber first, LongNumber second )
        {

            var output = new LongNumber();
            if ( AbsCompare( first, second ) == -1 )
            {
                var temp = second;
                second = first;
                first = temp;
            }

            for ( int i = 0; i < second._discharges.Count; i++ )
                multuADD( output, first * second._discharges[ i ], i );
            var lastIndex = output._discharges.Count;
            while ( lastIndex != 0 && output._discharges[ --lastIndex ] == 0 ) ;
            output._discharges.RemoveRange( lastIndex + 1, output._discharges.Count - lastIndex - 1 );
            output._isNegative = first._isNegative != second._isNegative;
            return output;
        }

        // умножение длинного на байт
        public static LongNumber operator *( LongNumber ln, byte b )
        {
            var output = new LongNumber( ln._discharges );
            byte carry = 0;
            for ( int i = 0; i < output._discharges.Count; i++ )
            {
                int intermediate = output._discharges[ i ] * b;
                var addCarry = ( ( intermediate & 255 ) + carry ) >> 8;
                output._discharges[ i ] = ( byte )( ( intermediate & 255 ) + carry );
                carry = ( byte )( ( intermediate >> 8 ) + addCarry );
            }
            if ( carry > 0 ) output._discharges.Add( carry );


            return output;
        }

        // деление (частное, остаток)
        public static Cont<LongNumber, LongNumber> operator /( LongNumber first, LongNumber second )
        {

            var output = new LongNumber();
            var residue = new LongNumber( first._discharges, first._isNegative );

            if ( AbsCompare( first, second ) == -1 )
            {
                output._discharges.Add( 0 );
                return new Cont<LongNumber, LongNumber>( output, residue );
            }
            var discharge = residue._discharges.Count - second._discharges.Count;

            for ( int i = 0; i < discharge; i++ ) second._discharges.Insert( 0, 0 );

            for ( int i = 0; i < discharge; i++ )
            {
                if ( AbsCompare( residue, second ) == -1 )
                    output._discharges.Insert( 0, 0 );
                else
                {
                    var cont = getDivider( residue, second );
                    output._discharges.Insert( 0, cont.First );
                    residue = cont.Second;

                }
                second._discharges.RemoveAt( 0 );
            }

            {
                var cont = getDivider( residue, second );
                output._discharges.Insert( 0, cont.First );
                residue = cont.Second;
            }

            var lastIndex = output._discharges.Count;
            while ( output._discharges[ --lastIndex ] == 0 ) ;
            output._discharges.RemoveRange( lastIndex + 1, output._discharges.Count - lastIndex - 1 );
            output._isNegative = first._isNegative != second._isNegative;
            return new Cont<LongNumber, LongNumber>( output, residue );
        }
        // подбор очередного байта частного методом дихотомии. (гуглить "окулов длинная арифметика")
        private static Cont<Byte, LongNumber> getDivider( LongNumber residue, LongNumber second )
        {
            var down = 0;
            var up = 256;
            while ( down + 1 < up )
            {
                var newValue = second * ( byte )( ( down + up ) / 2 );

                if ( AbsCompare( residue, newValue ) == -1 )
                {
                    up = ( down + up ) / 2;
                }
                else
                {
                    down = ( down + up ) / 2;
                }
            }

            var residueOut = Subtract( residue, second * ( byte )down );
            residueOut._isNegative = residue._isNegative;

            return new Cont<Byte, LongNumber>( ( byte )down, residueOut );
        }
        // специальное сложение для умножения. чтоб быстрее
        private static void multuADD( LongNumber output, LongNumber longNumber, int index )
        {

            var isCarry = false;
            for ( int i = 0; i < longNumber._discharges.Count; i++ )
            {
                if ( output._discharges.Count == i + index ) output._discharges.Add( 0 );
                var add = output._discharges[ i + index ] + longNumber._discharges[ i ] + ( isCarry ? 1 : 0 );

                isCarry = ( add >> 8 ) > 0;
                output._discharges[ i + index ] = ( byte )add;

            }
            var carryIndex = longNumber._discharges.Count + index;
        }
        // перегруз оператора. учтено, что иногда сложение - это вычитание (например -5+3 - это 3 - 5)
        public static LongNumber operator +( LongNumber first, LongNumber second )
        {

            if ( AbsCompare( first, second ) == -1 )
            {
                var temp = second;
                second = first;
                first = temp;
            }
            var output = first._isNegative == second._isNegative ? Add( first, second ) : Subtract( first, second );
            output._isNegative = first._isNegative;
            return output;


        }
        // перегруз оператора. учтено, что иногда сложение - это вычитание (например -5+3 - это 3 - 5)
        public static LongNumber operator -( LongNumber first, LongNumber second )
        {
            if ( first == second )
                second = new LongNumber( second._discharges ) { _isNegative = !first._isNegative };
            else
                second._isNegative = !second._isNegative;
            var output = first + second;
            second._isNegative = !second._isNegative;
            return output;
        }
        // сложение
        public static LongNumber Add( LongNumber first, LongNumber second )
        {

            var sum = new LongNumber( first._discharges );
            var isCarry = false;
            for ( int i = 0; i < second._discharges.Count; i++ )
            {
                var add = sum._discharges[ i ] + second._discharges[ i ] + ( isCarry ? 1 : 0 );
                isCarry = ( add >> 8 ) > 0;
                sum._discharges[ i ] = ( byte )add;
            }
            var carryIndex = second._discharges.Count;

            return sum;
        }
        // вычитание
        public static LongNumber Subtract( LongNumber first, LongNumber second )
        {
            var sum = new LongNumber( first._discharges );
            var isCarry = false;
            for ( int i = 0; i < second._discharges.Count; i++ )
            {
                var isCarryNext = sum._discharges[ i ] < second._discharges[ i ];
                sum._discharges[ i ] -= ( byte )( second._discharges[ i ] + ( isCarry ? 1 : 0 ) );
                isCarry = isCarryNext;
            }
            var carryIndex = second._discharges.Count;
            while ( isCarry )
            {
                if ( sum._discharges[ carryIndex ] != 0 ) isCarry = false;
                sum._discharges[ carryIndex ] -= 1;
                carryIndex++;
            }
            var lastIndex = sum._discharges.Count;
            while ( lastIndex != 0 && sum._discharges[ --lastIndex ] == 0 ) ;
            sum._discharges.RemoveRange( lastIndex + 1, sum._discharges.Count - lastIndex - 1 );

            return sum;
        }

        // сранение 1 - первое больше, -1 первое меньше, 0 - равны
        public static int AbsCompare( LongNumber first, LongNumber second )
        {
            if ( first._isNegative && !second._isNegative ) return -1;
            if ( !first._isNegative && second._isNegative ) return 1;
            if ( first._discharges.Count < second._discharges.Count ) return -1;
            if ( first._discharges.Count > second._discharges.Count ) return 1;
            for ( int i = first._discharges.Count - 1; i >= 0; i-- )
            {
                if ( first._discharges[ i ] < second._discharges[ i ] ) return -1;
                if ( first._discharges[ i ] > second._discharges[ i ] ) return 1;
            }
            return 0;
        }

        // перегрузы всяких сравнений
        public static bool operator >( LongNumber first, LongNumber second )
        {
            if ( first._isNegative )
            {
                if ( second._isNegative )
                    return AbsCompare( first, second ) == -1;

                return false;
            }

            if ( second._isNegative )
                return true;

            return AbsCompare( first, second ) == 1;
        }

        public static bool operator <( LongNumber first, LongNumber second )
        {
            return second > first;
        }

        public static bool operator <=( LongNumber first, LongNumber second )
        {
            return ( first == second ) || ( first < second );
        }

        public static bool operator >=( LongNumber first, LongNumber second )
        {
            return ( first == second ) || ( first > second );
        }

        public static bool operator ==( LongNumber first, LongNumber second )
        {
            if ( first._isNegative != second._isNegative ) return false;

            return AbsCompare( first, second ) == 0;
        }

        public static bool operator !=( LongNumber first, LongNumber second )
        {
            return !( first == second );
        }
        // перевод в обычную строку(поразрядно)
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append( Sign() );
            var forOut = new LongNumber( _discharges );
            var ten = new LongNumber( new byte[] { 10 } );
            while ( forOut._discharges.Count > 1 || forOut._discharges[ 0 ] > 9 )
            {
                var div = forOut / ten;
                forOut = div.First;
                sb.Insert( 1, ( char )( '0' + div.Second._discharges[ 0 ] ) );
            }
            sb.Insert( 1, ( char )( '0' + forOut._discharges[ 0 ] ) );
            return sb.ToString();
        }

        private char Sign()
        {
            return _isNegative ? '-' : '+';
        }

        public static LongNumber FromString( string s )
        {
            if (s.Length != 0 && s[0] != '-' && s[ 0 ] != '+' )
            {
                s = '+' + s;
            }
            var output = new LongNumber( new List<byte> { 0 } );
            for ( int i = 1; i < s.Length; i++ )
            {
                output *= 10;
                output += new LongNumber( new List<byte> { ( byte )( s[ i ] - '0' ) } );
            }
            output._isNegative = s[ 0 ] == '-';
            return output;
        }
    }
    // вспомогательный класс контейнер
    public class Cont<T, T1>
    {
        public Cont( T down, T1 residue )
        {
            First = down;
            Second = residue;
        }

        public T1 Second { get; set; }

        public T First { get; set; }

        public override string ToString()
        {
            return "Dividend is " + First + " : Residue is " + Second;
        }
    }
}