﻿namespace LibraryManagement.Application.Commands
{
    public class CreateBookCommand
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public string Genre { get; set; } = string.Empty;
    }
}
