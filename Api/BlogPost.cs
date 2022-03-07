using System.Collections.Generic;

namespace Api
{
    public class BlogPost
    {
        public string Name { get; set; }
        public string[] Tags { get; set; }

        public BlogPost(string name)
        {
            Name = name;
        }
    }
}
