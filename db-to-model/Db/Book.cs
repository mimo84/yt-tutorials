using System;
using System.Collections.Generic;

namespace db_to_model.Db;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Isbn { get; set; }
}
