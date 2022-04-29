using System;
namespace MediaValet.Samples.SkyHOOK.WebApi.Models
{
	public class AssetMediaFileAddedEvent
	{
        public string LibraryId { get; set; }
        public string AssetId { get; set; }
        public int Version { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MediaFileBlobUrl { get; set; }
    }
}

