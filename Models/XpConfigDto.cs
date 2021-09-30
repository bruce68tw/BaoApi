namespace BaoApi.Models
{
    /// <summary>
    /// get from appSettings.json XpConfig section
    /// </summary>
    public class XpConfigDto
    {
        //constructor
        public XpConfigDto()
        {
            //DirStageImage = "";
        }

        //db connect string
        public string DirStageImage { get; set; }

    }
}
