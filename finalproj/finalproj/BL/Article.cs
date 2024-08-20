using finalproj.DAL;
using System.Runtime.CompilerServices;

namespace finalproj.BL
{
    public class Article
    {
        int infoId;
        string picture;
        string header;
        string contenct;
        string link;
        public Article(int infoId, string picture, string header, string contenct, string link)
        {
            InfoId = infoId;
            Picture = picture;
            Header = header;
            Contenct = contenct;
            Link = link;
        }
        public Article() { }

        public int InfoId { get => infoId; set => infoId = value; }
        public string Picture { get => picture; set => picture = value; }
        public string Header { get => header; set => header = value; }
        public string Contenct { get => contenct; set => contenct = value; }
        public string Link { get => link; set => link = value; }


        public List<Article> Read()
        {
            DBservicesArticle dbs = new DBservicesArticle();
            return dbs.GetAllArticles();
        }
    }
}
