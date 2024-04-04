
using System.IO;

namespace fault3r_Common
{
    public static class ResourceMemorizer
    {
        public static MemoryStream DefaultProfilePicture
        {
            get { return GetSource("DefaultProfilePicture.png"); }
        }

        public static MemoryStream DefaultForumPicture
        {
            get { return GetSource("DefaultForumPicture.png"); }
        }

        private static MemoryStream GetSource(string resource)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", resource);
            FileStream picture = new FileStream(filePath, FileMode.Open);
            MemoryStream picMemory = new();
            picture.CopyTo(picMemory);
            picture.Close();
            picMemory.Close();
            return picMemory;
        }

    }
}
