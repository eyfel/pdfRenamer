# pdfRenamer

This C# application utilizes the iTextSharp library to parse text from PDF files and detect codes in the format "KOD: ...". Once detected, the application renames the PDF file accordingly. For example, if a PDF contains "KOD: KRDL-13", the file is renamed to "KRDL-13.pdf".

The project is built using .NET 6.0 (Windows) and features a Windows Forms interface. It is compiled with the AnyCPU target, making it compatible with both 32-bit and 64-bit systems.

Use cases:

Automatically renaming technical drawings or order forms

Organizing documents based on specific code formats

Archiving and sorting PDF files efficiently

This automation simplifies PDF file management, eliminating the need for manual renaming and improving workflow efficiency.
