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

        //folder of stage image
        public string DirStageImage { get; set; } = "";

        //folder of Cms
        public string DirCms { get; set; } = "";
    }
}
