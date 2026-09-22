namespace CombatParser.Domain.Interfaces;

public interface IPlayerStats
{
    int Strength { get;  }

    int Agility { get;  }

    int Intelligence { get;  }

    int Stamina { get;  }

    int Dodge { get;  }

    int Parry { get;  }

    int Block { get;  }

    int Crit { get;  }

    int Haste { get;  }

    int Armor { get;  }

    string Talents { get;  }
}
