# symmetrical-bassoon
Interview question: Parse EDIFACT message

## Question
Taking the following EDIFACT message text, write some code to parse out the all the LOC
segments and populate an array with the 2 nd and 3 rd element of each segment.
Note: the ‘+’ is an element delimiter
```
UNA:+.? &#39;
UNB+UNOC:3+2021000969+4441963198+180525:1225+3VAL2MJV6EH9IX+KMSV7HMD+CUSDECU-
IE++1++1&#39;
UNH+EDIFACT+CUSDEC:D:96B:UN:145050&#39;
BGM+ZEM:::EX+09SEE7JPUV5HC06IC6+Z&#39;
LOC+17+IT044100&#39;
LOC+18+SOL&#39;
LOC+35+SE&#39;
LOC+36+TZ&#39;
LOC+116+SE003033&#39;
DTM+9:20090527:102&#39;
DTM+268:20090626:102&#39;
DTM+182:20090527:102&#39;
```

## Solution
My solution assumes that the source EDIFACT data is received as an `.edi` file, as opposed to a data stream, or any other data format such as JSON.

The solution parses the file into a string. The string is then read for a UNA segment to define the file's separators and creates a dictionary based on the segment, or uses the standard's defaults.

The EDIFACT string is parsed into arrays of the requested segments, `LOC`, and the elements of this array are then split by the message separator `+` producing the final `[5,2]` string array.
