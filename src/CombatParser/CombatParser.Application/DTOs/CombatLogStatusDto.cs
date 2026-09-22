using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatParser.Application.DTOs;

public class CombatLogStatusDto
{
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public int Status { get; private set; }

    public int CombatLogId { get; set; }
}
