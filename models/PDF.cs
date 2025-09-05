using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiNetStudio.models
{
    using System;

    public enum PdfType
    {
        Patent = 0,
        Article = 1,   // scholarly article / paper
        Report = 2,   // tech report / whitepaper
        Book = 3,
        Other = 9
    }

    public class PdfEntry
    {
        public Guid PdfGuid { get; init; } = Guid.NewGuid();          // store as 16-byte BLOB in SQLite
        public string PdfGuidString => PdfGuid.ToString("N").ToUpperInvariant();
        public PdfType Type { get; set; } = PdfType.Other;
        public string PdfPath { get; set; } = string.Empty;             // RELATIVE (e.g., "Library/{GUID}.pdf")
        public string FileName { get; set; } = string.Empty;            // Original file name (e.g., "pat7505243.pdf")
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Authors { get; set; }
        public string Category { get; set; } = "Uncategorized";
        public string? SubCategory { get; set; }
        public string? Tags { get; set; }
        public string? Notes { get; set; }
        public int Rank { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? DOI { get; set; }
        public string? CountryCode { get; set; }
        public string? KindCode { get; set; }
        public string? PatentNumber { get; set; }
        public string? CPC { get; set; }
        public string? IPC { get; set; }
        public string? PriorityDataJson { get; set; }

    }

}
