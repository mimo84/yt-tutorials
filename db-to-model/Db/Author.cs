using System;
using System.Collections.Generic;

namespace db_to_model.Db;

public partial class Author
{
    public int AuthorId { get; set; }

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }
}
