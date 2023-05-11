using System;
using System.Collections.Generic;

namespace db_to_model.Db;

public partial class BookCategory
{
    public int CategoryId { get; set; }

    public int BookId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
