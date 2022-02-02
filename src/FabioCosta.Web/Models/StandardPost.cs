namespace FabioCosta.Web.Models;

using Piranha.AttributeBuilder;
using Piranha.Models;

using System.Collections.Generic;

[PostType(Title = "Standard post")]
public class StandardPost : Post<StandardPost>
{
    /// <summary>
    /// Gets/sets the available comments if these
    /// have been loaded from the database.
    /// </summary>
    public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
}
