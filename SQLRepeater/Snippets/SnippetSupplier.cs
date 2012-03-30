using System.Collections.ObjectModel;

namespace SQLRepeater.Snippets
{
    public class SnippetSupplier
    {
        public static ObservableCollection<ValueCreatorDelegate> GetSnippetsForDataType(string dataType)
        {
            switch (dataType)
            {
                case "int":
                    return Snippets.IntSnippets.Snippets;

                case "smallint":
                    return Snippets.SmallIntSnippets.Snippets;

                case "tinyint":
                    return Snippets.TinyIntSnippets.Snippets;

                case "decimal":
                case "float":
                    return Snippets.DecimalSnippets.Snippets;

                case "datetime":
                case "datetime2":
                    return Snippets.DateTimeSnippets.Snippets;

                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    return Snippets.StringSnippets.Snippets;

                case "bit":
                    return Snippets.BooleanSnippets.Snippets;

                default:
                    return Snippets.StringSnippets.Snippets;
            }
        }
    }
}
