set PROGRAM="../bin/Debug/lab1.exe"

echo not enough arguments
%PROGRAM% > output.txt
IF NOT ERRORLEVEL 1 GOTO err
FC output.txt incorrectData.txt
IF ERRORLEVEL 1 GOTO err

echo so many arguments
%PROGRAM% 1 2 3 4 > output.txt
IF NOT ERRORLEVEL 1 GOTO err
FC output.txt incorrectData.txt
IF ERRORLEVEL 1 GOTO err

echo Arguments are not numbers
%PROGRAM% notANumber a b > output.txt
IF NOT ERRORLEVEL 1 GOTO err
FC output.txt incorrectData.txt
IF ERRORLEVEL 1 GOTO err

echo not triangle
%PROGRAM% -1 0 1 > output.txt
IF NOT ERRORLEVEL 0 GOTO err
FC output.txt notTriangle.txt
IF ERRORLEVEL 1 GOTO err

echo not triangle
%PROGRAM% 3 2 1 > output.txt
IF NOT ERRORLEVEL 0 GOTO err
FC output.txt notTriangle.txt
IF ERRORLEVEL 1 GOTO err

echo isosceles triangle
%PROGRAM% 13 13 24 > output.txt
IF NOT ERRORLEVEL 0 GOTO err
FC output.txt isoscelesTriangle.txt
IF ERRORLEVEL 1 GOTO err

echo simple triangle
%PROGRAM% 3 4 5 > output.txt
IF NOT ERRORLEVEL 0 GOTO err
FC output.txt simpleTriangle.txt
IF ERRORLEVEL 1 GOTO err

echo equilateral triangle
%PROGRAM% 2,519 2,519 2,519 > output.txt
IF NOT ERRORLEVEL 0 GOTO err
FC output.txt equilateralTriangle.txt
IF ERRORLEVEL 1 GOTO err

ECHO Program testing succeeded :-)
EXIT

:err
ECHO Program testing failed :-(
EXIT