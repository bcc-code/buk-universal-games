﻿using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface ITeamRepository
    {
        Team? GetTeamByCode(string code);
    }
}
