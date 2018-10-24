using System;
using System.Collections.Generic;
using Xunit;

namespace lab5.Tests
{
    public class LongNumberTest
    {
        [Fact]
        public void LongNumber_LongNumberInitializationFromString_CorrectInput_GetCorrectNumber()
        {
            // Arrange
            var stringNumbers = new List<string> { "+100", "-100", "100", "0", "+22200", "-1", "-2", "3", "11111111111", "2222222" };
            var expectedData = new List<string> { "+100", "-100", "+100", "+0", "+22200", "-1", "-2", "+3", "+11111111111", "+2222222" };

            // Act
            List<LongNumber> result = GetLongNumbers( stringNumbers );

            // Assert
            Assert.Equal( expectedData.Count, result.Count );
            for ( int i = 0; i < result.Count; i++ )
            {
                Assert.Equal( expectedData[ i ], result[ i ].ToString() );
            }
        }

        [Fact]
        public void LongNumber_LongNumberInitializationFromAnotherLongNumber_CorrectInput_GetCorrectNumber()
        {
            // Arrange
            var stringNumbers = new List<string> { "+100", "-100", "100", "0", "+22200", "-1", "-2", "3" };
            var expectedData = new List<string> { "+100", "-100", "+100", "+0", "+22200", "-1", "-2", "+3" };
            List<LongNumber> longNumbers = GetLongNumbers( stringNumbers );

            // Act
            var result = new List<LongNumber>();
            foreach ( var longNumber in longNumbers )
            {
                result.Add( new LongNumber( longNumber ) );
            }

            // Assert
            Assert.Equal( expectedData.Count, result.Count );
            for ( int i = 0; i < result.Count; i++ )
            {
                Assert.Equal( expectedData[ i ], result[ i ].ToString() );
            }
        }

        [Theory]
        [InlineData("2", "2", "+4")]
        [InlineData( "3", "-3", "-9" )]
        [InlineData( "20", "20", "+400" )]
        [InlineData( "3", "4", "+12" )]
        [InlineData( "-1", "3", "-3" )]
        [InlineData( "3", "0", "+0" )]
        [InlineData( "4", "111", "+444" )]
        [InlineData( "4", "111", "+444" )]
        [InlineData( "44", "2", "+88" )]
        [InlineData( "11", "3", "+33" )]
        [InlineData( "122", "455", "+55510" )]
        [InlineData( "11111", "-1111", "-12344321" )]
        public void LongNumber_NumberMultipliedByNumber_GetCorrectNumber(string strNumberOne, string strNumberTwo, string strResult)
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var result = numberOne * numberTwo;

            // Assert
            Assert.Equal( strResult, result.ToString() );
        }

        [Theory]
        [InlineData( "2", "2", "Dividend is +1 : Residue is +0" )]
        [InlineData( "3", "-3", "Dividend is -1 : Residue is +0" )]
        [InlineData( "20", "20", "Dividend is +1 : Residue is +0" )]
        [InlineData( "3", "4", "Dividend is +0 : Residue is +3" )]
        [InlineData( "7", "3", "Dividend is +2 : Residue is +1" )]
        [InlineData( "21", "22", "Dividend is +0 : Residue is +21" )]
        [InlineData( "0", "3", "Dividend is +0 : Residue is +0" )]
        [InlineData( "3", "3", "Dividend is +1 : Residue is +0" )]
        [InlineData( "4", "-4", "Dividend is -1 : Residue is +0" )]
        [InlineData( "21", "21", "Dividend is +1 : Residue is +0" )]
        [InlineData( "4", "5", "Dividend is +0 : Residue is +4" )]
        [InlineData( "9", "5", "Dividend is +1 : Residue is +4" )]
        [InlineData( "22", "23", "Dividend is +0 : Residue is +22" )]
        [InlineData( "321", "123", "Dividend is +2 : Residue is +75" )]
        [InlineData( "1111111", "1111111", "Dividend is +1 : Residue is +0" )]
        [InlineData( "100", "1", "Dividend is +100 : Residue is +0" )]
        public void LongNumber_NumberDivededByNumber_GetCorrectNumber( string strNumberOne, string strNumberTwo, string strResult )
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var result = numberOne / numberTwo;

            // Assert
            Assert.Equal( strResult, result.ToString() );
        }

        [Theory]
        [InlineData( "3", "4", "+7" )]
        [InlineData( "1", "3", "+4" )]
        [InlineData( "20", "-4", "+16" )]
        [InlineData( "20", "0", "+20" )]
        [InlineData( "12", "15", "+27" )]
        [InlineData( "33", "44", "+77" )]
        [InlineData( "11", "-5", "+6" )]
        [InlineData( "100", "0", "+100" )]
        [InlineData( "1222", "1111", "+2333" )]
        [InlineData( "0", "100", "+100" )]
        [InlineData( "3333", "-1111", "+2222" )]
        public void LongNumber_NumberAddedToNumber_GetCorrectNumber( string strNumberOne, string strNumberTwo, string strResult )
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var result = numberOne + numberTwo;

            // Assert
            Assert.Equal( strResult, result.ToString() );
        }

        [Theory]
        [InlineData( "5", "4", "+1" )]
        [InlineData( "1", "-3", "+4" )]
        [InlineData( "20", "4", "+16" )]
        [InlineData( "20", "0", "+20" )]
        [InlineData( "20", "20", "+0" )]
        [InlineData( "33", "11", "+22" )]
        [InlineData( "10000", "55", "+9945" )]
        [InlineData( "66", "65", "+1" )]
        public void LongNumber_NumberSubtractAnotherNumber_GetCorrectNumber( string strNumberOne, string strNumberTwo, string strResult )
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var result = numberOne - numberTwo;

            // Assert
            Assert.Equal( strResult, result.ToString() );
        }

        [Theory]
        [InlineData( "3", "4", "-1" )]
        [InlineData( "1", "-3", "1" )]
        [InlineData( "-20", "4", "-1" )]
        [InlineData( "20", "20", "0" )]
        [InlineData( "33", "34", "-1" )]
        [InlineData( "22", "-11", "1" )]
        [InlineData( "-33", "22", "-1" )]
        [InlineData( "1", "1", "0" )]
        public void LongNumber_NumberIsComparedWitnNumber_GetCorrectCompare( string strNumberOne, string strNumberTwo, string strResult )
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var result = LongNumber.AbsCompare(numberOne, numberTwo);

            // Assert
            Assert.Equal( strResult, result.ToString() );
        }

        [Theory]
        [InlineData( "-5", "-4", false )]
        [InlineData( "-5", "4", false )]
        [InlineData( "20", "-4", true )]
        [InlineData( "20", "21", false )]
        [InlineData( "-6", "-5", false )]
        [InlineData( "-1", "1", false )]
        [InlineData( "33", "-22", true )]
        [InlineData( "11", "12", false )]
        public void LongNumber_NumberBiggerThenAnotherNumber_GetCorrectCompare( string strNumberOne, string strNumberTwo, bool expectedResult )
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var resultOne = numberOne > numberTwo;
            var resultTwo = !(numberOne < numberTwo) && numberOne != numberTwo;
            var resultThree = !( numberOne <= numberTwo );
            var resultFour = numberOne >= numberTwo;

            // Assert
            Assert.Equal( expectedResult, resultOne );
            Assert.Equal( expectedResult, resultTwo );
            Assert.Equal( expectedResult, resultThree );
            Assert.Equal( expectedResult, resultFour );
        }

        [Theory]
        [InlineData( "-5", "5", false )]
        [InlineData( "-4", "4", false )]
        [InlineData( "4", "4", true )]
        [InlineData( "111", "111", true )]
        public void LongNumber_NumberNotEqualAnotherNumber_GetCorrectCompare( string strNumberOne, string strNumberTwo, bool expectedResult )
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var resultOne = numberOne == numberTwo;
            var resultTwo = !(numberOne != numberTwo);

            // Assert
            Assert.Equal( expectedResult, resultOne );
            Assert.Equal( expectedResult, resultTwo );
        }

        [Theory]
        [InlineData( "5", "5", true )]
        [InlineData( "7", "7", true )]
        [InlineData( "8", "8", true )]
        public void LongNumber_NumberEqualAnotherNumber_GetCorrectCompare( string strNumberOne, string strNumberTwo, bool expectedResult )
        {
            // Arrange
            var numberOne = LongNumber.FromString( strNumberOne );
            var numberTwo = LongNumber.FromString( strNumberTwo );

            // Act
            var resultOne = numberOne >= numberTwo;
            var resultTwo = numberOne <= numberTwo;
            var resultThree = numberOne == numberTwo;

            // Assert
            Assert.Equal( expectedResult, resultOne );
            Assert.Equal( expectedResult, resultTwo );
            Assert.Equal( expectedResult, resultThree );
        }

        private List<LongNumber> GetLongNumbers( List<string> stringNumbers )
        {
            var longNumbers = new List<LongNumber>();
            foreach ( var stringNumber in stringNumbers )
            {
                longNumbers.Add( LongNumber.FromString( stringNumber ) );
            }

            return longNumbers;
        }
    }
}
