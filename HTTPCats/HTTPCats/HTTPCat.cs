using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace HTTPCats
{
    public class HTTPCat
    {
        private int _code;
        private MemoryStream _image;
        public int Code { get => _code; set => _code = value; }
        public MemoryStream Image { get => _image; set => _image = value; }
        [JsonConstructor]
        public HTTPCat(int c, string ms)
        {
            Code = c;
            byte[] b = Encoding.UTF8.GetBytes(ms);
            Image = new MemoryStream(b, 0, b.Length);
        }
    }
}
