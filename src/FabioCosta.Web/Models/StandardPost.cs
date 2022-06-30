namespace FabioCosta.Web.Models;

using FabioCosta.Web.Constants;
using FabioCosta.Web.Extensions;

using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Models;

using System;
using System.Collections.Generic;
using System.Linq;

[PostType(Title = "Standard post")]
public class StandardPost : Post<StandardPost>
{
    /// <summary>
    /// Gets/sets the available comments if these
    /// have been loaded from the database.
    /// </summary>
    public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

    /// <summary>
    /// Calculates AVG of time needed to read the post
    /// </summary>
    /// <returns></returns>
    public int GetTimeToRead()
    {
        List<Block> concernedBlocks = Blocks.Where(block => block is HtmlBlock || block is TextBlock).ToList();

        int totalWords = 0;
        concernedBlocks.ForEach(block =>
        {
            if (block is HtmlBlock)
            {
                var htmlBlock = block as HtmlBlock;
                totalWords += htmlBlock.Body.Value.WordCounter();
            }
            else if (block is TextBlock)
            {
                var textBlock = block as TextBlock;
                totalWords += textBlock.Body.Value.WordCounter();
            }
        });

        // AVG words read per minute
        var totalMinutes = Math.Ceiling((decimal) totalWords / DurationConstants.ReadingInMinutes.ReadingAvgEnglish);
        return Convert.ToInt32(totalMinutes);
    }
}
