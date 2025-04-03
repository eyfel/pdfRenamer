# pdfRenamer

This C# application utilizes the iTextSharp library to parse text from PDF files and detect codes in the format "KOD: ...". Once detected, the application renames the PDF file accordingly. For example, if a PDF contains "KOD: KRDL-13", the file is renamed to "KRDL-13.pdf". You can change the 'KOD' part.

# Dependencies

## Required Libraries
- **iTextSharp (v5.5.13.3)** – Used for reading and manipulating PDF files.
- **.NET 6.0 (Windows)** – Required runtime for the project.
- **Windows Forms** – Provides the UI framework.

## Installation

- Install-Package iTextSharp -Version 5.5.13.3
