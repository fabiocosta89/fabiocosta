namespace FabioCosta.Web.Models
{
    using Piranha.AttributeBuilder;
    using Piranha.Models;

    [PageType(Title = "Standard archive", IsArchive = true)]
    public class StandardArchive : Page<StandardArchive>
    {
        /// <summary>
        /// The currently loaded post archive.
        /// </summary>
        public PostArchive<PostInfo> Archive { get; set; }
    }
}
