namespace KitLabelConverter.Concrete
{
  /// <summary>
  ///   Code 128
  ///   Convert an input string to the equivalent string including start and stop characters.
  ///   This object compresses the values to the shortest possible code 128 barcode format
  /// </summary>
  public static class BarcodeConverter128
  {
    /// <summary>
    ///   Converts an input string to the equivalent string, that need to be produced using the 'Code 128' font.
    /// </summary>
    /// <param name="value">String to be encoded</param>
    /// <returns>Encoded string start/stop and checksum characters included</returns>
    public static string StringToBarcode(string value)
    {
      bool isTableB = true, isValid = true;
      var returnValue = string.Empty;

      if (value.Length <= 0) return returnValue;
      // Check for valid characters
      int currentChar;
      for (var charCount = 0; charCount < value.Length; charCount++) {
        currentChar = char.Parse(value.Substring(charCount, 1));
        if (currentChar >= 32 && currentChar <= 126) continue;

        isValid = false;
        break;
      }

      if (!isValid) return returnValue;

      var charPos = 0;
      while (charPos < value.Length) {
        int minCharPos;
        if (isTableB) {
          // See if interesting to switch to table C
          // yes for 4 digits at start or end, else if 6 digits
          if (charPos == 0 || charPos + 4 == value.Length)
            minCharPos = 4;
          else
            minCharPos = 6;


          minCharPos = IsNumber(value, charPos, minCharPos);

          if (minCharPos < 0) {
            // Choice table C
            if (charPos == 0) {
              // Starting with table C
              returnValue = ((char) 205).ToString();
            }
            else {
              // Switch to table C
              returnValue = returnValue + ((char) 199);
            }
            isTableB = false;
          }
          else {
            if (charPos == 0) {
              // Starting with table B
              returnValue = ((char) 204).ToString();
            }
          }
        }

        if (!isTableB) {
          // We are on table C, try to process 2 digits
          minCharPos = 2;
          minCharPos = IsNumber(value, charPos, minCharPos);
          if (minCharPos < 0) // OK for 2 digits, process it
          {
            currentChar = int.Parse(value.Substring(charPos, 2));
            currentChar = currentChar < 95 ? currentChar + 32 : currentChar + 100;

            // In table C, value 00 is translated to char 32
            // Remap char 32 (space glyph) to char 194 (Â glyph)
            // for IDAutomation Code 128 font:
            if (currentChar == 32) { currentChar = 194; }

            returnValue = returnValue + ((char) currentChar);
            charPos += 2;
          }
          else {
            // We haven't 2 digits, switch to table B
            returnValue = returnValue + ((char) 200);
            isTableB = true;
          }
        }
        if (!isTableB) continue;
        // Process 1 digit with table B
        returnValue = returnValue + value.Substring(charPos, 1);
        charPos++;
      }

      // Calculation of the checksum
      var checksum = 0;
      for (var loop = 0; loop < returnValue.Length; loop++) {
        currentChar = char.Parse(returnValue.Substring(loop, 1));

        // Restore value 32 to current char 194 
        // so checksum calculation will be correct:
        if (currentChar == 194) { currentChar = 32; }

        currentChar = currentChar < 127 ? currentChar - 32 : currentChar - 100;
        if (loop == 0)
          checksum = currentChar;
        else
          checksum = (checksum + (loop*currentChar))%103;
      }

      // Calculation of the checksum ASCII code
      checksum = checksum < 95 ? checksum + 32 : checksum + 100;
      // Add the checksum and the STOP
      returnValue = returnValue +
                    ((char) checksum) +
                    ((char) 206);

      return returnValue;
    }

    private static int IsNumber(string inputValue, int charPos, int minCharPos)
    {
      // if the MinCharPos characters from CharPos are numeric, then MinCharPos = -1
      minCharPos--;
      if (charPos + minCharPos < inputValue.Length) {
        while (minCharPos >= 0) {
          if (char.Parse(inputValue.Substring(charPos + minCharPos, 1)) < 48
              || char.Parse(inputValue.Substring(charPos + minCharPos, 1)) > 57) {
            break;
          }
          minCharPos--;
        }
      }
      return minCharPos;
    }
  }

  /// <summary>
  ///   Code 39
  ///   Convert an input string to the equivilant string including start and stop characters.
  /// </summary>
  public static class BarcodeConverter39
  {
    /// <summary>
    ///   Converts an input string to the equivalent string, that need to be produced using the 'Code 3 de 9' font.
    /// </summary>
    /// <param name="value">String to be encoded</param>
    /// <returns>Encoded string start/stop characters included</returns>
    public static string StringToBarcode(string value)
    {
      return StringToBarcode(value, false);
    }

    /// <summary>
    ///   Converts an input string to the equivalent string, that need to be produced using the 'Code 3 de 9' font.
    /// </summary>
    /// <param name="value">String to be encoded</param>
    /// <param name="addChecksum">Is checksum to be added</param>
    /// <returns>Encoded string start/stop and checksum characters included</returns>
    public static string StringToBarcode(string value, bool addChecksum)
    {
      // Parameters : a string
      // Return     : a string which give the bar code when it is dispayed with CODE128.TTF font
      // 			 : an empty string if the supplied parameter is no good
      var isValid = true;
      var returnValue = string.Empty;
      var checksum = 0;
      if (value.Length > 0) {
        //Check for valid characters
        char currentChar;
        for (var charPos = 0; charPos < value.Length; charPos++) {
          currentChar = char.Parse(value.Substring(charPos, 1));
          if (!((currentChar >= '0' && currentChar <= '9') || (currentChar >= 'A' && currentChar <= 'Z') ||
                currentChar == ' ' || currentChar == '-' || currentChar == '.' || currentChar == '$' ||
                currentChar == '/' || currentChar == '+' || currentChar == '%')) {
            isValid = false;
            break;
          }
        }
        if (isValid) {
          // Add start char
          returnValue = "*";
          // Add other chars, and calc checksum
          for (var charPos = 0; charPos < value.Length; charPos++) {
            currentChar = char.Parse(value.Substring(charPos, 1));
            returnValue += currentChar.ToString();
            if (currentChar >= '0' && currentChar <= '9') {
              checksum = checksum + currentChar - 48;
            }
            else if (currentChar >= 'A' && currentChar <= 'Z') {
              checksum = checksum + currentChar - 55;
            }
            else {
              switch (currentChar) {
                case '-':
                  checksum = checksum + currentChar - 9;
                  break;
                case '.':
                  checksum = checksum + currentChar - 9;
                  break;
                case '$':
                  checksum = checksum + currentChar + 3;
                  break;
                case '/':
                  checksum = checksum + currentChar - 7;
                  break;
                case '+':
                  checksum = checksum + currentChar - 2;
                  break;
                case '%':
                  checksum = checksum + currentChar + 5;
                  break;
                case ' ':
                  checksum = checksum + currentChar + 6;
                  break;
              }
            }
          }
          // Calculation of the checksum ASCII code
          if (addChecksum) {
            checksum = checksum%43;
            if (checksum >= 0 && checksum <= 9) {
              returnValue += ((char) (checksum + 48)).ToString();
            }
            else if (checksum >= 10 && checksum <= 35) {
              returnValue += ((char) (checksum + 55)).ToString();
            }
            else {
              switch (checksum) {
                case 36:
                  returnValue += "-";
                  break;
                case 37:
                  returnValue += ".";
                  break;
                case 38:
                  returnValue += " ";
                  break;
                case 39:
                  returnValue += "$";
                  break;
                case 40:
                  returnValue += "/";
                  break;
                case 41:
                  returnValue += "+";
                  break;
                case 42:
                  returnValue += "%";
                  break;
              }
            }
          }
          // Add stop char
          returnValue += "*";
        }
      }
      return returnValue;
    }
  }
}