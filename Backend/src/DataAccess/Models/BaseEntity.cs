using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Models;

public class BaseEntity
{
    public Guid Id { get; set; }   
    public DateTime CreateDate { get; set; }
    public DateTime ModifyDate { get; set; }
}