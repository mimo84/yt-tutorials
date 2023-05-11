using System;
using System.Collections.Generic;

namespace db_to_model.Db;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public int? Parent { get; set; }

    public virtual ICollection<Category> InverseParentNavigation { get; set; } = new List<Category>();

    public virtual Category? ParentNavigation { get; set; }
}
