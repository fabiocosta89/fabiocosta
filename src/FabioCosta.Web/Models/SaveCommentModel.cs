namespace FabioCosta.Web.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class SaveCommentModel
{
    [Required] 
    public Guid Id { get; set; }
    public string CommentAuthor { get; set; }
    public string CommentEmail { get; set; }
    public string CommentUrl { get; set; }
    public string CommentBody { get; set; }
}
