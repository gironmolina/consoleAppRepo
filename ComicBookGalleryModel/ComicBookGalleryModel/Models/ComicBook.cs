using System;
using System.Collections.Generic;

namespace ComicBookGalleryModel.Models
{
    public sealed class ComicBook
    {
        public ComicBook()
        {
            Artist = new List<ComicBookArtist>();
        }

        public int Id { get; set; }
        public int SeriesId { get; set; }
        public int IssueNumber { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal? AverageRating { get; set; }

        #region Navigation
        public Series Series { get; set; }
        public ICollection<ComicBookArtist> Artist { get; set; } 
        #endregion

        public string DisplayText => $"{Series?.Title} #{IssueNumber}";

        public void AddArtist(Artist artist, Role role)
        {
            Artist.Add(new ComicBookArtist()
            {
                Artist = artist,
                Role = role
            });
        }
    }
}
