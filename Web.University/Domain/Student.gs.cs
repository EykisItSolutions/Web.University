using System.ComponentModel.DataAnnotations.Schema;

namespace Web.University.Domain;

public partial class Student
{
    [NotMapped]
    public string FullName
    {
        get
        {
            return FirstName + " " + LastName;
        }
    }
}

