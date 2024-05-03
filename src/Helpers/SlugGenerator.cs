using System.Text.RegularExpressions;

// - written by Nada, commited by Sadeem.
public class SlugGenerator
{
    public static string GenerateSlug(string name)
    {
        // Convert the input to lowercase
        string slug = name.ToLower();

        // Replace spaces with hyphens
        slug = slug.Replace(" ", "-");

        // Remove any non-alphanumeric characters
        slug = Regex.Replace(slug, "[^a-z0-9-]", "");

        // Remove consecutive hyphens
        slug = Regex.Replace(slug, "-{2,}", "-");

        // Remove leading and trailing hyphens
        slug = slug.Trim('-');

        return slug;
    }
}
