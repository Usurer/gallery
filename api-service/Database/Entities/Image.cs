﻿namespace Database.Entities
{
    public class Image
    {
        // TODO: Make it Required
        public long FileSystemItemId
        {
            get; set;
        }

        public required FileSystemItem FileSystemItem
        {
            get; set;
        }

        public required string Extension
        {
            get; set;
        }

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }
    }
}