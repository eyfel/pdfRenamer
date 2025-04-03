# pdfRenamer

This C# application utilizes the iTextSharp library to parse text from PDF files and detect codes in the format "KOD: ...". Once detected, the application renames the PDF file accordingly. For example, if a PDF contains "KOD: KRDL-13", the file is renamed to "KRDL-13.pdf". You can change the 'KOD' part.
