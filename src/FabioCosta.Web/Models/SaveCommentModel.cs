namespace FabioCosta.Web.Models
{
    using System;

    internal class SaveCommentModel
    {
        internal Guid Id { get; set; }
        internal string CommentAuthor { get; set; }
        internal string CommentEmail { get; set; }
        internal string CommentUrl { get; set; }
        internal string CommentBody { get; set; }
    }
}
